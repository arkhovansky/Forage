using App.Game.Meta;



namespace App.Application.Contexts.RunningGame.Services {



public interface ILocaleFactory
{
	ILocale Create(LocaleId localeId);
}



}
