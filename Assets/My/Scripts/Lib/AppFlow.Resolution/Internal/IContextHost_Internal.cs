namespace Lib.AppFlow.Resolution.Internal {



public interface IContextHost_Internal : IContextHost
{
	IHostServices HostServices { get; set; }

	void RegisterContext(IContextEntryPoint entryPoint);
}



}
