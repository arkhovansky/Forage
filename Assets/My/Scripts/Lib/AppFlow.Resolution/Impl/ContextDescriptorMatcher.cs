using System;
using System.Collections.Generic;
using System.Linq;

using Lib.AppFlow.Resolution.Internal;



namespace Lib.AppFlow.Resolution.Impl {



public class ContextDescriptorMatcher : IContextDescriptorMatcher
{
	//----------------------------------------------------------------------------------------------
	// IContextDescriptorMatcher


	public bool Satisfies(IContextCapability capability, IContextRequest request)
	{
		return Satisfies((IContextCapability_Internal) capability, (IContextRequest_Internal) request);
	}


	//----------------------------------------------------------------------------------------------
	// private


	private bool Satisfies(IContextCapability_Internal capability, IContextRequest_Internal request)
	{
		var requestFields = new Dictionary<string, object>(request.Fields);

		foreach (var (name, capabilityValue) in capability.Fields) {
			if (!requestFields.ContainsKey(name))
				return false;

			requestFields.Remove(name, out var requestValue);

			switch (capabilityValue) {
				case string capabilityString:
					if (requestValue as string != capabilityString)
						return false;
					break;
				case Type capabilityType:
					if (!capabilityType.IsAssignableFrom(requestValue.GetType()))
						return false;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		if (requestFields.Count != 0)
			return false;


		var arguments = request.Arguments.ToList();

		foreach (var parameterType in capability.Parameters) {
			var i = arguments.FindIndex(x => parameterType.IsAssignableFrom(x.GetType()));

			if (i == -1)
				return false;

			arguments.RemoveAt(i);
		}

		if (arguments.Count != 0)
			return false;


		return true;
	}

}



}
