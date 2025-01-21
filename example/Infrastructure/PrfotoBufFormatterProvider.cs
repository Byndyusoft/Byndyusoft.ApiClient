namespace Byndyusoft.ApiClient.Infrastructure;

using System.Net.Http.Formatting;
using System.Net.Http.ProtoBuf;
using System.Net.Http.ProtoBuf.Formatting;
using Interfaces;

public class PrfotoBufFormatterProvider: IFormatterProvider
{
    public PrfotoBufFormatterProvider()
    {
        Formatter = new ProtoBufMediaTypeFormatter(ProtoBufDefaults.TypeModel);
    }

    public MediaTypeFormatter Formatter { get; }
}