using System.Collections.Generic;

using UnityEngine;

using App.Game.Database;
using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(TerrainTypes),
                 menuName = AssetMenuNames.Root_Database_Domain_+nameof(TerrainTypes),
                 order = 2)]
public class TerrainTypes : ScriptableObject
{
	public List<TerrainType> List = null!;
}



}
