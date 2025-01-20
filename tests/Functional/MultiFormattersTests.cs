using Xunit.Abstractions;

namespace Byndyusoft.ApiClient.Functional
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Net.Http.Json.Formatting;
    using System.Text.Json;
    using System.Text.Json.Serialization.Metadata;
    using System.Threading.Tasks;
    using System.Threading;
    using Byndyusoft.ApiClient.Client;
    using Extensions;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Xunit;
    using Xunit.Sdk;

    public class MultiFormattersTests : MvcTestFixture
    {
        private readonly ITestOutputHelper _testOutputHelper;
        protected TestFormatterSimpleModelClient TestSubjectCamelCase;
        protected TestFormatterSimpleModelClient TestSubjectSnakeCase;
        private readonly JsonSerializerOptions CamelCaseOptions = new(JsonSerializerDefaults.Web)
                                                                  {
                                                                      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                                                      TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                                                                  };
        private readonly JsonSerializerOptions SnakeCaseOptions = new(JsonSerializerDefaults.Web)
                                                                  {
                                                                      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                                                                      TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                                                                  };
        private static int _idProvider = 0;

        public MultiFormattersTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var camelCaseClient = _host.GetTestClient();
            camelCaseClient.DefaultRequestHeaders.Accept.Add(JsonDefaults.MediaTypeHeader);
            TestSubjectCamelCase = new TestFormatterSimpleModelClient(
                camelCaseClient,
                Options.Create(
                    new ApiClientSettings
                    {
                        ConnectionString = URL
                    }
                ),
                new JsonMediaTypeFormatter(CamelCaseOptions)
            );
            var snakeCaseClient = _host.GetTestClient();
            var snakeCassMediaType = MediaTypeWithQualityHeaderValue.Parse("application/json+snake");
            snakeCaseClient.DefaultRequestHeaders.Accept.Add(snakeCassMediaType);
            TestSubjectSnakeCase = new TestFormatterSimpleModelClient(
                snakeCaseClient,
                Options.Create(
                    new ApiClientSettings
                    {
                        ConnectionString = URL
                    }
                ),
                new JsonMediaTypeFormatter(SnakeCaseOptions)
            );
        }
        protected override void ConfigureMvc(IMvcCoreBuilder builder)
        {
            builder
                .AddJsonOptions(
                    options => { options.JsonSerializerOptions.CopyFrom(CamelCaseOptions); }
                )
                .AddJsonOptions(
                    "snake",
                    options => { options.JsonSerializerOptions.CopyFrom(SnakeCaseOptions); }
            );
        }

        [Fact]
        protected async Task GivenTwoFormattersExpectSameResultsTest()
        {
            var id = Interlocked.Increment(ref _idProvider);
            _testOutputHelper.WriteLine($"id: {id}");
            var cancel = CancellationToken.None;

            // Act
            var snakeCaseContent = await TestSubjectSnakeCase.GetModelAsync(cancel);
            await TestSubjectCamelCase.AddListAsync(id, cancel);
            var item = await TestSubjectCamelCase.PostModelToListAsync(id, snakeCaseContent, cancel);
            await TestSubjectSnakeCase.DeleteListAsync(id, cancel);

            //Assert
            Assert.Equal(snakeCaseContent, item);
        }
    }
}
