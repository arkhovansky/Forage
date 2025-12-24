namespace Lib.AppFlow {



/// <summary>
/// Data attached to Context
/// </summary>
/// <remarks>
/// Context Data forms a hierarchy parallel to Contexts. Each Context can write its Data and read all Data up
/// the hierarchy.
/// The root of the hierarchy is not attached to any Context and holds host data.
/// Context data is not used directly by a Context object; instead, the required pieces of it are injected into
/// the Context by its Entry Point.
/// </remarks>
public interface IContextData
{
	/// <summary>
	/// Add object to this Context's Data
	/// </summary>
	/// <param name="obj"></param>
	/// <typeparam name="T"></typeparam>
	void Add<T>(T obj)
		where T : notnull;

	/// <summary>
	/// Get object from Data of this Context or any level up the hierarchy
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	T Get<T>()
		where T : notnull;
}



}
