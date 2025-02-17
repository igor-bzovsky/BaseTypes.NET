# BaseTypes.NET

BaseTypes.NET is a small but useful library containing fundamental types for object-oriented and domain-driven design (DDD).

## 📌 Available Types:
- `Entity<TId>` – A base class for entities with a unique identifier.
- `ValueObject` – A value object class without an identifier.
- `Enumeration` – A base class for representing strongly-typed enumerations.
- `Unit` – A type representing an absence of a meaningful result, often used in functional programming.
- `Error` – A structured representation of an error.

## 🔍 Entity vs. ValueObject
### **Entity<TId>**
Entities represent objects with a **unique identity** that remains constant throughout their lifecycle. Even if an entity’s properties change, it is still the same entity as long as its **ID remains unchanged**. Entities are typically stored in a database and referenced by their unique identifier.

#### **Key Characteristics:**
- Has a unique identifier (`Id`).
- Identity does not change even if attributes change.
- Compared based on identity (ID), not attribute values.
- Implements equality using `Equals` and `GetHashCode` based on `Id`.

#### **Example:**
```csharp
public class User : Entity<Guid>
{
    public string Name { get; }
    public User(Guid id, string name) : base(id) => Name = name;
}

var user1 = new User(Guid.NewGuid(), "Alice");
var user2 = new User(Guid.NewGuid(), "Alice");
Console.WriteLine(user1 == user2); // false, since IDs are different
```

### **ValueObject**
Value objects, on the other hand, represent **conceptual values** rather than distinct entities. Their identity is defined by the combination of their attribute values, meaning that two value objects are considered equal if all their attributes are identical.

#### **Key Characteristics:**
- Does not have a unique identifier.
- Completely interchangeable if their values are the same.
- Compared by value (all attributes must be equal).
- Useful for representing concepts like monetary amounts, coordinates, or measurements.

#### **Example:**
```csharp
public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}

var money1 = new Money(100, "USD");
var money2 = new Money(100, "USD");
var money3 = new Money(50, "USD");

Console.WriteLine(money1 == money2); // true, as values are identical
Console.WriteLine(money1 == money3); // false, since Amount is different
```

## ⚖️ How Equals Works
### **Entity Equals Implementation**
Entities implement equality by comparing their **ID**:
```csharp
public override bool Equals(object? obj)
{
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != this.GetType()) return false;
    return EqualsCore((Entity<TId>)obj);
}

private bool EqualsCore(Entity<TId> other) => Id.Equals(other.Id);
```
- If the reference is the same, it returns `true`.
- If the object type is different, it returns `false`.
- Otherwise, it compares **only the ID**.

### **ValueObject Equals Implementation**
Value objects implement equality by comparing **all of their attributes**:
```csharp
private bool EqualsCore(ValueObject other)
    => this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
```
- The equality check ensures that all attributes are equal.
- If **all** attributes match, the objects are considered equal.
- This approach makes value objects **immutable and interchangeable**.

## 🚀 Installation
You can install the package via NuGet:
```sh
dotnet add package BaseTypes.NET
```

For more details, check the [documentation](docs/) or visit the Wiki.

---

## 📄 License
BaseTypes.NET is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## 🤝 Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.


