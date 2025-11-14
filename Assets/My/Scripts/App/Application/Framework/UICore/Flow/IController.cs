using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;



namespace App.Application.Framework.UICore.Flow {



public interface IController
{
	IController? Parent { get; set; }

	IReadOnlyDictionary<Type, Delegate> CommandHandlers { get; }


	UniTask Start();

	void Update();

	void UpdateViewModels();

	void Destroy();
}



}
