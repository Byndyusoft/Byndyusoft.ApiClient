namespace Byndyusoft.ApiClient.Functional
{
    using System.Net.Http;
    using System.Net.Http.MessagePack;
    using System.Net.Http.MessagePack.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using Byndyusoft.ApiClient.Models;
    using Client;
    using MessagePack;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Xunit;

    public class MvcMessagePackTests : MvcTestFixture
    {
        private readonly MessagePackSerializerOptions _serializerOptions;
        private readonly TestFormatterClient _testSubject;

        public MvcMessagePackTests()
        {
            _serializerOptions = MessagePackDefaults.SerializerOptions;
            _testSubject = new TestFormatterClient(
                Client,
                new MessagePackMediaTypeFormatter(_serializerOptions),
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
            client.DefaultRequestHeaders.Accept.Add(MessagePackDefaults.MediaTypeHeader);
        }

        protected override void ConfigureMvc(IMvcCoreBuilder builder)
        {
            builder.AddMessagePackFormatters(options => { options.SerializerOptions = _serializerOptions; });
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