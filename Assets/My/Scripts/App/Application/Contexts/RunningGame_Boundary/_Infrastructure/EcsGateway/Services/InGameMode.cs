using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;

using App.Application.Contexts.RunningGame_Boundary.Services;
using App.Game.ECS.Prefabs.Components;
using App.Infrastructure.Shared.Contracts.Services;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.EcsGateway.Services {



public class InGameMode : IInGameMode
{
	private const string GameSceneName = "Game";


	private readonly IEcsSystems_Service _ecsSystems_Service;

	//----------------------------------------------------------------------------------------------


	public InGameMode(IEcsSystems_Service ecsSystems_Service)
	{
		_ecsSystems_Service = ecsSystems_Service;
	}


	//----------------------------------------------------------------------------------------------
	// IInGameMode implementation


	public async UniTask Enter()
	{
		_ecsSystems_Service.GameSystems_Enabled = false;
		_ecsSystems_Service.SetEcsSystemsEnabled(true);
		await LoadGameScene_Async();
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameSceneName));
	}


	public UniTask Exit()
	{
		throw new System.NotImplementedException();
	}


	//----------------------------------------------------------------------------------------------
	// private


	private async UniTask LoadGameScene_Async()
	{
		// Might be already loaded in editor
		if (SceneManager.GetSceneByName(GameSceneName).IsValid())
			return;

		await SceneManager.LoadSceneAsync(GameSceneName, LoadSceneMode.Additive);
		await WaitForSubsceneLoading();
	}


	private async UniTask WaitForSubsceneLoading()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<PrefabReferences>());

		await UniTask.WaitWhile(() => query.IsEmpty);
	}
}



}
