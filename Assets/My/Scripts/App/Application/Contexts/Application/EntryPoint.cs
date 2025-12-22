using Lib.AppFlow;
using Lib.AppFlow.Resolution;

using App.Application.Contexts.Application.Settings;
using App.Infrastructure;



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


	public IContext Create(IContextRequest request, IHostServices hostServices)
	{
		var applicationSettings = request.GetArgument<IApplicationSettings>();
		var hostServices_ = (HostServices) hostServices;

		return new ApplicationContext(applicationSettings,
		                              hostServices_.MessageDispatcher,
		                              hostServices_.ContextHost);
	}
}



}
