namespace Byndyusoft.ApiClient;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.MessagePack;
using System.Net.Http.MessagePack.Formatting;
using System.Net.Http.ProtoBuf;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Client;
using Models;
using Functional;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[SimpleJob(RunStrategy.Throughput)]
[AllStatisticsColumn]
public class FormattersBenchmark : MvcTestFixture
{
    private BenchmarkData _benchmarkData;
    protected TestFormatterSimpleModelClient TestSubject;
    private static int _idProvider = 0;
    
    [GlobalSetup]
    public void Setup()
    {
        //TestSubject = _benchmarkData.TestSubject.Value;
        switch (DataType)
        {
            case BenchmarkDataType.ProtoBuf:
                TestSubject = new TestFormatterSimpleModelClient(
                    Client,
                    new OptionsWrapper<ApiClientSettings>(
                        new ApiClientSettings
                        {
                            ConnectionString = Client.BaseAddress!.AbsoluteUri,
                        }
                    ),
                    new PrfotoBufFormatterProvider()
                );
                break;
            case BenchmarkDataType.MessagePack:
                TestSubject = new TestFormatterSimpleModelClient(
                    Client,
                    new OptionsWrapper<ApiClientSettings>(
                        new ApiClientSettings
                        {
                            ConnectionString = Client.BaseAddress!.AbsoluteUri,
                        }
                    ),
                    new TestFormatterProvider(new MessagePackMediaTypeFormatter(MessagePackDefaults.SerializerOptions))
                );
                break;
            case BenchmarkDataType.Json:
            default:
                TestSubject = new TestFormatterSimpleModelClient(
                    Client,
                    new OptionsWrapper<ApiClientSettings>(
                        new ApiClientSettings
                        {
                            ConnectionString = Client.BaseAddress!.AbsoluteUri,
                        }
                    )
                );
                break;
        }
    }

    protected override void ConfigureMvc(IMvcCoreBuilder builder)
    {
        switch (DataType)
        {
            case BenchmarkDataType.ProtoBuf:
                builder.AddProtoBufNet(options => { options.Model = ProtoBufDefaults.TypeModel; });
                break;
            case BenchmarkDataType.MessagePack:
                builder.AddMessagePackFormatters(
                    options =>
                    {
                        options.SerializerOptions = MessagePackDefaults.SerializerOptions;
                    }
                );
                break;
            case BenchmarkDataType.Json:
            default:
                builder.AddJsonOptions(
                    options =>
                    {
                        options.JsonSerializerOptions.CopyFrom(
                            new(JsonSerializerDefaults.Web)
                            {
                                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                            });
                    }
                );
                break;
        }
    }

    protected override void ConfigureHttpClient(HttpClient client)
    {
        switch (DataType)
        {
            case BenchmarkDataType.ProtoBuf:
                client.DefaultRequestHeaders.Accept.Add(ProtoBufDefaults.MediaTypeHeader);
                break;
            case BenchmarkDataType.MessagePack:
                client.DefaultRequestHeaders.Accept.Add(MessagePackDefaults.MediaTypeHeader);
                break;
            case BenchmarkDataType.Json:
            default:
                client.DefaultRequestHeaders.Accept.Add(JsonDefaults.MediaTypeHeader);
                break;
        }
    }

    [Params(BenchmarkDataType.MessagePack, BenchmarkDataType.ProtoBuf, BenchmarkDataType.Json)]
    public BenchmarkDataType DataType = BenchmarkDataType.Json;
    
    [Benchmark]
    public async Task<List<SimpleModel>> ListStressTest()
    {
        // Arrange
        var id = Interlocked.Increment(ref _idProvider);
        var cancel = CancellationToken.None;
        var input = SimpleModel.Create();
        var length = 10_000;
        var stopwatch = new Stopwatch();

        // Act
        await TestSubject.AddListAsync(id, cancel);
        stopwatch.Start();
        for (var i = 0; i < length; i++)
            await TestSubject.PostModelToListAsync(id, input, cancel);
        stopwatch.Stop();

        stopwatch.Reset();
        stopwatch.Start();
        var response = await TestSubject.GetAllFromListModelAsync(id, cancel);
        stopwatch.Stop();

        stopwatch.Reset();
        stopwatch.Start();
        for (var i = 0; i < length; i++)
        {
            await TestSubject.DeleteFromListAsync(id, cancel);
        }
        stopwatch.Stop();
        await TestSubject.DeleteListAsync(id, cancel);
        return response;
    }
}