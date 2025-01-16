namespace Byndyusoft.ApiClient.Functional.Models;

using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public static class ListModel
{
    [ProtoMember(0)] public static Dictionary<int, List<SimpleModel>> Data = new();
}