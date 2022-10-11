﻿namespace Xy.Project.Core.Models
{
    [Serializable]
    public sealed class AppResult : IResult<object>
    {
        public AppResult()
        {

        }

        //public AppResult(HttpCode status) : this(default, status, default)
        //{

        //}

        //public AppResult(object? data, HttpCode status) : this(data, status, default)
        //{

        //}

        //public AppResult(HttpCode status, string? message) : this(default, status, message)
        //{

        //}
        //public AppResult(object? data, HttpCode status, string? message)
        //{
        //    Data = data;
        //    Status = status;
        //    Message = message;
        //}
        public string? Msg { get; set; }
        public bool Succeeded => Code == (int)HttpCode.成功;
        public int Code { get; set; }
        public object? Data { get; set; }

        public static AppResult Problem(HttpCode status, string? message = default, object? data = default)
        {
            return new AppResult() { Code = (int)status, Msg = message, Data = data };
        }

        public static async Task<AppResult> ProblemAsync(HttpCode status, string? message = default, object? data = default)
        {
            return await Task.FromResult(Problem(status, message, data));
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <returns></returns>
        public static async Task<AppResult> Error(string msg = "操作失败")
        {
            return await Task.FromResult(Problem(HttpCode.失败, msg));
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static async Task<AppResult> Success()
        {
            return await Task.FromResult(Problem(HttpCode.成功, "操作失败"));
        }
    }

    [Serializable]
    public class AppResult<T> : IResult<T>
    {
        public AppResult()
        {

        }
        public string? Msg { get; set; }
        public bool Succeeded => Code == (int)HttpCode.成功;
        public int Code { get; set; }

        public T? Data { get; set; }


        public static AppResult<T> Problem(HttpCode status, string? message = default, T? data = default)
        {
            return new AppResult<T>() { Code = (int)status, Msg = message, Data = data };
        }


    }
}