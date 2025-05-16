namespace App.Game.Meta {



public class GameInstance : IGameInstance
{
	public IScene Scene { get; }


	public GameInstance()
	{
		Scene = new Scene();
	}
}



}
