using UnityEngine;
using UnityEngine.UIElements;

using Lib.Grid;
using Lib.VisualGrid;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.HighLevel.Impl;
using App.Client.Framework.UICore.LowLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Client.Framework.UnityUICore.LowLevel;
using App.Client.Framework.UnityUICore.Mvvm;

using App.Client.UI;
using App.Services;
using App.Services.Resources;
using App.Services.Terrain;



public class Application : MonoBehaviour
{
	private IGui? _gui;
	private IVvmBinder? _vvmBinder;
	private ICommandRouter? _commandRouter;

	private ApplicationController? _applicationController;

	private const HexOrientation HexOrientation = Lib.Grid.HexOrientation.FlatTop;
	private HexLayout _hexLayout;
	private TerrainTypeRepository? _terrainTypeRepository;
	private TerrainTypePresentationRepository? _terrainTypePresentationRepository;
	private ResourceTypeRepository? _resourceTypeRepository;
	private ResourceTypePresentationRepository? _resourceTypePresentationRepository;
	private TerrainInitializer? _terrainInitializer;
	private ResourcesInitializer? _resourcesInitializer;
	private GameTimeInitializer? _gameTimeInitializer;
	private GameService? _gameService;



	private void Awake()
	{
		_gui = new Gui();
		_vvmBinder = new VvmBinder();
		_commandRouter = new CommandRouter();

		_hexLayout = new HexLayout(HexOrientation);
		_terrainTypeRepository = new TerrainTypeRepository();
		_terrainTypePresentationRepository = new TerrainTypePresentationRepository(_hexLayout);
		_resourceTypeRepository = new ResourceTypeRepository();
		_resourceTypePresentationRepository = new ResourceTypePresentationRepository();
		_terrainInitializer = new TerrainInitializer(_hexLayout, _terrainTypePresentationRepository);
		_resourcesInitializer = new ResourcesInitializer(_resourceTypeRepository);
		_gameTimeInitializer = new GameTimeInitializer();
		_gameService = new GameService(
			_terrainInitializer, _resourcesInitializer, _gameTimeInitializer,
			_hexLayout, _resourceTypePresentationRepository);
	}



	private void Start()
	{
		_applicationController = new ApplicationController(
			_hexLayout,
			_terrainTypeRepository!, _resourceTypeRepository!,
			_gameService!,
			_gui!, _vvmBinder!, _commandRouter!);
		// _commandRouter.SetRootController(applicationController);
		_applicationController.Start();
	}



	private void OnEnable()
	{
		// OnEnable is called after Live Reload when UIDocument's UXML asset is reloaded

		var uiDocument = GetComponent<UIDocument>();

		_gui!.SetRootVisualNode(new UITKVisualNode(uiDocument.rootVisualElement));
	}


	private void Update()
	{
		_commandRouter!.Update();
		_applicationController!.Update();
	}


	private void LateUpdate()
	{
		_applicationController!.UpdateViewModels();
	}
}
