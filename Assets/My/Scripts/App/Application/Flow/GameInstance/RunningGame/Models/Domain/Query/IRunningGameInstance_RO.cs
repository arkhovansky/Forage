using App.Game.Models;



namespace App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query {



public interface IRunningGameInstance_RO
{
	IWorld_RO World { get; }

	GamePhase GamePhase { get; }
}



}
