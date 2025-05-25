using System.Collections.Generic;

using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Properties;

using Lib.Grid;

using App.Client.Framework.UICore.HighLevel;
using App.Client.Framework.UICore.Mvvm;
using App.Game.ECS.BandMember.Components;
using App.Game.ECS.Components;
using App.Game.ECS.GameTime.Components;
using App.Game.ECS.Resource.Plant.Components;
using App.Game.ECS.Terrain.Components;
using App.Game.ECS.UI.HoveredTile.Components;
using App.Services.BandMembers;
using App.Services.Resources;
using App.Services.Terrain;



namespace App.Client.UI.GameInstance.RunningGame {



public class GameVM : IViewModel
{
	[CreateProperty]
	public string Month { get; private set; }

	public List<BandMemberVM> BandMembers { get; }

	public TileInfoVM TileInfoVM { get; }

	public EnterPlaceCampMode_CommandVM EnterPlaceCampModeCommand { get; }
	public EndTurn_CommandVM EndTurnCommand { get; }



	private readonly IBandMemberTypeRepository _bandMemberTypeRepository;



	public GameVM(IController controller,
	              ICommandRouter commandRouter,
	              ITerrainTypeRepository terrainTypeRepository,
	              IResourceTypeRepository resourceTypeRepository,
	              IBandMemberTypeRepository bandMemberTypeRepository)
	{
		Month = string.Empty;

		BandMembers = new List<BandMemberVM>();

		TileInfoVM = new TileInfoVM(terrainTypeRepository, resourceTypeRepository);

		EnterPlaceCampModeCommand = new EnterPlaceCampMode_CommandVM(
			() => commandRouter.EmitCommand(new EnterPlaceCampMode(), controller));
		EndTurnCommand = new EndTurn_CommandVM(
			() => commandRouter.EmitCommand(new EndTurnCommand(), controller));

		_bandMemberTypeRepository = bandMemberTypeRepository;
	}


	public void Update()
	{
		UpdateYearPeriod();
		UpdateBandMembers();
		TileInfoVM.Update();
	}


	private void UpdateYearPeriod()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<CurrentYearPeriod>());
		var yearPeriod = query.GetSingleton<CurrentYearPeriod>();

		Month = yearPeriod.Value.Month.ToString();
	}


	private void UpdateBandMembers()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(
			ComponentType.ReadOnly<BandMember>(),
			ComponentType.ReadOnly<Human>(),
			ComponentType.ReadOnly<Forager>());

		var bandMembers = query.ToComponentDataArray<BandMember>(Allocator.Temp);
		var humans = query.ToComponentDataArray<Human>(Allocator.Temp);
		var foragers = query.ToComponentDataArray<Forager>(Allocator.Temp);

		if (BandMembers.Count == 0) {
			for (var i = 0; i < bandMembers.Length; i++) {
				BandMembers.Add(new BandMemberVM() {
					Id = bandMembers[i].Id,
					Gender = _bandMemberTypeRepository.Get(humans[i].TypeId).Gender.ToString(),
					Assignment = foragers[i].Activity.ToString()
				});
			}
		}
		else {
			Assert.IsTrue(BandMembers.Count == bandMembers.Length);

			for (var i = 0; i < bandMembers.Length; i++) {
				var bandMemberVM = BandMembers.Find(x => x.Id == bandMembers[i].Id);

				bandMemberVM.Gender = _bandMemberTypeRepository.Get(humans[i].TypeId).Gender.ToString();
				bandMemberVM.Assignment = foragers[i].Activity.ToString();
			}
		}
	}
}



public class TileInfoVM : IViewModel
{
	[CreateProperty]
	public string TerrainType { get; private set; }


	public ResourceInfoVM ResourceInfoVM { get; }



	private readonly ITerrainTypeRepository _terrainTypeRepository;



	public TileInfoVM(ITerrainTypeRepository terrainTypeRepository,
	                  IResourceTypeRepository resourceTypeRepository)
	{
		_terrainTypeRepository = terrainTypeRepository;

		TerrainType = string.Empty;

		ResourceInfoVM = new ResourceInfoVM(resourceTypeRepository);
	}


	public void Update()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<HoveredTileEntity>());

		if (query.TryGetSingleton<HoveredTileEntity>(out var hoveredTileEntityComponent)) {
			var hoveredTileEntity = hoveredTileEntityComponent.Entity;

			var terrainTileComponent = entityManager.GetComponentData<TerrainTile>(hoveredTileEntity);
			var terrainTypeId = terrainTileComponent.TerrainType;

			var terrainType = _terrainTypeRepository.Get(terrainTypeId);
			TerrainType = terrainType.Name;


			var position = entityManager.GetComponentData<TilePosition>(hoveredTileEntity).Position;

			if (TryGetResource(position, out Entity resourceEntity))
				ResourceInfoVM.Show(resourceEntity);
			else
				ResourceInfoVM.Hide();
		}
		else {
			TerrainType = string.Empty;
			ResourceInfoVM.Hide();
		}
	}


	private bool TryGetResource(AxialPosition position, out Entity resourceEntity)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var query = entityManager.CreateEntityQuery(
			ComponentType.ReadOnly<TilePosition>(),
			ComponentType.ReadOnly<PlantResource>());

		var positions = query.ToComponentDataArray<TilePosition>(Allocator.Temp);
		var entities = query.ToEntityArray(Allocator.Temp);

		for (var i = 0; i < positions.Length; i++) {
			if (positions[i].Position == position) {
				resourceEntity = entities[i];
				return true;
			}
		}

		resourceEntity = Entity.Null;
		return false;
	}
}



public class ResourceInfoVM : IViewModel
{
	[CreateProperty]
	public bool IsVisible { get; private set; }

	[CreateProperty]
	public string Name { get; private set; }

	[CreateProperty]
	public uint PotentialBiomass { get; private set; }

	[CreateProperty]
	public string RipenessPeriod { get; private set; }

	[CreateProperty]
	public uint RipeBiomass { get; private set; }



	private readonly IResourceTypeRepository _resourceTypeRepository;



	public ResourceInfoVM(IResourceTypeRepository resourceTypeRepository)
	{
		_resourceTypeRepository = resourceTypeRepository;

		Name = string.Empty;
		RipenessPeriod = string.Empty;
	}


	public void Show(Entity resourceEntity)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

		var resource = entityManager.GetComponentData<PlantResource>(resourceEntity);

		var resourceType = _resourceTypeRepository.Get(resource.TypeId);

		Name = resourceType.Name;
		PotentialBiomass = (uint) resource.PotentialBiomass;
		RipenessPeriod = resource.RipenessPeriod.Month.ToString();

		RipeBiomass = (uint) entityManager.GetComponentData<RemainingRipeBiomass>(resourceEntity).Value;

		IsVisible = true;
	}


	public void Hide()
	{
		IsVisible = false;
	}
}




}
