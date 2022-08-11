using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application
{
    public class ServiceResult<T>
    {
        public ServiceResultCodeEnum Code { get; set; }

        public T Message { get; set; }

        public ServiceResult(ServiceResultCodeEnum code, T message)
        {
            this.Code = code;
            this.Message = message;
        }

        public ServiceResult()
        {

        }
    }

    public class ServiceResult : ServiceResult<string>
    {
        public ServiceResult(ServiceResultCodeEnum code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
        public ServiceResult(string message)
        {
            this.Code = ServiceResultCodeEnum.success;
            this.Message = message;
        }

        public ServiceResult()
        {
        }
    }

    public static class ServiceResultConst
    {
        public static string unlegalMediaType = "不支持上传的文件类型。";
    }

    public enum ServiceResultCodeEnum
    {
        success = 200,
        custom = 202,
        error = 203
    }
}
