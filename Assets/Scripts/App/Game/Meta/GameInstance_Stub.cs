namespace App.Game.Meta {



public class GameInstance_Stub : IGameInstance
{
	public IScene Scene { get; }


	public GameInstance_Stub()
	{
		Scene = new Scene_Stub();
	}
}



}
