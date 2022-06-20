namespace Application.Common.Exceptions
{
    public static class ExceptionCode
    {
        public static string BadRequest { get; } = "4000";
        public static string ValidationError { get; } = "4001";
        public static string InternalServerError { get; } = "5001";
    }
}
