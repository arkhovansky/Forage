using Unity.Entities;

using App.Game.Database;



namespace App.Game.ECS.BandMember.General.Components {



public readonly struct Human : IComponentData
{
	public readonly HumanTypeId TypeId;


	public Human(HumanTypeId humanTypeId)
	{
		TypeId = humanTypeId;
	}
}



}
