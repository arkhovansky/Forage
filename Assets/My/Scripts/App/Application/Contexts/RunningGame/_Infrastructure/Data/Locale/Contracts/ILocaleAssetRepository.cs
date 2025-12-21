using App.Game.Meta;



namespace App.Application.Contexts.RunningGame._Infrastructure.Data.Locale.Contracts {



public interface ILocaleAssetRepository
{
	Locale_Asset Get(LocaleId localeId);
}



}
