using UnityEngine;
using UnityEngine.UIElements;

using Cysharp.Threading.Tasks;

using Lib.AppFlow;
using Lib.AppFlow.Impl;
using Lib.UICore.Gui;
using Lib.UICore.Mvvm;
using Lib.UICore.Unity.Gui;
using Lib.UICore.Unity.Mvvm;

using App.Application.Contexts.Application;
using App.Application.Contexts.Application._Infrastructure.Settings;
using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Services;
using App.Infrastructure.Shared.Contracts.Services;



namespace App.Infrastructure {



public class Bootstrap : MonoBehaviour
{
	[SerializeField] private ApplicationSettings_Asset ApplicationSettings = null!;


	private ApplicationSettings _applicationSettings = null!;

	private IEcsSystems_Service _ecsSystems_Service = null!;

	private IGui _gui = null!;
	private IVvmBinder _vvmBinder = null!;
	private IMessageDispatcher _messageDispatcher = null!;

	private ApplicationContext _applicationContext = null!;

	private bool _isStarted;



	private void Awake()
	{
		_applicationSettings = new ApplicationSettings(ApplicationSettings);

		_ecsSystems_Service = new EcsSystems_Service();

		_gui = new Gui();
		_vvmBinder = new VvmBinder();
		_messageDispatcher = new MessageDispatcher();
	}



	// ReSharper disable once Unity.IncorrectMethodSignature
	// ReSharper disable once UnusedMember.Local
	private async UniTaskVoid Start()
	{
		_ecsSystems_Service.SetEcsSystemsEnabled(false);

		_applicationContext = new ApplicationContext(
			_applicationSettings,
			_ecsSystems_Service,
			_gui, _vvmBinder, _messageDispatcher);
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
}



}
