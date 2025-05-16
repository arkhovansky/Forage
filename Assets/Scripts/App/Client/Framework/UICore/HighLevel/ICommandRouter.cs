namespace App.Client.Framework.UICore.HighLevel {



public interface ICommandRouter
{
	void AddController(IController controller);

	void RemoveController(IController controller);

	void EmitCommand(ICommand command, IController emitter);

	void Update();
}



}
