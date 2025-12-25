namespace App.Game.Meta.Impl {



public class GameInstance : IGameInstance
{
	public IGameInstance_Setup Setup { get; }



	public GameInstance(IGameInstance_Setup setup)
	{
		Setup = setup;
	}
}



}
