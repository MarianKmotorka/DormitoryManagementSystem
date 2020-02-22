using System.Collections.Generic;

namespace Library.Models
{
    public class PropertiesResultModel
    {
        public bool Success { get; set; }

        public bool Fail => !Success;

        public Dictionary<string, List<string>> Errors { get; set; }

        public static PropertiesResultModel Succesful => new PropertiesResultModel { Success = true };
    }
}
