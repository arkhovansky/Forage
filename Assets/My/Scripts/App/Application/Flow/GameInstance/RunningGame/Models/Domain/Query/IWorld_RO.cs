namespace App.Application.Flow.GameInstance.RunningGame.Models.Domain.Query {



public interface IWorld_RO
{
	ITime Time { get; }

	IMap Map { get; }

	IBand_RO Band { get; }
}



}
