using Cysharp.Threading.Tasks;



namespace App.Application.Contexts.RunningGame.Services {



/// <summary>
/// In-Game Mode of application when game world resources are loaded
/// </summary>
public interface IInGameMode
{
	UniTask Enter();

	UniTask Exit();
}



}
