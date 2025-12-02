using UnityEngine;

using App.Game.ECS.GameTime.Settings;



namespace App.Infrastructure.External.Database.DomainSettings_Impl.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(DomainSettings),
                 menuName = AssetMenuNames.Database_+nameof(DomainSettings))]
public class DomainSettings : ScriptableObject
{
	public GameTime_Settings GameTime;
}



}
