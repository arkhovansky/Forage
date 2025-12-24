using UnityEngine;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.ScriptableObjects;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database {



public class GameDatabase : MonoBehaviour
{
	public static GameDatabase Instance { get; private set; } = null!;



	public DomainDatabase Domain = null!;

	public DomainSettings.ScriptableObjects.DomainSettings DomainSettings = null!;

	public PresentationDatabase Presentation = null!;



	private void Awake()
	{
		Instance = this;

		enabled = false;
	}
}



}
