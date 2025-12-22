namespace Lib.AppFlow.Resolution {



/// <summary>
/// Context request descriptor, holding semantic info and argument values for context resolution
/// </summary>
public interface IContextRequest
{
	T GetSubject<T>();

	T GetArgument<T>();
}



}
