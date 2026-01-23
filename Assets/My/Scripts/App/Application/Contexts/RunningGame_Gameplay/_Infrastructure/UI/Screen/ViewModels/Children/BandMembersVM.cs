using System.Collections.Generic;

using Lib.UICore.Gui;

using App.Game.Core.Query;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Gameplay._Infrastructure.UI.Screen.ViewModels.Children {



public class BandMembersVM : IViewModel
{
	public List<BandMemberVM> BandMembers { get; }



	private readonly IBand_RO _band;

	private readonly ITime _time;

	private readonly IHumanType_TextualPresentation_Repository _humanTypePresentationRepository;

	private IReadOnlyList<IBandMember_RO>? _bandMembers;



	public BandMembersVM(IBand_RO band,
	                     ITime time,
	                     IHumanType_TextualPresentation_Repository humanTypePresentationRepository)
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
