using Lib.AppFlow;
using Lib.AppFlow.Resolution;
using Lib.UICore.Gui;
using Lib.UICore.Mvvm;

using App.Infrastructure.Shared.Contracts.Services;



namespace App.Infrastructure {



public class HostServices : IHostServices
{
	public readonly IContextHost ContextHost;

	public readonly IMessageDispatcher MessageDispatcher;

	public readonly IGui Gui;
	public readonly IVvmBinder VvmBinder;

	public readonly IEcsSystems_Service EcsSystems_Service;



	public HostServices(IContextHost contextHost,
	                    IMessageDispatcher messageDispatcher,
	                    IGui gui,
	                    IVvmBinder vvmBinder,
	                    IEcsSystems_Service ecsSystems_Service)
	{
		ContextHost = contextHost;
		MessageDispatcher = messageDispatcher;
		Gui = gui;
		VvmBinder = vvmBinder;
		EcsSystems_Service = ecsSystems_Service;
	}
}



}
