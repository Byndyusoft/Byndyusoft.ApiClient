namespace Benchmark;

using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.MessagePack;
using System.Net.Http.MessagePack.Formatting;
using System.Net.Http.ProtoBuf;
using System.Net.Http.ProtoBuf.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Api.Client;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Byndyusoft.ApiClient;
using Models;
using Microsoft.Extensions.Options;

[SimpleJob(RunStrategy.Throughput)]
[AllStatisticsColumn]
public class FormattersBenchmark : MvcTestFixture
{
    private PersonApi TestSubject;
    private static int _idProvider = 0;
    
    [GlobalSetup]
    public void Setup()
    {
        var settings =
            new OptionsWrapper<ApiClientSettings>(
                new ApiClientSettings
                {
                    ConnectionString = "http://localhost:5000",
                }
            );
        switch (DataType)
        {
            case BenchmarkDataType.ProtoBuf:
                TestSubject = new PersonApi(
                    Client,
                    settings,
                    new ProtoBufMediaTypeFormatter(ProtoBufDefaults.TypeModel)
                );
                break;
            case BenchmarkDataType.MessagePack:
                TestSubject = new PersonApi(
                    Client,
                    settings,
                    new MessagePackMediaTypeFormatter(MessagePackDefaults.SerializerOptions)
                );
                break;
            case BenchmarkDataType.Json:
            default:
                TestSubject = new PersonApi(
                    Client,
                    settings
                );
                break;
        }
    }

    [Params(BenchmarkDataType.MessagePack, BenchmarkDataType.ProtoBuf, BenchmarkDataType.Json)]
    public BenchmarkDataType DataType = BenchmarkDataType.Json;
    
    [Benchmark]
    public async Task<Dictionary<ulong,PersonModel>> ListStressTest()
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
        var response = await TestSubject.GetEveryPersonAsync(id, cancel);
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