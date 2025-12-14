using System;
using System.Collections.Generic;



namespace Lib.AppFlow {



/// <summary>
/// Controller handles business commands in a Context
/// </summary>
public interface IController
{
	IReadOnlyDictionary<Type, Delegate> CommandHandlers { get; }


	void Start();

	void Update();
}



}
