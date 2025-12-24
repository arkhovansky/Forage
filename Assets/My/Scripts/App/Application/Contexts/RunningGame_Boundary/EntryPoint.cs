using Lib.AppFlow;
using Lib.AppFlow.Resolution;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services;
using App.Application.Contexts.RunningGame_Boundary.Composition.Impl;
using App.Game.Meta;
using App.Infrastructure.Shared.Contracts.Services;



namespace App.Application.Contexts.RunningGame_Boundary {



public class EntryPoint : IContextEntryPoint
{
	public static readonly EntryPoint Instance;

	static EntryPoint()
	{
		Instance = new EntryPoint();
	}


	//----------------------------------------------------------------------------------------------
	// IContextEntryPoint


	public IContextCapability Get_CapabilityDescriptor(IContextCapability_Builder builder)
	{
		return builder
			.Subject<IGameInstance>()
			.Field("intent", "Play")
			.Build();
	}


	public IContext Create(IContextRequest request, IContextData contextData)
	{
		var gameInstance = request.GetSubject<IGameInstance>();

		return Create(gameInstance, contextData);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private IContext Create(IGameInstance gameInstance, IContextData contextData)
	{
		var inGameMode = new InGameMode(contextData.Get<IEcsSystems_Service>());

		var loadedContextComposer = new LoadedContextComposer(contextData);

		return new RunningGame_Boundary_Context(
			gameInstance,
			inGameMode,
			loadedContextComposer,
			contextData.Get<IContextHost>(),
			contextData.Get<IMessageDispatcher>());
	}
}



}
