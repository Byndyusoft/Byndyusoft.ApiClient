namespace Models;

using System;
using System.Linq;
using System.Text.Json.Serialization;
using ProtoBuf;

[ProtoContract]
public class PersonModel
{
    [ProtoMember(1)][JsonInclude] public string? FirstName { get; set; }

    [ProtoMember(2)][JsonPropertyName(nameof(LastName))][JsonInclude] public string? LastName = String.Empty;

    [ProtoMember(3)] public DateTime? DateOfBirth { get; set; }

    [ProtoMember(4)] public bool? IsMarried { get; set; }

    [ProtoMember(5)] public FavoriteDessertEnum? FavoriteDessert { get; set; }

    [ProtoMember(6)] public ulong Id { get; set; }

    [ProtoMember(7)] public ulong? DriverLicenseId { get; set; }

    [ProtoMember(8)] public string[]? ChildrenNames { get; set; }

    public PersonModel(
        string? firstName = null,
        string? lastName = null,
        DateTime? dateOfBirth = null,
        string[]? childrenNames = null,
        bool? isMarried = null,
        FavoriteDessertEnum? favoriteDessert = null,
        ulong id = 0,
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

    public PersonModel()
    {
        FirstName = null;
        LastName = null;
        DateOfBirth = null;
        IsMarried = null;
        FavoriteDessert = null;
        Id = 0;
        DriverLicenseId = null;
        ChildrenNames = null;
    }

    public static PersonModel Create() => new(
        "John",
        "Doe",
        DateTime.UnixEpoch,
        ["Jane"],
        true,
        FavoriteDessertEnum.Apple,
        111
    );

    public override int GetHashCode() =>
        HashCode.Combine(FirstName, LastName, DateOfBirth, IsMarried, FavoriteDessert, Id, DriverLicenseId, ChildrenNames);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj == null) return false;
        if (obj is PersonModel other) return Equals(other);
        return false;
    }

    private bool Equals(PersonModel other)
    {
        var childrenEqual = ChildrenNames?.SequenceEqual(other.ChildrenNames) ?? other.ChildrenNames == null;
        return
            String.Equals(FirstName, other.FirstName, StringComparison.InvariantCulture) &&
            String.Equals(LastName, other.LastName, StringComparison.InvariantCulture) &&
            Nullable.Equals(DateOfBirth, other.DateOfBirth) &&
            Nullable.Equals(IsMarried, other.IsMarried) &&
            Nullable.Equals(FavoriteDessert, other.FavoriteDessert) &&
            Id == other.Id &&
            Nullable.Equals(DriverLicenseId, other.DriverLicenseId) &&
            childrenEqual;
    }
}