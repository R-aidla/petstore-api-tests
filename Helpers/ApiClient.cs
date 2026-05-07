using RestSharp;

public static class ApiClient
{
    public static RestClient Client =>
        new RestClient("https://petstore.swagger.io/v2");
}