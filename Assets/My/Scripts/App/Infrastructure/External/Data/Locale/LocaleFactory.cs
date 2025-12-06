using App.Application.Services;
using App.Game.Meta;
using App.Infrastructure.External.Data.Locale.Contracts;



namespace App.Infrastructure.External.Data.Locale {



public class LocaleFactory : ILocaleFactory
{
	private readonly ILocaleAssetRepository _assetRepo;



	public LocaleFactory(ILocaleAssetRepository assetRepo)
	{
		_assetRepo = assetRepo;
	}


	public ILocale Create(LocaleId localeId)
	{
		var asset = _assetRepo.Get(localeId);
		return new Locale(asset);
	}
}



}
