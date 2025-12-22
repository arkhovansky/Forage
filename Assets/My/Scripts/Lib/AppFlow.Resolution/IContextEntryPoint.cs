namespace Lib.AppFlow.Resolution {



public interface IContextEntryPoint
{
	IContextCapability Get_CapabilityDescriptor(IContextCapability_Builder builder);

	IContext Create(IContextRequest request, IHostServices hostServices);
}



}
