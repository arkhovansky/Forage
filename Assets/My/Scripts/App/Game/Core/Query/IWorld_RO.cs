namespace App.Game.Core.Query {



public interface IWorld_RO
{
	ITime Time { get; }

	IMap Map { get; }

	IBand_RO Band { get; }
}



}
