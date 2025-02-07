namespace Byndyusoft.ApiClient.Example.Tests;

using System.Net.Http.MessagePack;
using System.Net.Http.MessagePack.Formatting;
using Api.Client;
using Xunit.Abstractions;

public class MessagePackFormatterTests : FormattersTests
{
    public MessagePackFormatterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        TestSubject = new PersonApi(
            Client,
            _clientSettings,
            new MessagePackMediaTypeFormatter(MessagePackDefaults.SerializerOptions)
        );
    }
}
