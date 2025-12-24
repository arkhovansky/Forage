using System.Collections.Generic;

using UnityEngine;

using App.Game.Database;
using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(PlantResourceTypes),
                 menuName = AssetMenuNames.Root_Database_Domain_+nameof(PlantResourceTypes),
                 order = 3)]
public class PlantResourceTypes : ScriptableObject
{
	public List<PlantResourceType> List = null!;
}



}
