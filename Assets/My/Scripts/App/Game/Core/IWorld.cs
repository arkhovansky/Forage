using App.Game.Core.Query;



namespace App.Game.Core {



public interface IWorld : IWorld_RO
{
	new IBand Band { get; }
}



}
