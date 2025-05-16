using System;
using System.Collections.Generic;

namespace App.Client.Framework.UICore.HighLevel {



public interface IController
{
	IController? Parent { get; set; }

	IReadOnlyDictionary<Type, Delegate> CommandHandlers { get; }


	void Start();

	void Update();

	void UpdateViewModels();

	void Destroy();


	// Delegate? TryGetCommandTypeHandler(Type commandType);


	// void AttachChildToGui(IController child);
	//
	// void DetachFromGui();
	//
	// void SetVisualNode(IVisualNode visualNode);
}



}
