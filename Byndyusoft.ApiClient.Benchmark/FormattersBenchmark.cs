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
using Functional;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.ProtoBuf.Formatting;
using Example.Client;
using Example.Models;

[SimpleJob(RunStrategy.Throughput)]
[AllStatisticsColumn]
public class FormattersBenchmark : MvcTestFixture
{
    protected PersonModelListClient TestSubject;
    private static int _idProvider = 0;
    
    [GlobalSetup]
    public void Setup()
    {
        //TestSubject = _benchmarkData.TestSubject.Value;
        switch (DataType)
        {
            case BenchmarkDataType.ProtoBuf:
                TestSubject = new PersonModelListClient(
                    Client,
                    new OptionsWrapper<ApiClientSettings>(
                        new ApiClientSettings
                        {
                            ConnectionString = Client.BaseAddress!.AbsoluteUri,
                        }
                    ),
                    Options.Create(new ProtoBufMediaTypeFormatter(ProtoBufDefaults.TypeModel))
                );
                break;
            case BenchmarkDataType.MessagePack:
                TestSubject = new PersonModelListClient(
                    Client,
                    new OptionsWrapper<ApiClientSettings>(
                        new ApiClientSettings
                        {
                            ConnectionString = Client.BaseAddress!.AbsoluteUri,
                        }
                    ),
                    Options.Create(new MessagePackMediaTypeFormatter(MessagePackDefaults.SerializerOptions))
                );
                break;
            case BenchmarkDataType.Json:
            default:
                TestSubject = new PersonModelListClient(
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
    public async Task<List<PersonModel>> ListStressTest()
    {
        // Arrange
        var id = Interlocked.Increment(ref _idProvider);
        var cancel = CancellationToken.None;
        var input = new PersonModel();
        var length = 10_000;
        var stopwatch = new Stopwatch();

        // Act
        await TestSubject.AddListAsync(id, cancel);
        stopwatch.Start();
        for (var i = 0; i < length; i++)
        {
            input.Id = (ulong)i;
            await TestSubject.AddPersonAsync(id, input, cancel);
        }

        stopwatch.Stop();

        stopwatch.Reset();
        stopwatch.Start();
        var response = await TestSubject.GetPersonListAsync(id, cancel);
        stopwatch.Stop();

        stopwatch.Reset();
        stopwatch.Start();
        for (var i = 0; i < length; i++)
        {
            var personId = new PersonId(id, (ulong)i);
            await TestSubject.DeletePersonAsync(personId, cancel);
        }
        stopwatch.Stop();
        await TestSubject.DeleteListAsync(id, cancel);
        return response;
    }
}