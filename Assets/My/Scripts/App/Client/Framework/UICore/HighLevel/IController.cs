using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;



namespace App.Client.Framework.UICore.HighLevel {



public interface IController
{
	IController? Parent { get; set; }

	IReadOnlyDictionary<Type, Delegate> CommandHandlers { get; }


	UniTask Start();

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
