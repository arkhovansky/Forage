using UnityEngine;

using App.Infrastructure.External.Database.Database_Impl.ScriptableObjects;
using App.Infrastructure.External.Database.DomainSettings_Impl.ScriptableObjects;
using App.Infrastructure.External.Database.PresentationDatabase_Impl.ScriptableObjects;



namespace App.Infrastructure.External.Database {



public class GameDatabase : MonoBehaviour
{
	public static GameDatabase Instance { get; private set; } = null!;



	public DomainDatabase Domain = null!;

	public DomainSettings DomainSettings = null!;

	public PresentationDatabase Presentation = null!;



	private void Awake()
	{
		Instance = this;

		enabled = false;
	}
}



}
