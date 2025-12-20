namespace App.Infrastructure.Common.Contracts.Services {



public interface IEcsSystems_Service
{
	void SetEcsSystemsEnabled(bool enabled);

	bool GameSystems_Enabled { get; set; }
}



}
