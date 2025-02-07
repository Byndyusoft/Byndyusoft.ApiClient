namespace Tests.ClientsConfiguration;

using System.Net.Http.ProtoBuf;
using System.Net.Http.ProtoBuf.Formatting;
using Api;
using Api.Client;
using Tests;
using Infrastructure;
using Xunit.Abstractions;

public class ProtoBufFormatterTests : FormattersTests
{
    public ProtoBufFormatterTests(
        CustomWebApplicationFactory<Program> factory,
        ITestOutputHelper testOutputHelper
    ) : base(factory, testOutputHelper)
    {
        TestSubject = new PersonApi(
            Client,
            _clientSettings,
            new ProtoBufMediaTypeFormatter(ProtoBufDefaults.TypeModel)
        );
    }
}
