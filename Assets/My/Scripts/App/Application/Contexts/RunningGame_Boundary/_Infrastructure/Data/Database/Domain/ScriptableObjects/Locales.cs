using System.Collections.Generic;

using UnityEngine;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Locale;
using App.Infrastructure.Data;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Domain.ScriptableObjects {



[CreateAssetMenu(fileName = nameof(Locales),
                 menuName = AssetMenuNames.Root_Database_Domain_+nameof(Locales),
                 order = 10)]
public class Locales : ScriptableObject
{
	public List<Locale_Asset> List = null!;
}



}
