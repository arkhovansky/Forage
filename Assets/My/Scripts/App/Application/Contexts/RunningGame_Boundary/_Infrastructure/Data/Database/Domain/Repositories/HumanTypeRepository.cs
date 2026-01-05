using System;
using System.Collections.Generic;
using System.Linq;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.ScriptableObjects;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Contracts.Database.Domain;
using App.Game.Database;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.Repositories {



public class HumanTypeRepository : IHumanTypeRepository
{
	private readonly Dictionary<HumanTypeId, HumanType> _humanTypes = new();

	//----------------------------------------------------------------------------------------------


	public HumanTypeRepository(HumanTypes humanTypes_Asset)
	{
		foreach (HumanTypeId typeId in Enum.GetValues(typeof(HumanTypeId)))
			_humanTypes[typeId] = ConvertData(humanTypes_Asset.List.First(x => x.Id == typeId));
	}


	//----------------------------------------------------------------------------------------------
	// IHumanTypeRepository


	public HumanType Get(HumanTypeId typeId)
	{
		return _humanTypes[typeId];
	}


	//----------------------------------------------------------------------------------------------
	// private


	private static HumanType ConvertData(HumanTypes.HumanType_Data humanType_Data)
	{
		return new HumanType(
			humanType_Data.Id,
			humanType_Data.Gender,
			humanType_Data.EnergyRequiredDaily,
			humanType_Data.BaseSpeed,
			humanType_Data.GatheringSpeed);
	}
}



}
