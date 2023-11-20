namespace ProductsAPI.Services;

public class FromService : IFromService
{
    public string HelloWorld(string name)
    {
        return $"Hello, {name} \n\n{DateTime.Now}";
    }
}
