using System.Collections.Generic;

using UnityEngine;

using App.Infrastructure.External.Data.Locale;



namespace App.Infrastructure.External.Data.Database.Domain.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(Locales),
                 menuName = AssetMenuNames.Root_Database_Domain_+nameof(Locales),
                 order = 10)]
public class Locales : ScriptableObject
{
	public List<Locale_Asset> List = null!;
}



}
