using App.Game.Meta;



namespace App.Infrastructure.External.Data.Locale.Contracts {



public interface ILocaleAssetRepository
{
	Locale_Asset Get(LocaleId localeId);
}



}
