namespace App.Game.Meta.Impl {



public class GameInstance : IGameInstance
{
	public LocaleId LocaleId { get; }


	public GameInstance(LocaleId localeId)
	{
		LocaleId = localeId;
	}
}



}
