using Cysharp.Threading.Tasks;

using Lib.AppFlow;
using Lib.AppFlow.Resolution;
using Lib.AppFlow.Unity;

using App.Application.Contexts.Application.Settings;
using App.Game.Meta;
using App.Game.Meta.Impl;



namespace App.Application.Contexts.Application {



public class ApplicationContext : ApplicationContext_Base
{
	private readonly IApplicationSettings _settings;

	private readonly IContextHost _contextHost;

	private IGameInstance? _gameInstance;



	public ApplicationContext(IApplicationSettings settings,
	                          IMessageDispatcher messageDispatcher,
	                          IContextHost contextHost)
		: base(messageDispatcher)
	{
		_settings = settings;
		_contextHost = contextHost;
	}


	public override async UniTask Start()
	{
		var localeId = _settings.DefaultLocale;
		_gameInstance = new GameInstance(localeId);

		var childRequest = _contextHost.New_ContextRequest()
			.Subject(_gameInstance)
			.Field("intent", "Play")
			.Build();
		var child = _contextHost.CreateContext(childRequest);
		AddChildContext(child);
		await child.Start();
	}
}



}
