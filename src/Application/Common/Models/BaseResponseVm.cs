namespace Application.Common.Models;

public class BaseResponseVm
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string? Transaction { get; set; }
    public string? ApplicationName { get; set; }
    //public int Status { get; set; }
    public BaseResponseVmException? Exception { get; set; }
}
