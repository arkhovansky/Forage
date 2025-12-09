using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;



namespace Lib.AppFlow {



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
