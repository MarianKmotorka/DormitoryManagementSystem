namespace Library.Models
{
    public class ResultModel
    {
        public bool Success { get; set; }

        public bool Fail => !Success;

        public string Error { get; set; }

        public static ResultModel Successful => new ResultModel { Success = true };
    }
}
