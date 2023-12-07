using Application.ViewModels.ProjectViewModels;

namespace Application.Commons.ServiceResponse
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();

        public static ServiceResponse<T> SuccessResult(T data, string message = "")
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ServiceResponse<T> ErrorResult(string errorMessage)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = "Error",
                Data = default(T), // Có thể sẽ không có dữ liệu ở trường hợp này
                ErrorMessages = new List<string> { errorMessage }
            };
        }

        public static ServiceResponse<T> ErrorResult(List<string> errorMessages)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = "Error",
                Data = default(T),
                ErrorMessages = errorMessages
            };
        }

        public static ServiceResponse<T> UnauthorizedResult()
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = "Unauthorized",
                Data = default(T),
                ErrorMessages = new List<string> { "Unauthorized" }
            };
        }
        public static ServiceResponse<T> NotFoundResult(string entityName)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = $"{entityName} not found",
                Data = default(T),
                ErrorMessages = new List<string> { $"{entityName} not found" }
            };
        }

    }
}