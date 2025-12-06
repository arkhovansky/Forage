using System.Linq;

using App.Game.Meta;
using App.Infrastructure.External.Data.Database.Domain.ScriptableObjects;
using App.Infrastructure.External.Data.Locale;
using App.Infrastructure.External.Data.Locale.Contracts;



namespace App.Infrastructure.External.Data.Database.Domain.Repositories {



public class LocaleAssetRepository : ILocaleAssetRepository
{
	private readonly Locales _localesAsset;



	public LocaleAssetRepository(Locales localesAsset)
	{
		_localesAsset = localesAsset;
	}


	public Locale_Asset Get(LocaleId localeId)
	{
		return _localesAsset.List.First(x => x.Id == localeId);
	}
}



}
