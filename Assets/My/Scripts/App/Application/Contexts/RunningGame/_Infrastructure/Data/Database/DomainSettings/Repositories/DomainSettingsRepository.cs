using App.Application.Contexts.RunningGame._Infrastructure.EcsGateway.Contracts.Database.DomainSettings;
using App.Game.ECS.GameTime.Settings;



namespace App.Application.Contexts.RunningGame._Infrastructure.Data.Database.DomainSettings.Repositories {



public class DomainSettingsRepository : IDomainSettingsRepository
{
	private readonly ScriptableObjects.DomainSettings _domainSettings_Asset;



	public DomainSettingsRepository(ScriptableObjects.DomainSettings domainSettings_Asset)
	{
		_domainSettings_Asset = domainSettings_Asset;
	}


	public GameTime_Settings Get_GameTime_Settings()
		=> _domainSettings_Asset.GameTime;
}



}
