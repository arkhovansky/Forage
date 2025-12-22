namespace Lib.AppFlow.Resolution {



public interface IContextCapability_Builder
{
	IContextCapability_Builder Subject(string value);

	IContextCapability_Builder Subject<T>()
		where T : class;

	IContextCapability_Builder Field(string name, object value);

	IContextCapability_Builder Parameter<T>();


	IContextCapability Build();
}



}
