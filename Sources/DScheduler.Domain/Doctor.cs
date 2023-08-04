namespace DScheduler.Domain;

public sealed class Doctor : IEquatable<Doctor>
{
    public Doctor(string name)
    {
        Name = name;
    }

    public string Name { get; }

    /// <inheritdoc />
    public bool Equals(Doctor? other)
    {
        if (ReferenceEquals(objA: null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is Doctor other && Equals(other));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public static bool operator ==(Doctor? left, Doctor? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Doctor? left, Doctor? right)
    {
        return !Equals(left, right);
    }
}