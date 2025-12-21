using App.Application.Contexts.RunningGame._Infrastructure.Data.Locale.Contracts;
using App.Application.Contexts.RunningGame.Services;
using App.Game.Meta;



namespace App.Application.Contexts.RunningGame._Infrastructure.Data.Locale {



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
