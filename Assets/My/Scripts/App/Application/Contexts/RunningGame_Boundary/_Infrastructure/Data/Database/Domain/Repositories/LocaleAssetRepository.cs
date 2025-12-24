using System.Linq;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.ScriptableObjects;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Locale;
using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Locale.Contracts;
using App.Game.Meta;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.Repositories {



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
