namespace Byndyusoft.ApiClient.Interfaces
{
    using System.Net.Http.Formatting;

    public interface IFormatterProvider
    {
        MediaTypeFormatter Formatter { get; }
    }
}