namespace Byndyusoft.ApiClient.Example.Tests;

using Api.Client;
using Xunit.Abstractions;

public class JsonFormatterTests : FormattersTests
{
    public JsonFormatterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        TestSubject = new PersonApi(
            Client,
            _clientSettings
        );
    }
}
