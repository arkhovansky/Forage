using UnityEngine;
using UnityEngine.UIElements;

using Cysharp.Threading.Tasks;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Flow.Impl;
using App.Application.Framework.UICore.Gui;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Framework.UnityUICore.Gui;
using App.Application.Framework.UnityUICore.Mvvm;

using App.Application.Flow;
using App.Infrastructure.EcsGateway.Services;



namespace App {



public class Bootstrap : MonoBehaviour
{
	private IGui? _gui;
	private IVvmBinder? _vvmBinder;
	private ICommandRouter? _commandRouter;

	private ApplicationController? _applicationController;

	private bool _isStarted;



	private void Awake()
	{
		_gui = new Gui();
		_vvmBinder = new VvmBinder();
		_commandRouter = new CommandRouter();
	}



	// ReSharper disable once Unity.IncorrectMethodSignature
	private async UniTaskVoid Start()
	{
		EcsService.SetEcsSystemsEnabled(false);

		_applicationController = new ApplicationController(
			_gui!, _vvmBinder!, _commandRouter!);
		await _applicationController.Start();

		_isStarted = true;
	}



	private void OnEnable()
	{
		// OnEnable is called after Live Reload when UIDocument's UXML asset is reloaded

		var uiDocument = GetComponent<UIDocument>();

		_gui!.SetRootVisualNode(new UITKVisualNode(uiDocument.rootVisualElement));
	}


	private void Update()
	{
		if (!_isStarted)
			return;

		_commandRouter!.Update();
		_applicationController!.Update();
	}


	private void LateUpdate()
	{
		if (!_isStarted)
			return;

		_applicationController!.UpdateViewModels();
	}
}



}
