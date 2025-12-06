using UnityEngine;

using App.Infrastructure.External.Data.Database.Domain.ScriptableObjects;
using App.Infrastructure.External.Data.Database.Presentation.ScriptableObjects;



namespace App.Infrastructure.External.Data.Database {



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
