using System.Collections.Generic;



namespace Lib.AppFlow.Resolution.Internal {



public interface IContextRequest_Internal
{
	IReadOnlyDictionary<string, object> Fields { get; }

	IReadOnlyCollection<object> Arguments { get; }
}



}
