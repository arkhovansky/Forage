namespace Lib.AppFlow {



/// <summary>
/// View receives DomainEvents and PresentationEvents in a Context, and emits Commands
/// </summary>
/// <remarks>This is a conceptual View, which includes, e.g., an MVVM's ViewModel.</remarks>
public interface IView : ILoopComponent {}



}
