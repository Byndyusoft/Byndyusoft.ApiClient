namespace Byndyusoft.ApiClient.Models;

using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using ProtoBuf;

[ProtoContract]
public class PersonModel
{
    [ProtoMember(1)][JsonInclude] public string? FirstName { get; set; }

    [ProtoMember(2)][JsonInclude] public string? LastName = null;

    [ProtoMember(2)] public DateTime? DateOfBirth { get; set; }

    [ProtoMember(3)] public bool? IsMarried { get; set; }

    [ProtoMember(4)] public FavoriteDessertEnum? FavoriteDessert { get; set; }

    [ProtoMember(5)] public ulong? Id { get; set; }

    [ProtoMember(6)] public ulong? DriverLicenseId { get; set; }

    [ProtoMember(7)] public string[]? ChildrenNames { get; set; }

    public PersonModel(
        string? firstName = null,
        string? lastName = null,
        DateTime? dateOfBirth = null,
        string[]? childrenNames = null,
        bool? isMarried = null,
        FavoriteDessertEnum? favoriteDessert = null,
        ulong? id = null,
        ulong? driverLicenseId = null)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        IsMarried = isMarried;
        FavoriteDessert = favoriteDessert;
        Id = id;
        DriverLicenseId = driverLicenseId;
        ChildrenNames = childrenNames;
    }

    public static PersonModel Create() => new(
        "Jhon",
        "Doe",
        DateTime.UnixEpoch,
        ["Jane"],
        true,
        FavoriteDessertEnum.Apple,
        111
    );

    public override int GetHashCode() =>
        HashCode.Combine(FirstName, LastName, DateOfBirth, IsMarried, FavoriteDessert, Id, DriverLicenseId, ChildrenNames);

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj == null) return false;
        if (obj is PersonModel other) return Equals(other);
        return false;
    }

    private bool Equals(PersonModel other)
    {
        return
            FirstName == other.FirstName &&
            LastName == other.LastName &&
            DateOfBirth == other.DateOfBirth &&
            IsMarried == other.IsMarried &&
            FavoriteDessert == other.FavoriteDessert &&
            Id == other.Id &&
            Nullable.Equals(DriverLicenseId, other.DriverLicenseId) &&
            ChildrenNames.SequenceEqual(other.ChildrenNames);
    }
}