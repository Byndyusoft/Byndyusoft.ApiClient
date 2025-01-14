namespace Byndyusoft.ApiClient.Functional
{
    using System.Net.Http;
    using System.Net.Http.MessagePack;
    using System.Net.Http.MessagePack.Formatting;
    using System.Net.Http.ProtoBuf;
    using System.Net.Http.ProtoBuf.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using Byndyusoft.ApiClient.Models;
    using Client;
    using MessagePack;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using ProtoBuf.Meta;
    using Xunit;
    public class MvcProtobufTests : MvcTestFixture
    {
        private readonly TypeModel _typeModel;
        private readonly TestFormatterClient _testSubject;

        public MvcProtobufTests()
        {
            _typeModel = ProtoBufDefaults.TypeModel;
            _testSubject = new TestFormatterClient(
                Client,
                new ProtoBufMediaTypeFormatter(_typeModel),
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
            client.DefaultRequestHeaders.Accept.Add(ProtoBufDefaults.MediaTypeHeader);
        }

        protected override void ConfigureMvc(IMvcCoreBuilder builder)
        {
            builder.AddProtoBufFormatters(options => { options.TypeModel = _typeModel; });
        }
        [Fact(Skip = "Protobuf not working")]
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

        [Fact(Skip = "Protobuf not working")]
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

        [Fact(Skip = "Protobuf not working")]
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