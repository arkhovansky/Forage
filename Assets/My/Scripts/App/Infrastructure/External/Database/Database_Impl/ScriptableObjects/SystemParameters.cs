using UnityEngine;

using App.Game.ECS.BandMember.Gathering.Rules;
using App.Game.ECS.BandMember.Movement.Rules;
using App.Game.ECS.GameTime.Rules;



namespace App.Infrastructure.External.Database.Database_Impl.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(SystemParameters),
                 menuName = AssetMenuNames.Database_Domain_+nameof(SystemParameters))]
public class SystemParameters : ScriptableObject
{
	public GameTime_Rules GameTime;

	public Daylight_Rules Daylight;

	public Movement_Rules Movement;

	public Gathering_Rules Gathering;
}



}
