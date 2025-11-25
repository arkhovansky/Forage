using UnityEngine;



namespace App.Infrastructure.External.Database.Database_Impl.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(DomainDatabase),
                 menuName = AssetMenuNames.Database_Domain_+nameof(DomainDatabase))]
public class DomainDatabase : ScriptableObject
{
	public TerrainTypes TerrainTypes = null!;

	public PlantResourceTypes PlantResourceTypes = null!;

	public SystemParameters SystemParameters = null!;
}



}
