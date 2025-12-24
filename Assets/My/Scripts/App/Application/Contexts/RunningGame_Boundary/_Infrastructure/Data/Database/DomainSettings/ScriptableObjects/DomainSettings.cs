using UnityEngine;

using App.Game.ECS.GameTime.Settings;
using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.DomainSettings.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(DomainSettings),
                 menuName = AssetMenuNames.Root_Database_+nameof(DomainSettings))]
public class DomainSettings : ScriptableObject
{
	public GameTime_Settings GameTime;
}



}
