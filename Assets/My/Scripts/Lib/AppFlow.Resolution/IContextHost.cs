namespace Lib.AppFlow.Resolution {



public interface IContextHost
{
	IContextCapability_Builder New_ContextCapability();

	IContextRequest_Builder New_ContextRequest();


	IContext CreateContext(IContextRequest request, IContext parent);
}



}
