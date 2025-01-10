namespace Byndyusoft.ApiClient.Functional;

using System.Net.Http.ProtoBuf;
using System.Net.Http.ProtoBuf.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models;
using ProtoBuf.Meta;
using Xunit;

public class FormatterClientTests: MvcTestFixture
{
    private readonly TypeModel _typeModel;
    private readonly TestFormatterClient TestSubject;
    public FormatterClientTests()
    {
        _typeModel = RuntimeTypeModel.Default;
        var formatter = new ProtoBufMediaTypeFormatter(_typeModel);
        TestSubject = new TestFormatterClient
            (
                Client,
                formatter,
                new OptionsWrapper<ApiClientSettings>(
                    new ApiClientSettings()
                )
            );
    }
    protected override void ConfigureMvc(IMvcCoreBuilder builder)
    {
        builder.AddProtoBufNet(
            options => { options.Model = _typeModel; });
    }
    
    [Fact]
    public async Task PostAsProtoBufAsync()
    {
        // Arrange
        var input = SimpleType.Create();
        var cancel = CancellationToken.None;

        // Act
        var result = await TestSubject.PostAsync<SimpleType>("/protobuf-formatter", input, cancel);

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<SimpleType>(result);
        model.Verify();
    }
}