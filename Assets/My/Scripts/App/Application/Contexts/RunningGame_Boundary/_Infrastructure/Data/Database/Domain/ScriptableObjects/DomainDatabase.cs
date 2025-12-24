using UnityEngine;

using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(DomainDatabase),
                 menuName = AssetMenuNames.Root_Database_Domain_+nameof(DomainDatabase),
                 order = 0)]
public class DomainDatabase : ScriptableObject
{
	public TerrainTypes TerrainTypes = null!;

	public PlantResourceTypes PlantResourceTypes = null!;

	public SystemParameters SystemParameters = null!;

	public Locales Locales = null!;
}



}
