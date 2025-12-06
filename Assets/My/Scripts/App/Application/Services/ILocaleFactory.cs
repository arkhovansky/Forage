using App.Game.Meta;



namespace App.Application.Services {



public interface ILocaleFactory
{
	ILocale Create(LocaleId localeId);
}



}
