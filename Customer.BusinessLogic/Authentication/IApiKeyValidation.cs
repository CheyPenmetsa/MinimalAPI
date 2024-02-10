namespace Customer.BusinessLogic.Authentication
{
    public interface IApiKeyValidation
    {
        bool IsValidApiKey(string userApiKey);
    }
}
