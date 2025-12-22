namespace Lib.AppFlow.Resolution.Internal {



public interface IContextDescriptorMatcher
{
	bool Satisfies(IContextCapability capability, IContextRequest request);
}



}
