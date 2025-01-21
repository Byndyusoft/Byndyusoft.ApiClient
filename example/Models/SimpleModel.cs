namespace Byndyusoft.ApiClient.Models;

using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using ProtoBuf;

[ProtoContract]
public class SimpleModel
{
    [ProtoMember(1)] public string FirstName { get; set; }

    [ProtoMember(2)][JsonInclude]  public string LastName = default!;

    [ProtoMember(2)] public DateTime DateOfBirth { get; set; }

    [ProtoMember(3)] public bool IsMarried { get; set; }

    [ProtoMember(4)] public SeekOrigin Enum { get; set; }

    [ProtoMember(5)] public ulong Id { get; set; }

    [ProtoMember(6)] public ulong? MilitaryId { get; set; }

    [ProtoMember(7)] public string[] ChildrenNames { get; set; } = default!;

    public SimpleModel(string firstName, string lastName, DateTime dateOfBirth, ulong? militaryId, string[] childrenNames)
    {
        FirstName = firstName;
    }
}