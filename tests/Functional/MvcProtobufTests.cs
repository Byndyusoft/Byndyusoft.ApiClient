namespace Byndyusoft.ApiClient.Functional;

using System.Net.Http;
using System.Net.Http.ProtoBuf;
using System.Net.Http.ProtoBuf.Formatting;
using Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProtoBuf.Meta;
using Xunit.Abstractions;

public class MvcProtobufTests : MvcFormattersTests
{
    private readonly TypeModel _typeModel;

    public MvcProtobufTests(ITestOutputHelper testOutputHelper):base(testOutputHelper)
    {
        _typeModel = ProtoBufDefaults.TypeModel;
        TestSubject = new TestFormatterSimpleModelClient(
            Client,
            new ProtoBufMediaTypeFormatter(_typeModel),
            new OptionsWrapper<ApiClientSettings>(
                new ApiClientSettings
                {
                    ConnectionString = URL
                }
            )
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
}
