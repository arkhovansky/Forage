namespace App.Game.Core.Query {



public interface IRunningGameInstance_RO
{
	IWorld_RO World { get; }

	GamePhase GamePhase { get; }
}



}
