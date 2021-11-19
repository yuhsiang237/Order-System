using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.ViewModels
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public object Error { get; set; }
        public int DataCount { get; set; }


        public static ResponseModel Success(string message = null, object data = null, int dataCount = 0)
        {
            return new ResponseModel()
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                Error = null
            };
        }
        public static ResponseModel Fail(string message = null, object data = null, int dataCount = 0,object error = null)
        {
            return new ResponseModel()
            {
                IsSuccess = false,
                Message = message,
                Data = data,
                Error = error
            };
        }
    }

}
