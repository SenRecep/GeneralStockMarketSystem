using System.Collections.Generic;

namespace GeneralStockMarket.CoreLib.Response
{
    public class Error
    {
        public IEnumerable<string> Errors { get;  set; }
        public bool IsShow { get;  set; }
        public string Path { get;  set; }

        public static Error SendError(string path = "", bool isShow = true, params string[] errors) => new Error()
        {
            Errors = errors,
            IsShow = isShow,
            Path = path
        };
        public static string GetError(Error error, string separator = "\n")
        {
            return string.Join(separator, error.Errors);
        }

    }
}
