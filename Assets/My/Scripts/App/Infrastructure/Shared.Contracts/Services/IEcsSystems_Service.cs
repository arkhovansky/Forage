namespace App.Infrastructure.Shared.Contracts.Services {



public interface IEcsSystems_Service
{
	void SetEcsSystemsEnabled(bool enabled);

	bool GameSystems_Enabled { get; set; }
}



}
