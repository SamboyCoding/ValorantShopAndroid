namespace ValorantShop.Models;

public class ValorantApiResponse<T>
{
    public T data { get; set; }
    public int status;
}