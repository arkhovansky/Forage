using App.Game.Core.Query;



namespace App.Game.Core {



/// <summary>
/// Running game world facade for use in Application layer
/// </summary>
public interface IRunningGameInstance : IRunningGameInstance_RO
{
	new IWorld World { get; }

	void RunYearPeriod();
}



}
