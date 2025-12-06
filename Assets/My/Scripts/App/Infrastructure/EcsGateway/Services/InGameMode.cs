using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;

using App.Application.Services;
using App.Game.ECS.Prefabs.Components;



namespace App.Infrastructure.EcsGateway.Services {



public class InGameMode : IInGameMode
{
	private const string GameSceneName = "Game";



	public async UniTask Enter()
	{
		EcsService.GameSystems_Enabled = false;
		EcsService.SetEcsSystemsEnabled(true);
		await LoadGameScene_Async();
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
		var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<PrefabReferences>());

		await UniTask.WaitWhile(() => query.IsEmpty);
	}
}



}
