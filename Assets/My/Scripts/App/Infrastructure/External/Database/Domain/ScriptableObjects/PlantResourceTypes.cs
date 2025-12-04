using System.Collections.Generic;

using UnityEngine;

using App.Game.Database;



namespace App.Infrastructure.External.Database.Domain.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(PlantResourceTypes),
                 menuName = AssetMenuNames.Root_Database_Domain_+nameof(PlantResourceTypes),
                 order = 3)]
public class PlantResourceTypes : ScriptableObject
{
	public List<PlantResourceType> List = null!;
}



}
