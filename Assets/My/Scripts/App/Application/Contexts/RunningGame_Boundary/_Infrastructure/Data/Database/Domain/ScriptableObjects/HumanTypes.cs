using System;
using System.Collections.Generic;

using UnityEngine;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Game.Database;
using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(HumanTypes),
                 menuName = AssetMenuNames.Root_Database_Domain_+nameof(HumanTypes),
                 order = 4)]
public class HumanTypes : ScriptableObject
{
	[Serializable]
	public class HumanType_Data
	{
		public HumanTypeId Id;

		public Gender Gender;

		[Tooltip("Energy required daily, kcal")]
		public uint EnergyRequiredDaily;

		[Tooltip("Base movement speed, km/h")]
		public float BaseSpeed;

		[Tooltip("Gathering speed, kg/h")]
		public float GatheringSpeed;
	}


	public List<HumanType_Data> List = null!;
}



}
