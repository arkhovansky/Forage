using UnityEngine;

using App.Game.ECS.GameTime.Settings;



namespace App.Infrastructure.External.Data.Database.DomainSettings.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(DomainSettings),
                 menuName = AssetMenuNames.Root_Database_+nameof(DomainSettings))]
public class DomainSettings : ScriptableObject
{
	public GameTime_Settings GameTime;
}



}
