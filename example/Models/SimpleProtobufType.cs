namespace Byndyusoft.ApiClient.Models;

using System.IO;
using ProtoBuf;

[ProtoContract]
public class SimpleProtobufType
{
    [ProtoMember(2)] public string Field = default!;

    [ProtoMember(1)] public int Property { get; set; }

    [ProtoMember(3)] public SeekOrigin Enum { get; set; }

    [ProtoMember(4)] public int? Nullable { get; set; }

    [ProtoMember(5)] public int[] Array { get; set; } = default!;

    public static SimpleProtobufType Create()
    {
        return new()
               {
                   Property = 10,
                   Enum = SeekOrigin.Current,
                   Field = "string",
                   Array = new[] {1, 2},
                   Nullable = 100
               };
    }
}