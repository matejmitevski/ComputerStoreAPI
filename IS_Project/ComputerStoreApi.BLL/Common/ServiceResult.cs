using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.BLL.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }

        public T? Data { get; set; }

        public string? Error { get; set; }

        public int StatusCode { get; set; }

        public static ServiceResult<T> Ok(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = 200
            };
        }

        public static ServiceResult<T> Created(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = 201
            };
        }

        public static ServiceResult<T> NoContent()
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                StatusCode = 204
            };
        }

        public static ServiceResult<T> BadRequest(string error)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Error = error,
                StatusCode = 400
            };
        }

        public static ServiceResult<T> NotFound(string error)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Error = error,
                StatusCode = 404
            };
        }

        public static ServiceResult<T> Conflict(string error)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Error = error,
                StatusCode = 409
            };
        }
    }
}