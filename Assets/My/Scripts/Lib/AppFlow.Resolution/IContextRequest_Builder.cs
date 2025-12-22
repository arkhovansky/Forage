namespace Lib.AppFlow.Resolution {



public interface IContextRequest_Builder
{
	IContextRequest_Builder Subject(string value);

	IContextRequest_Builder Subject<T>(T value)
		where T : class;

	IContextRequest_Builder Field(string name, object value);

	IContextRequest_Builder Argument(object value);


	IContextRequest Build();
}



}
