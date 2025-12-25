using Lib.AppFlow;
using Lib.AppFlow.Resolution;

using App.Application.Contexts.Application._Infrastructure.Services;
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

		var gameInstance_Factory = new GameInstance_Factory();

		return new ApplicationContext(
			applicationSettings,
			gameInstance_Factory);
	}
}



}
