namespace Byndyusoft.ApiClient.Example.Tests;

using System.Net.Http;
using System.Net.Http.MessagePack;
using System.Net.Http.MessagePack.Formatting;
using Client;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

public class MessagePackFormatterTests : FormattersTests
{
    private readonly MessagePackSerializerOptions _serializerOptions;

    public MessagePackFormatterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        _serializerOptions = MessagePackDefaults.SerializerOptions;
        TestSubject = new PersonModelListClient(
            Client,
            new OptionsWrapper<ApiClientSettings>(
                new ApiClientSettings
                {
                    ConnectionString = URL
                }
            ),
            Options.Create(new MessagePackMediaTypeFormatter(_serializerOptions))
        );
    }

    protected override void ConfigureHttpClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(MessagePackDefaults.MediaTypeHeader);
    }

    protected override void ConfigureMvc(IMvcCoreBuilder builder)
    {
        builder.AddMessagePackFormatters(options => { options.SerializerOptions = _serializerOptions; });
    }
}
