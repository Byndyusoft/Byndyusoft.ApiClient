namespace Byndyusoft.ApiClient.Example.Tests;

using Client;
using Xunit.Abstractions;

public class JsonFormatterTests : FormattersTests
{
    public JsonFormatterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        TestSubject = new PersonModelListClient(
            Client,
            _clientSettings
        );
    }
}
