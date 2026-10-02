using Microsoft.AspNetCore.Http.HttpResults;

namespace ASP_P42.Models.Rest
{

    // статус ответа, сообщает клиенту, успешно ли выполнен запрос и какой статус получен
    public class RestStatus
    {
        public bool IsOk { get; set; }
        public int Code { get; set; }
        public String Message { get; set; } = null!;

        public static readonly RestStatus Ok = new() { IsOk = true, Code = 200, Message = "OK" };
        public static readonly RestStatus BadRequest = new() { IsOk = false, Code = 400, Message = "Bad Request" };
        public static readonly RestStatus NotFound = new () { IsOk = false,  Code = 404, Message = "Not Found" };
    }
}
