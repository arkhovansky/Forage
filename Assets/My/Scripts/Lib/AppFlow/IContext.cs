using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;



namespace Lib.AppFlow {



/// <summary>
/// Application Flow Context, a node in Context hierarchy
/// </summary>
/// <remarks>
/// A Context usually has a local MV* setup.
/// A Context listens for navigation commands from its level and downward ones, and manages child Contexts.
/// </remarks>
public interface IContext
{
	IContext? Parent { get; set; }

	IReadOnlyDictionary<Type, Delegate> CommandHandlers { get; }

	IController? Controller { get; }


	UniTask Start();

	void Update();

	void UpdateViewModels();

	void Destroy();
}



}
