namespace Byndyusoft.ApiClient.Functional;

using System.Net.Http;
using System.Net.Http.ProtoBuf;
using System.Net.Http.ProtoBuf.Formatting;
using Example.Client;
using Example.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProtoBuf.Meta;
using Xunit;
using Xunit.Abstractions;

public class MvcProtobufTests : MvcFormattersTests
{
    private readonly TypeModel _typeModel= ProtoBufDefaults.TypeModel;

    public MvcProtobufTests(ITestOutputHelper testOutputHelper):base(testOutputHelper)
    {
        TestSubject = new PersonModelListClient(
            Client,
            new OptionsWrapper<ApiClientSettings>(
                new ApiClientSettings
                {
                    ConnectionString = URL
                }
            ),
            Options.Create(new ProtoBufMediaTypeFormatter(ProtoBufDefaults.TypeModel))
        );
    }

    protected override void ConfigureHttpClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(ProtoBufDefaults.MediaTypeHeader);
    }

    protected override void ConfigureMvc(IMvcCoreBuilder builder)
    {
        builder
            .AddProtoBufNet(options => { options.Model = _typeModel; });
    }

    [Fact]
    public void ExpectPersonModelSerializedTest()
    {
        var formatter = new ProtoBufMediaTypeFormatter(ProtoBufDefaults.TypeModel);
        Assert.True(formatter.CanReadType(typeof(PersonModel)));
        Assert.True(formatter.CanWriteType(typeof(PersonModel)));
    }
}
