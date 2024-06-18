namespace Employee_Api.Common
{
    public class ErrorCodes
    {
        private static readonly Dictionary<int, (int StatusCode, string ErrorMessage)> ErrorCodeMapping = new()
    {
        { 20001, (405, "First name must be between 2 and 50 characters long") },
        { 20002, (406, "Last name must be between 2 and 50 characters long") },
        { 20003, (407, "Profession must be between 2 and 100 characters long") },
        { 20004, (408, "Age exceeds maximum length of 3 characters") },
        { 20005, (404, "Person ID does not exist") },
        { 20006, (409, "Person with the given ID already exists") }

    };

        public static (int StatusCode, string ErrorMessage) GetErrorInfo(int errorCode)
        {
            if (ErrorCodeMapping.TryGetValue(errorCode, out var errorInfo))
            {
                return errorInfo;
            }
            return (500, "An error occurred while processing the request.");
        }
    }
}
