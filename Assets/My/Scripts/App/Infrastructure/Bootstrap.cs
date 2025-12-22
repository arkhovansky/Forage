using UnityEngine;
using UnityEngine.UIElements;

using Cysharp.Threading.Tasks;

using Lib.AppFlow;
using Lib.AppFlow.Impl;
using Lib.AppFlow.Resolution.Impl;
using Lib.AppFlow.Resolution.Internal;
using Lib.UICore.Gui;
using Lib.UICore.Unity.Gui;
using Lib.UICore.Unity.Mvvm;

using App.Application.Contexts.Application._Infrastructure.Settings;
using App.Application.Contexts.Application.Settings;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services;
using App.Infrastructure.Shared.Contracts.Services;



namespace App.Infrastructure {



public class Bootstrap : MonoBehaviour
{
	[SerializeField] private ApplicationSettings_Asset ApplicationSettings = null!;

	//----------------------------------------------------------------------------------------------

	private IEcsSystems_Service _ecsSystems_Service = null!;

	private IContextHost_Internal _contextHost = null!;

	private IMessageDispatcher _messageDispatcher = null!;
	private IGui _gui = null!;

	private HostServices _hostServices = null!;

	private IApplicationSettings _applicationSettings = null!;

	private IContext _applicationContext = null!;

	private bool _isStarted;

	//----------------------------------------------------------------------------------------------


	private void Awake()
	{
		_ecsSystems_Service = new EcsSystems_Service();

		_contextHost = new ContextHost(
			new ContextDescriptorMatcher());

		_messageDispatcher = new MessageDispatcher();
		_gui = new Gui();
		var vvmBinder = new VvmBinder();

		_hostServices = new HostServices(_contextHost, _messageDispatcher, _gui, vvmBinder, _ecsSystems_Service);
		_contextHost.HostServices = _hostServices;

		_applicationSettings = new ApplicationSettings(ApplicationSettings);

		RegisterContexts();
	}


	// ReSharper disable once Unity.IncorrectMethodSignature
	// ReSharper disable once UnusedMember.Local
	private async UniTaskVoid Start()
	{
		_ecsSystems_Service.SetEcsSystemsEnabled(false);

		var contextRequest = _contextHost.New_ContextRequest()
			.Subject("Application")
			.Argument(_applicationSettings)
			.Build();
		_applicationContext = _contextHost.CreateContext(contextRequest);
		await _applicationContext.Start();

		_isStarted = true;
	}


	private void OnEnable()
	{
		// OnEnable is called after Live Reload when UIDocument's UXML asset is reloaded

		var uiDocument = GetComponent<UIDocument>();

		_gui.SetRootVisualNode(new UITKVisualNode(uiDocument.rootVisualElement));
	}


	private void Update()
	{
		if (!_isStarted)
			return;

		_messageDispatcher.Update();
		_applicationContext.Update();
	}


	private void LateUpdate()
	{
		if (!_isStarted)
			return;

		_applicationContext.LateUpdate();
	}


	//----------------------------------------------------------------------------------------------
	// private


	private void RegisterContexts()
	{
		_contextHost.RegisterContext(Application.Contexts.Application.EntryPoint.Instance);
		_contextHost.RegisterContext(Application.Contexts.RunningGame.EntryPoint.Instance);
	}
}



}
