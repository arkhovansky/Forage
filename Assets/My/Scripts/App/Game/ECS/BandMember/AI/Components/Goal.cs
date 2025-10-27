using Unity.Entities;



namespace App.Game.ECS.BandMember.AI.Components {



public enum Goal
{
	Forage,
	Leisure,
	Sleep
}


public struct GoalComponent : IComponentData, IEnableableComponent
{
	public Goal Goal;


	public GoalComponent(Goal goal)
	{
		Goal = goal;
	}
}



}
