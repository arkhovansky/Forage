using System;

using Unity.Entities;
using UnityEngine;



namespace App.Game.ECS.GameTime.Settings {



[Serializable]
public struct GameTime_Settings : IComponentData
{
	/// <summary>
	/// Game-world hours per real-world second
	/// </summary>
	/// <remarks>The most accurate simulation is achieved when a game hour contains a whole number of simulation steps,
	/// i.e. time_scale / fixed_step_rate = 1/integer.</remarks>
	[Tooltip("Game-world hours per real-world second")]
	public float GameTimeScale;
}



}
