using System;
using System.Collections.Generic;

using Lib.AppFlow.Resolution.Internal;



namespace Lib.AppFlow.Resolution.Impl {



public class ContextHost : IContextHost_Internal
{
	private readonly IContextDescriptorMatcher _descriptorMatcher;

	private readonly List<IContextEntryPoint> _contextEntryPoints = new();

	//----------------------------------------------------------------------------------------------


	public ContextHost(IContextDescriptorMatcher descriptorMatcher)
	{
		_descriptorMatcher = descriptorMatcher;
	}


	//----------------------------------------------------------------------------------------------
	// IContextHost_Internal


	public IHostServices HostServices { get; set; } = null!;


	public void RegisterContext(IContextEntryPoint entryPoint)
	{
		_contextEntryPoints.Add(entryPoint);
	}


	//----------------------------------------------------------------------------------------------
	// IContextHost


	public IContextCapability_Builder New_ContextCapability()
		=> new ContextCapability();

	public IContextRequest_Builder New_ContextRequest()
		=> new ContextRequest();


	public IContext CreateContext(IContextRequest request)
	{
		var entryPoint = Resolve_ContextRequest(request);
		return entryPoint.Create(request, HostServices);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private IContextEntryPoint Resolve_ContextRequest(IContextRequest request)
	{
		foreach (var entryPoint in _contextEntryPoints) {
			var capability = entryPoint.Get_CapabilityDescriptor(New_ContextCapability());
			if (_descriptorMatcher.Satisfies(capability, request))
				return entryPoint;
		}

		throw new Exception("Context not found");
	}
}



}
