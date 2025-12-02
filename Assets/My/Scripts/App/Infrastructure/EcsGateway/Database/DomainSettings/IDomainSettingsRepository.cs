using App.Game.ECS.GameTime.Settings;



namespace App.Infrastructure.EcsGateway.Database.DomainSettings {



public interface IDomainSettingsRepository
{
	GameTime_Settings Get_GameTime_Settings();
}



}
