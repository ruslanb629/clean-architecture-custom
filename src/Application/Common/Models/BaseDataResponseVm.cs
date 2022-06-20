namespace Application.Common.Models;

public class BaseDataResponseVm<TResponse>: BaseResponseVm
{
    public TResponse? Data { get; set; }
}