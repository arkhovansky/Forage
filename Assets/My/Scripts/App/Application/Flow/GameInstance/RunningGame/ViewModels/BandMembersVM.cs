using System.Collections.Generic;

using App.Application.Framework.UICore.Mvvm;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;
using App.Game.Database;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



public class BandMembersVM : IViewModel
{
	public List<BandMemberVM> BandMembers { get; }



	private readonly IBand_RO _band;

	private readonly ITime _time;

	private readonly IHumanTypeRepository _humanTypeRepository;

	private IReadOnlyList<IBandMember_RO>? _bandMembers;



	public BandMembersVM(IBand_RO band,
	                     ITime time,
	                     IHumanTypeRepository humanTypeRepository)
	{
		_band = band;
		_time = time;
		_humanTypeRepository = humanTypeRepository;

		BandMembers = new List<BandMemberVM>();
	}


	public void Update()
	{
		if (BandMembers.Count == 0)
			CreateMembers();

		foreach (var memberVM in BandMembers)
			memberVM.Update();
	}


	private void CreateMembers()
	{
		_bandMembers = _band.Get_Members();

		foreach (var member in _bandMembers)
			BandMembers.Add(new BandMemberVM(member, _time, _humanTypeRepository));
	}
}



}
