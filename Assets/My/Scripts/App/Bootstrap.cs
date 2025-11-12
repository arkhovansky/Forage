using UnityEngine;
using UnityEngine.UIElements;

using Cysharp.Threading.Tasks;

using Lib.Grid;
using Lib.VisualGrid;
using Lib.Math;

using App.Application.Framework.UICore.Flow;
using App.Application.Framework.UICore.Flow.Impl;
using App.Application.Framework.UICore.Gui;
using App.Application.Framework.UICore.Mvvm;
using App.Application.Framework.UnityUICore.Gui;
using App.Application.Framework.UnityUICore.Mvvm;

using App.Application.Flow;
using App.Infrastructure.EcsGateway.Services;
using App.Infrastructure.EcsGateway.Services.RunningGameInitializer_Impl;
using App.Infrastructure.External.Database_Impl;
using App.Infrastructure.External.PresentationDatabase_Impl;



namespace App {



public class Bootstrap : MonoBehaviour
{
	private IGui? _gui;
	private IVvmBinder? _vvmBinder;
	private ICommandRouter? _commandRouter;

	private ApplicationController? _applicationController;

	private const HexOrientation HexOrientation = Lib.Grid.HexOrientation.FlatTop;
	private HexLayout3D _hexLayout;
	private TerrainTypeRepository? _terrainTypeRepository;
	private TerrainTypePresentationRepository? _terrainTypePresentationRepository;
	private ResourceTypeRepository? _resourceTypeRepository;
	private ResourceTypePresentationRepository? _resourceTypePresentationRepository;
	private BandMemberTypeRepository? _bandMemberTypeRepository;
	private TerrainInitializer? _terrainInitializer;
	private ResourcesInitializer? _resourcesInitializer;
	private ResourcePresentationInitializer? _resourcePresentationInitializer;
	private GameTimeInitializer? _gameTimeInitializer;
	private BandInitializer? _bandInitializer;
	private RunningGameInitializer? _runningGameInitializer;

	private bool _isStarted;



	private void Awake()
	{
		_gui = new Gui();
		_vvmBinder = new VvmBinder();
		_commandRouter = new CommandRouter();

		_hexLayout = new HexLayout3D(
			new HexLayout(HexOrientation),
			new Matrix3x2(Vector3.right, Vector3.forward));
		_terrainTypeRepository = new TerrainTypeRepository();
		_terrainTypePresentationRepository = new TerrainTypePresentationRepository(_hexLayout);
		_resourceTypeRepository = new ResourceTypeRepository();
		_resourceTypePresentationRepository = new ResourceTypePresentationRepository();
		_bandMemberTypeRepository = new BandMemberTypeRepository();
		_terrainInitializer = new TerrainInitializer(_hexLayout, _terrainTypePresentationRepository);
		_resourcesInitializer = new ResourcesInitializer(_resourceTypeRepository);
		_resourcePresentationInitializer = new ResourcePresentationInitializer(_resourceTypePresentationRepository);
		_gameTimeInitializer = new GameTimeInitializer();
		_bandInitializer = new BandInitializer(_bandMemberTypeRepository);
		_runningGameInitializer = new RunningGameInitializer(
			_terrainInitializer, _resourcesInitializer, _resourcePresentationInitializer, _gameTimeInitializer,
			_bandInitializer, _hexLayout);
	}



	// ReSharper disable once Unity.IncorrectMethodSignature
	private async UniTaskVoid Start()
	{
		EcsService.SetEcsSystemsEnabled(false);

		_applicationController = new ApplicationController(
			_hexLayout,
			_terrainTypeRepository!, _resourceTypeRepository!, _bandMemberTypeRepository!,
			_runningGameInitializer!,
			_gui!, _vvmBinder!, _commandRouter!);
		// _commandRouter.SetRootController(applicationController);
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
