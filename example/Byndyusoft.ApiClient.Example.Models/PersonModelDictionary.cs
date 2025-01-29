namespace Byndyusoft.ApiClient.Example.Models;

using System.Collections.Concurrent;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public static class PersonModelDictionary
{
    [ProtoMember(0)] public static ConcurrentDictionary<int, Dictionary<ulong, PersonModel>> Data = new();
}