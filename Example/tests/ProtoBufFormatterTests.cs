namespace Byndyusoft.ApiClient.Example.Tests;

using System.Net.Http.ProtoBuf;
using System.Net.Http.ProtoBuf.Formatting;
using Client;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

public class ProtoBufFormatterTests : FormattersTests
{
    public ProtoBufFormatterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        TestSubject = new PersonModelListClient(
            Client,
            _clientSettings,
            Options.Create(new ProtoBufMediaTypeFormatter(ProtoBufDefaults.TypeModel))
        );
    }
}
