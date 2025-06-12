using Unity.Entities;

using Lib.VisualGrid;



namespace App.Game.ECS.Map.Components.Singletons {



public struct HexLayout3D_Component : IComponentData
{
	public readonly HexLayout3D Layout;


	public HexLayout3D_Component(HexLayout3D layout)
	{
		Layout = layout;
	}
}



}
