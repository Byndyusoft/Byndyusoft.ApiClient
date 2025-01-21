namespace Byndyusoft.ApiClient.Functional;

using System.Net.Http;
using System.Net.Http.ProtoBuf;
using Client;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProtoBuf.Meta;
using Xunit.Abstractions;

public class MvcProtobufTests : MvcFormattersTests
{
    private readonly TypeModel _typeModel= ProtoBufDefaults.TypeModel;

    public MvcProtobufTests(ITestOutputHelper testOutputHelper):base(testOutputHelper)
    {
        TestSubject = new TestFormatterSimpleModelClient(
            Client,
            new OptionsWrapper<ApiClientSettings>(
                new ApiClientSettings
                {
                    ConnectionString = URL
                }
            ),
            new PrfotoBufFormatterProvider()
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
