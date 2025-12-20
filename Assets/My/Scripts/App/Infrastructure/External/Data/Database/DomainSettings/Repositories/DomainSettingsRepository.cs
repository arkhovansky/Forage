using App.Game.ECS.GameTime.Settings;
using App.Infrastructure.EcsGateway.Contracts.Database.DomainSettings;



namespace App.Infrastructure.External.Data.Database.DomainSettings.Repositories {



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
