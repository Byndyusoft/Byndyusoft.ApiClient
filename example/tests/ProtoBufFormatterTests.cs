namespace Byndyusoft.ApiClient.Example.Tests;

using System.Net.Http;
using System.Net.Http.ProtoBuf;
using System.Net.Http.ProtoBuf.Formatting;
using Client;
using Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProtoBuf.Meta;
using Xunit;
using Xunit.Abstractions;

public class ProtoBufFormatterTests : FormattersTests
{
    private readonly TypeModel _typeModel = ProtoBufDefaults.TypeModel;

    public ProtoBufFormatterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        TestSubject = new PersonModelListClient(
            Client,
            _clientSettings,
            Options.Create(new ProtoBufMediaTypeFormatter(ProtoBufDefaults.TypeModel))
        );
    }
}
