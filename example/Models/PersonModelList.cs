namespace Byndyusoft.ApiClient.Models;

using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public static class PersonModelList
{
    [ProtoMember(0)] public static Dictionary<int, Dictionary<ulong, PersonModel>> Data = new();
}