namespace Byndyusoft.ApiClient.Models;

using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using ProtoBuf;

[ProtoContract]
public class SimpleModel
{
    [ProtoMember(1)] public int PropertyForCaseTesting { get; set; }

    [ProtoMember(2)][JsonInclude]  public string FieldForCaseTesting = default!;

    [ProtoMember(3)] public SeekOrigin Enum { get; set; }

    [ProtoMember(4)] public int? Nullable { get; set; }

    [ProtoMember(5)] public int[] Array { get; set; } = default!;
    public static SimpleModel Create(
        int property = 10,
        SeekOrigin @enum = SeekOrigin.Current,
        string field = "string",
        int[] array = null,
        int? nullable =  100
    )
    {
        return new()
               {
                   PropertyForCaseTesting = property,
                   Enum = SeekOrigin.Current,
                   FieldForCaseTesting = field,
                   Array = [1, 2],
                   Nullable = nullable
               };
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is SimpleModel model)
            return Equals(model);
        return false;
    }

    protected bool Equals(SimpleModel other)
    {
        return
            PropertyForCaseTesting == other.PropertyForCaseTesting
            && string.Equals(FieldForCaseTesting, FieldForCaseTesting, StringComparison.InvariantCulture)
            && Enum == other.Enum
            && System.Nullable.Equals(Nullable, other.Nullable)
            && Array.SequenceEqual(other.Array);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FieldForCaseTesting, PropertyForCaseTesting, (int)Enum, Nullable, Array);
    }
}