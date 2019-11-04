namespace Pl.WebFramework.Models
{
    public class ApiResponseData
    {
        /// <summary>
        /// Response code. in base
        /// -1 is error
        /// 0 is not process, data not validate
        /// 1 to success full
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Response message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// json data of object return to client
        /// </summary>
        public string Data { get; set; }
    }
}
