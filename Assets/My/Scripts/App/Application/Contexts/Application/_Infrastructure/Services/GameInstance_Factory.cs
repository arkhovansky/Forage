using App.Application.Contexts.Application.Services;
using App.Game.Meta;
using App.Game.Meta.Impl;



namespace App.Application.Contexts.Application._Infrastructure.Services {



public class GameInstance_Factory : IGameInstance_Factory
{
	public IGameInstance Create()
	{
		var setup = new GameInstance_Setup();
		return new GameInstance(setup);
	}
}



}
