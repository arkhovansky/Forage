using Unity.Properties;

using App.Application.Database.Presentation;
using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;



namespace App.Application.Flow.GameInstance.RunningGame.ViewModels {



public class BandMemberVM
{
	[CreateProperty]
	public string HumanType { get; set; }

	[CreateProperty]
	public string Goal { get; set; } = string.Empty;

	[CreateProperty]
	public string Activity { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageForagingHours { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageGatheringHours { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageMovingHours { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageLeisureHours { get; set; } = string.Empty;

	[CreateProperty]
	public string AverageSleepingHours { get; set; } = string.Empty;



	private readonly IBandMember_RO _bandMember;

	private readonly ITime _time;



	public BandMemberVM(IBandMember_RO bandMember,
	                    ITime time,
	                    IHumanTypePresentationRepository humanTypePresentationRepository)
	{
		_bandMember = bandMember;
		_time = time;

		HumanType = humanTypePresentationRepository.GetName(bandMember.TypeId);
	}


	public void Update()
	{
		Goal = _bandMember.Get_Goal().ToString();

		Activity = _bandMember.Get_Activity().ToString();

		if (_time.Get_DayChanged()) {
			var statistics = _bandMember.Get_YearPeriodStatistics();

			const string format = "F1";

			AverageForagingHours = statistics.AverageForagingHours.ToString(format);
			AverageGatheringHours = statistics.AverageGatheringHours.ToString(format);
			AverageMovingHours = statistics.AverageMovingHours.ToString(format);
			AverageLeisureHours = statistics.AverageLeisureHours.ToString(format);
			AverageSleepingHours = statistics.AverageSleepingHours.ToString(format);
		}
	}
}



}
