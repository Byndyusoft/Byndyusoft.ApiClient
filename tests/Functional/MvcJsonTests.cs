namespace Byndyusoft.ApiClient.Functional;

using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Json.Formatting;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

public class MvcJsonTests : MvcFormattersTests
{
    private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
                                                                {
                                                                    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                                                                };

    public MvcJsonTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        TestSubject = new TestFormatterClient(
            Client,
            new JsonMediaTypeFormatter(_serializerOptions),
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
        client.DefaultRequestHeaders.Accept.Add(JsonDefaults.MediaTypeHeader);
    }

    protected override void ConfigureMvc(IMvcCoreBuilder builder)
    {
        builder.AddJsonOptions
        (
            options => { options.JsonSerializerOptions.CopyFrom(_serializerOptions); }
        );
    }
}
