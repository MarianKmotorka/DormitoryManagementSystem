namespace WebApi.Common
{
    public class ErrorDetail
    {
        public string PropertyName { get; set; }

        public string Message { get; set; }

        public object CustomState { get; set; }
    }
}
