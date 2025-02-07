namespace Tests.ClientsConfiguration;

using System.Net.Http.MessagePack;
using System.Net.Http.MessagePack.Formatting;
using Api;
using Api.Client;
using Tests;
using Infrastructure;
using Xunit.Abstractions;

public class MessagePackFormatterTests : FormattersTests
{
    public MessagePackFormatterTests(
        CustomWebApplicationFactory<Program> factory,
        ITestOutputHelper testOutputHelper
    ) : base(factory, testOutputHelper)
    {
        TestSubject = new PersonApi(
            Client,
            _clientSettings,
            new MessagePackMediaTypeFormatter(MessagePackDefaults.SerializerOptions)
        );
    }
}
