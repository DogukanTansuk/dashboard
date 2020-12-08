using System;

namespace DashboardApi.Services
{
    public class ServiceException: Exception
    {
        public int StatusCode { get; }
        public string ErrorMessage { get;  }

        public ServiceException(int statusCode, string errorMessage) : base(errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}