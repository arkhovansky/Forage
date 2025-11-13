using Lib.Grid;

using App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Domain {



/// <summary>
/// Running game world facade for use in Application layer
/// </summary>
public interface IRunningGameInstance : IRunningGameInstance_RO
{
	void Start();

	void PlaceCamp(AxialPosition position);

	void RunYearPeriod();

	bool IsYearPeriodChanged();
}



}
