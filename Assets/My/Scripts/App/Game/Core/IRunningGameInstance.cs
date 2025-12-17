using Lib.Grid;

using App.Game.Core.Query;



namespace App.Game.Core {



/// <summary>
/// Running game world facade for use in Application layer
/// </summary>
public interface IRunningGameInstance : IRunningGameInstance_RO
{
	void PlaceCamp(AxialPosition position);

	void RunYearPeriod();

	bool IsYearPeriodChanged();
}



}
