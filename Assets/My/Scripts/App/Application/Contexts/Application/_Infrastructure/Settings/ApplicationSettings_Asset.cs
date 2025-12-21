using UnityEngine;

using App.Infrastructure.Data;



namespace App.Application.Contexts.Application._Infrastructure.Settings {



[CreateAssetMenu(fileName = "ApplicationSettings",
                 menuName = AssetMenuNames.Root_+"ApplicationSettings")]
public class ApplicationSettings_Asset : ScriptableObject
{
	public string DefaultLocale = null!;
}



}
