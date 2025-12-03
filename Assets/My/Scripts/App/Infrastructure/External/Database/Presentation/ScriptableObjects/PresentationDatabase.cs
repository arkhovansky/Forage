using UnityEngine;



namespace App.Infrastructure.External.Database.Presentation.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(PresentationDatabase),
                 menuName = AssetMenuNames.Database_GamePresentation_+nameof(PresentationDatabase))]
public class PresentationDatabase : ScriptableObject
{
	public TerrainTypes_Presentation TerrainTypes = null!;

	public Material TerrainGridMaterial = null!;

	public ResourceTypes_Presentation ResourceTypes = null!;
}



}
