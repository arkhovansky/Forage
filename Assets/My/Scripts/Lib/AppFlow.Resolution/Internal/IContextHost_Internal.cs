namespace Lib.AppFlow.Resolution.Internal {



public interface IContextHost_Internal : IContextHost
{
	void RegisterContext(IContextEntryPoint entryPoint);

	IContext CreateRootContext(IContextRequest request, IContextData contextData);
}



}
