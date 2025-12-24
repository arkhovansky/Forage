using Cysharp.Threading.Tasks;

using Lib.AppFlow.Unity;

using App.Application.Contexts.Application.Settings;
using App.Game.Meta;
using App.Game.Meta.Impl;



namespace App.Application.Contexts.Application {



public class ApplicationContext : ApplicationContext_Base
{
	private readonly IApplicationSettings _settings;

	private IGameInstance? _gameInstance;



	public ApplicationContext(IApplicationSettings settings)
	{
		_settings = settings;
	}


	public override async UniTask Start()
	{
		var localeId = _settings.DefaultLocale;
		_gameInstance = new GameInstance(localeId);

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
