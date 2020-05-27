namespace JotterAPI.Model.Reponses
{
	public class Response<T> where T : ResponseResult
	{
		public string Error { get; set; }

		public bool IsSuccessful { get; set; }

		public T ResponseResult { get; set;}

		public Response(string error)
		{
			Error = error;
			IsSuccessful = false;
		}

		public Response(T result)
		{
			IsSuccessful = true;
			ResponseResult = result;
		}

		public Response() { }
	}
}
