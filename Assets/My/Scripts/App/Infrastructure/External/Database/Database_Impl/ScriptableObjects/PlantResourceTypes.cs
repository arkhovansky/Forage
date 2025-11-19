using System.Collections.Generic;

using UnityEngine;

using App.Game.Database;



namespace App.Infrastructure.External.Database.Database_Impl.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(PlantResourceTypes),
                 menuName = AssetMenuNames.Database_Domain_+nameof(PlantResourceTypes))]
public class PlantResourceTypes : ScriptableObject
{
	public List<PlantResourceType> List = null!;
}



}
