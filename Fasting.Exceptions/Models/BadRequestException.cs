namespace Fasting.Exceptions.Models
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string type, string message, dynamic logObject = null) : base(message)
        {
            Type = type;
            LogObject = logObject;
        }

        public string Type { get; }
        public dynamic LogObject { get; }
    }
}
