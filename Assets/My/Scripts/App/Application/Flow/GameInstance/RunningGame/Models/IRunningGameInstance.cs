using Lib.Grid;



namespace App.Application.Flow.GameInstance.RunningGame.Models {



/// <summary>
/// Running game world facade for use in Application layer
/// </summary>
public interface IRunningGameInstance
{
	void Start();

	void PlaceCamp(AxialPosition position);

	void RunYearPeriod();

	bool IsYearPeriodChanged();
}



}
