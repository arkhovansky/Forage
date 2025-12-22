using Lib.AppFlow;
using Lib.AppFlow.Resolution;

using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services;
using App.Application.Contexts.RunningGame.Composition.Impl;
using App.Game.Meta;
using App.Infrastructure;



namespace App.Application.Contexts.RunningGame {



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


	public IContext Create(IContextRequest request, IHostServices hostServices)
	{
		var gameInstance = request.GetSubject<IGameInstance>();

		return Create(gameInstance, (HostServices) hostServices);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private IContext Create(IGameInstance gameInstance, HostServices hostServices)
	{
		var inGameMode = new InGameMode(hostServices.EcsSystems_Service);

		var loadedContextComposer = new LoadedContextComposer(
			hostServices.EcsSystems_Service, hostServices.Gui, hostServices.VvmBinder);

		return new RunningGameContext(gameInstance,
		                              hostServices.MessageDispatcher,
		                              inGameMode,
		                              loadedContextComposer);
	}
}



}
