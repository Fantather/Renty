using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Common
{
    public class OperationResult<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public Dictionary<string, string[]> ValidationErrors { get; set; } = new Dictionary<string, string[]>();

        public static OperationResult<T> Success(T data) => new()
        {
            Data = data,
            IsSuccess = true
        };

        public static OperationResult<T> Fail(params string[] errors) => new()
        {
            Errors = errors.ToList(),
            IsSuccess = false
        };
    }
}
