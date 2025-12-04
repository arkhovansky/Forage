using System.Collections.Generic;

using UnityEngine;

using App.Game.Database;



namespace App.Infrastructure.External.Database.Domain.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(TerrainTypes),
                 menuName = AssetMenuNames.Root_Database_Domain_+nameof(TerrainTypes),
                 order = 2)]
public class TerrainTypes : ScriptableObject
{
	public List<TerrainType> List = null!;
}



}
