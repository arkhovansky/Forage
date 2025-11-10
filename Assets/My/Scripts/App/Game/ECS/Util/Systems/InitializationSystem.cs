using Unity.Burst;
using Unity.Entities;

using App.Game.ECS.Util.Components;



namespace App.Game.ECS.Util.Systems {



/// <summary>
/// On creation creates the singleton entity tagged with <see cref="SingletonEntity_Tag"/>.
/// </summary>
public partial struct InitializationSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.EntityManager.CreateSingleton<SingletonEntity_Tag>();

		state.Enabled = false;
	}
}



}
