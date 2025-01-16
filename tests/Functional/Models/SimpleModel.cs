namespace Byndyusoft.ApiClient.Functional.Models;

using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using ProtoBuf;
using Xunit;

[ProtoContract]
public class SimpleModel
{
    [ProtoMember(2)][JsonInclude] public string Field = default!;

    [ProtoMember(1)] public int Property { get; set; }

    [ProtoMember(3)] public SeekOrigin Enum { get; set; }

    [ProtoMember(4)] public int? Nullable { get; set; }

    [ProtoMember(5)] public int[] Array { get; set; } = default!;
    public static SimpleModel Create
    (
        int property = 10,
        SeekOrigin @enum = SeekOrigin.Current,
        string field = "string",
        int[] array = null,
        int? nullable = 100
    )
    {
        return new()
               {
                   Property = property,
                   Enum = @enum,
                   Field = field,
                   Array = array ?? [1, 2],
                   Nullable = nullable
               };
    }

    public void Verify()
    {
        var input = Create();

        Assert.Equal(input.Property, Property);
        Assert.Equal(input.Field, Field);
        Assert.Equal(input.Enum, Enum);
        Assert.Equal(input.Array, Array);
        Assert.Equal(input.Nullable, Nullable);
    }

    public void Verify(ParamsTestModel model)
    {
        var input = Create
        (
            model.property,
            field: model.field,
            nullable:model.nullable
        );

        Assert.Equal(input.Property, Property);
        Assert.Equal(input.Field, Field);
        Assert.Equal(input.Enum, Enum);
        Assert.Equal(input.Array, Array);
        Assert.Equal(input.Nullable, Nullable);
    }

    public override bool Equals(object obj)
    {
        if (obj is SimpleModel model)
            return
                Property == model.Property
                && String.Equals(Field, model.Field, StringComparison.InvariantCulture)
                && Enum == model.Enum
                && System.Nullable.Equals(Nullable, model.Nullable)
                && Array.SequenceEqual(model.Array);
        return false;
    }
}