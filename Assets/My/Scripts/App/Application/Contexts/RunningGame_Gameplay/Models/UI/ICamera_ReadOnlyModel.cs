using UnityEngine;

using UniMob;



namespace App.Application.Contexts.RunningGame_Gameplay.Models.UI {



public interface ICamera_ReadOnlyModel
{
	Atom<Transform> CameraTransform_Atom { get; }
}



}
