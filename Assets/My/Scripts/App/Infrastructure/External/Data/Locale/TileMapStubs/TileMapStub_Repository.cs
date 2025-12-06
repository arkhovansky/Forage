using System.Collections.Generic;

using App.Game.Meta;



namespace App.Infrastructure.External.Data.Locale.TileMapStubs {



public static class TileMapStub_Repository
{
	private static readonly Dictionary<LocaleId, int[]> TileMaps = new();


	static TileMapStub_Repository()
	{
		TileMaps.Add(new LocaleId("TropicalForest_1"), TileMapStub_TropicalForest_1.Tiles);
	}


	public static int[] Get(LocaleId localeId)
	{
		return TileMaps[localeId];
	}
}



}
