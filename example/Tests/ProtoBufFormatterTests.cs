namespace Byndyusoft.ApiClient.Example.Tests;

using System.Net.Http.ProtoBuf;
using System.Net.Http.ProtoBuf.Formatting;
using Api.Client;
using Xunit.Abstractions;

public class ProtoBufFormatterTests : FormattersTests
{
    public ProtoBufFormatterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        TestSubject = new PersonApi(
            Client,
            _clientSettings,
            new ProtoBufMediaTypeFormatter(ProtoBufDefaults.TypeModel)
        );
    }
}
