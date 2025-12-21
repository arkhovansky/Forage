using App.Application.Contexts.Application.Settings;
using App.Game.Meta;



namespace App.Application.Contexts.Application._Infrastructure.Settings {



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
