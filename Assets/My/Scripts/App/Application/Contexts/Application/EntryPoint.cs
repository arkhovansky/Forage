using Lib.AppFlow;
using Lib.AppFlow.Resolution;

using App.Application.Contexts.Application.Settings;



namespace App.Application.Contexts.Application {



public class EntryPoint : IContextEntryPoint
{
	public static readonly EntryPoint Instance;

	static EntryPoint()
	{
		Instance = new EntryPoint();
	}


	//----------------------------------------------------------------------------------------------
	// IContextEntryPoint


	public IContextCapability Get_CapabilityDescriptor(IContextCapability_Builder builder)
	{
		return builder
			.Subject("Application")
			.Parameter<IApplicationSettings>()
			.Build();
	}


	public IContext Create(IContextRequest request, IContextData contextData)
	{
		var applicationSettings = request.GetArgument<IApplicationSettings>();

		return new ApplicationContext(
			applicationSettings,
			contextData.Get<IMessageDispatcher>(),
			contextData.Get<IContextHost>());
	}
}



}
