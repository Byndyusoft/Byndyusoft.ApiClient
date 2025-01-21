namespace Byndyusoft.ApiClient.Functional;

using System.Net.Http.Formatting;
using Interfaces;

public class TestFormatterProvider: IFormatterProvider
{
    public TestFormatterProvider(MediaTypeFormatter formatter)
    {
        Formatter = formatter;
    }

    public MediaTypeFormatter Formatter { get; }
}