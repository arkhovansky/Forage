using Unity.Entities;

using Lib.Grid.Spatial;



namespace App.Game.ECS.Map.Components.Singletons {



public struct HexLayout3D_Component : IComponentData
{
	public readonly HexGridLayout_3D Layout;


	public HexLayout3D_Component(HexGridLayout_3D layout)
	{
		Layout = layout;
	}
}



}
