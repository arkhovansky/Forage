using System.Collections.Generic;

using Lib.UICore.Gui;

using App.Application.Contexts.RunningGame._Infrastructure.Shared.Contracts.Database.Presentation;
using App.Game.Core.Query;



namespace App.Application.Contexts.RunningGame._Infrastructure.UI.ViewModels.Children {



public class BandMembersVM : IViewModel
{
	public List<BandMemberVM> BandMembers { get; }



	private readonly IBand_RO _band;

	private readonly ITime _time;

	private readonly IHumanTypePresentationRepository _humanTypePresentationRepository;

	private IReadOnlyList<IBandMember_RO>? _bandMembers;



	public BandMembersVM(IBand_RO band,
	                     ITime time,
	                     IHumanTypePresentationRepository humanTypePresentationRepository)
	{
		_band = band;
		_time = time;
		_humanTypePresentationRepository = humanTypePresentationRepository;

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
			BandMembers.Add(new BandMemberVM(member, _time, _humanTypePresentationRepository));
	}
}



}
