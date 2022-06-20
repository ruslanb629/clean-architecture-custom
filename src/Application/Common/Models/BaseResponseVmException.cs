namespace Application.Common.Models;

public class BaseResponseVmException
{
    public string? Code { get; set; }
    public string? Message { get; set; }
    public string? ErrorStack { get; set; }
}
