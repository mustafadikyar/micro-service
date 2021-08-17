using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Micro.Shared.DTOs
{
    public class Response<T>
    {
        public T Data { get; set; }
        [JsonIgnore] public int Status { get; set; }
        [JsonIgnore] public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }


        public static Response<T> Success(int statusCode) => new Response<T>
        {
            Data = default,
            Status = statusCode,
            IsSuccess = true
        };

        public static Response<T> Success(T data, int statusCode) => new Response<T>
        {
            Data = data,
            Status = statusCode,
            IsSuccess = true
        };



        public static Response<T> Error(List<string> errors, int statusCode) => new Response<T>
        {
            Errors = errors,
            Status = statusCode,
            IsSuccess = false
        };

        public static Response<T> Error(string error, int statusCode) => new Response<T>
        {
            Errors = new List<string> { error },
            Status = statusCode,
            IsSuccess = false
        };
    }
}
