using System;
using System.Collections.Generic;

using Lib.AppFlow.Impl;
using Lib.AppFlow.Internal;
using Lib.AppFlow.Resolution.Internal;



namespace Lib.AppFlow.Resolution.Impl {



public class ContextHost : IContextHost_Internal
{
	private readonly IContextDescriptorMatcher _descriptorMatcher;

	private readonly List<IContextEntryPoint> _contextEntryPoints = new();

	//----------------------------------------------------------------------------------------------


	public ContextHost(IContextDescriptorMatcher descriptorMatcher)
	{
		_descriptorMatcher = descriptorMatcher;
	}


	//----------------------------------------------------------------------------------------------
	// IContextHost_Internal


	public void RegisterContext(IContextEntryPoint entryPoint)
	{
		_contextEntryPoints.Add(entryPoint);
	}


	public IContext CreateRootContext(IContextRequest request, IContextData contextData)
	{
		return CreateContext(request, contextData);
	}


	//----------------------------------------------------------------------------------------------
	// IContextHost


	public IContextCapability_Builder New_ContextCapability()
		=> new ContextCapability();

	public IContextRequest_Builder New_ContextRequest()
		=> new ContextRequest();


	public IContext CreateContext(IContextRequest request, IContext parent)
	{
		var contextData = new ContextData(((IContext_Internal)parent).ContextData);
		return CreateContext(request, contextData);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private IContextEntryPoint Resolve_ContextRequest(IContextRequest request)
	{
		foreach (var entryPoint in _contextEntryPoints) {
			var capability = entryPoint.Get_CapabilityDescriptor(New_ContextCapability());
			if (_descriptorMatcher.Satisfies(capability, request))
				return entryPoint;
		}

		throw new Exception("Context not found");
	}


	private IContext CreateContext(IContextRequest request, IContextData contextData)
	{
		var entryPoint = Resolve_ContextRequest(request);
		var context = entryPoint.Create(request, contextData);
		((IContext_Internal)context).Init(contextData.Get<IMessageDispatcher>(),
		                                  contextData,
		                                  this);
		return context;
	}
}



}
