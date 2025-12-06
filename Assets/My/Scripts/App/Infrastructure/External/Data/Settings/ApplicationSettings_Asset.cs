using UnityEngine;



namespace App.Infrastructure.External.Data.Settings {



[CreateAssetMenu(fileName = "ApplicationSettings",
                 menuName = AssetMenuNames.Root_+"ApplicationSettings")]
public class ApplicationSettings_Asset : ScriptableObject
{
	public string DefaultLocale = null!;
}



}
