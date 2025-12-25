using Cysharp.Threading.Tasks;

using Lib.AppFlow.Unity;

using App.Application.Contexts.Application.Services;
using App.Application.Contexts.Application.Settings;
using App.Game.Meta;



namespace App.Application.Contexts.Application {



public class ApplicationContext : ApplicationContext_Base
{
	private readonly IApplicationSettings _settings;

	private readonly IGameInstance_Factory _gameInstance_Factory;

	private IGameInstance? _gameInstance;



	public ApplicationContext(IApplicationSettings settings,
	                          IGameInstance_Factory gameInstance_Factory)
	{
		_settings = settings;
		_gameInstance_Factory = gameInstance_Factory;
	}


	public override async UniTask Start()
	{
		_gameInstance = _gameInstance_Factory.Create();
		_gameInstance.Setup.LocaleId = _settings.DefaultLocale;

		var childRequest = ContextHost.New_ContextRequest()
			.Subject(_gameInstance)
			.Field("intent", "Play")
			.Build();
		var child = ContextHost.CreateContext(childRequest, this);
		AddChildContext(child);
		await child.Start();
	}
}



}
