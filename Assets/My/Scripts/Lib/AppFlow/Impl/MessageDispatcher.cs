using System;
using System.Collections.Generic;

using Lib.AppFlow.Internal;



namespace Lib.AppFlow.Impl {



public class MessageDispatcher : IMessageDispatcher
{
	private record MessageInfo(
		IMessage Message,
		IContext Emitter
	);

	private readonly Queue<MessageInfo> _messages = new();


	//----------------------------------------------------------------------------------------------
	// IMessageDispatcher implementation


	public void Emit(IMessage message, IContext emitter)
	{
		_messages.Enqueue(new MessageInfo(message, emitter));
	}


	public void Update()
	{
		while (_messages.Count > 0) {
			var messageInfo = _messages.Dequeue();
			DispatchMessage(messageInfo);
		}
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void DispatchMessage(MessageInfo messageInfo)
	{
		DispatchMessage(messageInfo.Message, messageInfo.Emitter);
	}


	private void DispatchMessage(IMessage message, IContext emitter)
	{
		switch (message) {
			case IInputEvent inputEvent: Dispatch(inputEvent, emitter); break;
			case ICommand command: Dispatch(command, emitter); break;
			case IDomainEvent domainEvent: Dispatch(domainEvent, emitter); break;
			case IPresentationEvent presentationEvent: Dispatch(presentationEvent, emitter); break;

			default: throw new ArgumentOutOfRangeException(nameof(message), message, null);
		}
	}


	private void Dispatch(IInputEvent inputEvent, IContext context)
	{
		var handler = Find_InputEvent_Handler(inputEvent.GetType(), context);
		handler?.DynamicInvoke(inputEvent);
	}


	private void Dispatch(ICommand command, IContext emitter)
	{
		IContext? context = emitter;
		bool searchInController = true;

		do {
			var handler = Find_Command_Handler(command.GetType(), context, searchInController);

			if (handler != null) {
				handler.DynamicInvoke(command);
				return;
			}

			context = context.Parent;
			searchInController = false;
		}
		while (context != null);
	}


	private void Dispatch(IDomainEvent domainEvent, IContext context)
	{
		throw new NotImplementedException();
	}


	private void Dispatch(IPresentationEvent presentationEvent, IContext context)
	{
		var context_Internal = (IContext_Internal) context;

		foreach (var view in context_Internal.Views) {
			var handler = Find_PresentationEvent_Handler(presentationEvent.GetType(), view);
			handler?.DynamicInvoke(presentationEvent);
		}
	}


	private Delegate? Find_InputEvent_Handler(Type messageType, IContext context)
	{
		var context_Internal = (IContext_Internal) context;

		if (context_Internal.Controller != null &&
		    ((IController_Internal)context_Internal.Controller).InputEvent_Handlers.TryGetValue(
			    messageType, out var handler))
		{
			return handler;
		}

		return null;
	}


	private Delegate? Find_Command_Handler(Type messageType, IContext context, bool searchInController)
	{
		var context_Internal = (IContext_Internal) context;

		Delegate handler;

		if (searchInController) {
			if (context_Internal.Controller != null &&
			    ((IController_Internal)context_Internal.Controller).Command_Handlers.TryGetValue(
				    messageType, out handler))
			{
				return handler;
			}
		}

		if (context_Internal.Command_Handlers.TryGetValue(messageType, out handler))
			return handler;

		return null;
	}


	private Delegate? Find_PresentationEvent_Handler(Type messageType, IView view)
	{
		return ((IView_Internal) view).PresentationEvent_Handlers.GetValueOrDefault(messageType);
	}
}



}
