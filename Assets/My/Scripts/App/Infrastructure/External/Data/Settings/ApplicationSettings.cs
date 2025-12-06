using App.Application.Settings;
using App.Game.Meta;



namespace App.Infrastructure.External.Data.Settings {



public class ApplicationSettings : IApplicationSettings
{
	private readonly ApplicationSettings_Asset _asset;



	public ApplicationSettings(ApplicationSettings_Asset asset)
	{
		_asset = asset;
	}


	public LocaleId DefaultLocale
		=> new(_asset.DefaultLocale);
}



}
