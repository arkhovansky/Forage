namespace App.Game.Core.Query {



public interface IWorld_RO
{
	ITime Time { get; }

	IMap Map { get; }

	IPlantResources PlantResources { get; }

	IBand_RO Band { get; }
}



}
