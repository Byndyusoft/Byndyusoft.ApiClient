namespace Byndyusoft.ApiClient.Models;

using System.IO;
using System.Text.Json.Serialization;
using ProtoBuf;

[ProtoContract]
public class SimpleModel
{
    [ProtoMember(2)][JsonInclude]  public string Field = default!;

    [ProtoMember(1)] public int Property { get; set; }

    [ProtoMember(3)] public SeekOrigin Enum { get; set; }

    [ProtoMember(4)] public int? Nullable { get; set; }

    [ProtoMember(5)] public int[] Array { get; set; } = default!;

    public static SimpleModel Create()
    {
        return new()
               {
                   Property = 10,
                   Enum = SeekOrigin.Current,
                   Field = "string",
                   Array = [1, 2],
                   Nullable = 100
               };
    }
    public static SimpleModel Create(int property, string field, int? nullable)
    {
        return new()
               {
                   Property = property,
                   Enum = SeekOrigin.Current,
                   Field = field,
                   Array = [1, 2],
                   Nullable = nullable
               };
    }
}