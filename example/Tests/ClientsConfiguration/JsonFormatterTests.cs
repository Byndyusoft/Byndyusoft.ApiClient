namespace Tests.ClientsConfiguration;

using Api;
using Api.Client;
using Infrastructure;
using Xunit.Abstractions;

public class JsonFormatterTests : FormattersTests
{
    public JsonFormatterTests(
        CustomWebApplicationFactory<Program> factory,
        ITestOutputHelper testOutputHelper
    ) : base(factory, testOutputHelper)
    {
        TestSubject = new PersonApi(Client, _clientSettings);
    }
}
