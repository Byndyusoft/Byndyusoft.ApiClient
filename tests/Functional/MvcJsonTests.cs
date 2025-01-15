namespace Byndyusoft.ApiClient.Functional
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Net.Http.Json.Formatting;
    using System.Text.Json;
    using System.Text.Json.Serialization.Metadata;
    using System.Threading;
    using System.Threading.Tasks;
    using Byndyusoft.ApiClient.Models;
    using Client;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Xunit;

    public class MvcJsonTests : MvcTestFixture
    {
        private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
                                                                   {
                                                                       TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                                                                   };
        private readonly TestFormatterClient _testSubject;

        public MvcJsonTests()
        {
            _testSubject = new TestFormatterClient(
                Client,
                new JsonMediaTypeFormatter(_serializerOptions),
                new OptionsWrapper<ApiClientSettings>(
                    new ApiClientSettings
                    {
                        ConnectionString = _url
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
                options =>
                {
                    options.JsonSerializerOptions.CopyFrom(_serializerOptions);
                }
            );
        }

        [Fact]
        public async Task PostAsMessagePackAsync()
        {
            // Arrange
            var input = SimpleModel.Create();

            // Act
            var response = await _testSubject.PostAsync<SimpleModel>("/formatter/post", input, CancellationToken.None);
            
            // Assert
            Assert.NotNull(response);
            var model = Assert.IsType<SimpleModel>(response);
            model.Verify();
        }

        [Fact]
        public async Task PutAsMessagePackAsync()
        {
            // Arrange
            var input = SimpleModel.Create();

            // Act
            var response = await _testSubject.PutAsync<SimpleModel>("/formatter/put", input, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            var model = Assert.IsType<SimpleModel>(response);

            model.Verify();
        }

        [Fact]
        public async Task GetFromMessagePackAsync()
        {
            // Act
            var response = await _testSubject.GetAsync<SimpleModel>("/formatter/get", CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            var model = Assert.IsType<SimpleModel>(response);

            model.Verify();
        }
    }
}