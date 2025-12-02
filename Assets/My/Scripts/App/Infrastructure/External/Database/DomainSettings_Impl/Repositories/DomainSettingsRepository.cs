using App.Game.ECS.GameTime.Settings;
using App.Infrastructure.EcsGateway.Database.DomainSettings;



namespace App.Infrastructure.External.Database.DomainSettings_Impl.Repositories {



public class DomainSettingsRepository : IDomainSettingsRepository
{
	public GameTime_Settings Get_GameTime_Settings()
		=> GameDatabase.Instance.DomainSettings.GameTime;
}



}
