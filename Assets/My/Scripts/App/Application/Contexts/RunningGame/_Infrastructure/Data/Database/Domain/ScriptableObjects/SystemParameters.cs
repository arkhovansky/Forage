using UnityEngine;

using App.Game.ECS.BandMember.Gathering.Rules;
using App.Game.ECS.BandMember.Movement.Rules;
using App.Game.ECS.GameTime.Rules;
using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame._Infrastructure.Data.Database.Domain.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(SystemParameters),
                 menuName = AssetMenuNames.Root_Database_Domain_+nameof(SystemParameters),
                 order = 1)]
public class SystemParameters : ScriptableObject
{
	public GameTime_Rules GameTime;

	public Daylight_Rules Daylight;

	public Movement_Rules Movement;

	public Gathering_Rules Gathering;
}



}
