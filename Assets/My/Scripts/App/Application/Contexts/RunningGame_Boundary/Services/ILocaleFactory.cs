using App.Game.Meta;



namespace App.Application.Contexts.RunningGame_Boundary.Services {



public interface ILocaleFactory
{
	ILocale Create(LocaleId localeId);
}



}
