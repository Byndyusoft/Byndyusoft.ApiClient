namespace Byndyusoft.ApiClient;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.MessagePack;
using System.Net.Http.MessagePack.Formatting;
using System.Net.Http.ProtoBuf;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Client;
using Functional;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public class BenchmarkData
{
    private BenchmarkData(Lazy<TestFormatterSimpleModelClient> testSubject, Action<IMvcCoreBuilder> mvcConfiguration,
        Action<HttpClient> clientConfiguration)
    {
        TestSubject = testSubject;
        MvcConfiguration = mvcConfiguration;
        ClientConfiguration = clientConfiguration;
    }

    public Lazy<TestFormatterSimpleModelClient> TestSubject { get; }
    public Action<IMvcCoreBuilder> MvcConfiguration { get; }
    public Action<HttpClient> ClientConfiguration { get; }

    public static BenchmarkData Create(HttpClient client, BenchmarkDataType type)
    {
        switch (type)
        {
            case BenchmarkDataType.ProtoBuf:
                return new BenchmarkData(new Lazy<TestFormatterSimpleModelClient>(new TestFormatterSimpleModelClient(
                        client,
                        new OptionsWrapper<ApiClientSettings>(
                            new ApiClientSettings
                            {
                                ConnectionString = client.BaseAddress!.AbsoluteUri,
                            }
                        ),
                        new PrfotoBufFormatterProvider()
                    )),
                    builder => builder.AddProtoBufNet(options => { options.Model = ProtoBufDefaults.TypeModel; }),
                    httpClient => httpClient.DefaultRequestHeaders.Accept.Add(ProtoBufDefaults.MediaTypeHeader));
            case BenchmarkDataType.MessagePack:
                return new BenchmarkData(new Lazy<TestFormatterSimpleModelClient>(new TestFormatterSimpleModelClient(
                        client,
                        new OptionsWrapper<ApiClientSettings>(
                            new ApiClientSettings
                            {
                                ConnectionString = client.BaseAddress!.AbsoluteUri,
                            }
                        ),
                        new TestFormatterProvider(new MessagePackMediaTypeFormatter(MessagePackDefaults.SerializerOptions))
                    )),
                    builder => builder
                        .AddMessagePackFormatters(
                            options =>
                            {
                                options.SerializerOptions = MessagePackDefaults.SerializerOptions;
                            }
                        ),
                    httpClient => httpClient.DefaultRequestHeaders.Accept.Add(MessagePackDefaults.MediaTypeHeader));
            case BenchmarkDataType.Json:
            default:
                return new BenchmarkData(new Lazy<TestFormatterSimpleModelClient>(new TestFormatterSimpleModelClient(
                        client,
                        new OptionsWrapper<ApiClientSettings>(
                            new ApiClientSettings
                            {
                                ConnectionString = client.BaseAddress!.AbsoluteUri,
                            }
                        )
                    )),
                    builder => builder.AddJsonOptions(
                        options =>
                        {
                            options.JsonSerializerOptions.CopyFrom(
                                new(JsonSerializerDefaults.Web)
                                {
                                    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                                });
                        }
                    ),
                    httpClient => httpClient.DefaultRequestHeaders.Accept.Add(JsonDefaults.MediaTypeHeader));
        }
    }
}