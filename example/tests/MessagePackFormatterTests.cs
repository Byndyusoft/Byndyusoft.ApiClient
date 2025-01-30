namespace Byndyusoft.ApiClient.Example.Tests;

using System.Net.Http.MessagePack;
using System.Net.Http.MessagePack.Formatting;
using Client;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

public class MessagePackFormatterTests : FormattersTests
{
    public MessagePackFormatterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        TestSubject = new PersonModelListClient(
            Client,
            _clientSettings,
            Options.Create(new MessagePackMediaTypeFormatter(MessagePackDefaults.SerializerOptions))
        );
    }
}
