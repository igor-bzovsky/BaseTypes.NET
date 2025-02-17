# BaseTypes.NET

BaseTypes.NET is a small but useful library containing fundamental types for object-oriented programming and domain-driven design (DDD).

## 📌 Available Types:
- `Entity<TId>` – A base class for entities that have a unique identity throughout their lifecycle. Entities are distinguished by their identifier (`Id`), even if their properties change. They are commonly used to model real-world objects in domain-driven design (DDD).
- `ValueObject` – A base class for objects that do not have an identity but are defined by their attributes. Two `ValueObject` instances are considered equal if all their attributes match. They are immutable and useful for modeling concepts like monetary amounts, dates, or measurement units.
- `Enumeration` – A base class for representing named instances with assigned values, providing an alternative to traditional enums.
- `Unit` – A type representing an absence of a meaningful result, often used in functional programming to indicate the completion of a task without returning a value.
- `Error` – A structured representation of an error, encapsulating an error code and message to provide more meaningful failure handling.

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

## ⚖️ Equals Implementation
### **Why Implement IEquatable<T>?**
Both `Entity<TId>` and `ValueObject` implement `IEquatable<T>`, which provides optimized equality checks. This has several benefits:

1. **Improved Performance:** 
   - `IEquatable<T>` allows for direct comparison without boxing/unboxing (avoiding unnecessary object conversions).
   - More efficient in **collections** like `HashSet<T>` or `Dictionary<TKey, TValue>` where frequent equality checks occur.

2. **Type Safety:**
   - Ensures type-safe equality comparisons, preventing incorrect comparisons between unrelated types.

3. **Better Integration with .NET Collections:**
   - `IEquatable<T>` is used internally in many .NET APIs (e.g., LINQ operations, collections), ensuring consistent equality handling.

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

## 🔢 Enumeration
`Enumeration` is a base class for strongly-typed enumerations, offering more flexibility than built-in `enum` types. Unlike standard enums, Enumeration allows defining named instances with associated values and provides methods for retrieving and comparing them.

#### **Key Characteristics:**
- Allows creation of new enumerations and enumeration fields at runtime, offering greater flexibility.
- Provides methods for retrieving instances by value or display name.
- Implements equality and comparison logic. Compared by `Value` (not `DisplayName`).

### **Example:**
```csharp
public class OrderStatus : Enumeration
{
    public static readonly OrderStatus Pending = new(1, "Pending");
    public static readonly OrderStatus Shipped = new(2, "Shipped");
    public static readonly OrderStatus Delivered = new(3, "Delivered");
    
    private OrderStatus(int value, string displayName) : base(value, displayName) {}
}
```
#### **Usage Example:**
```csharp
// Retrieve all available statuses
var statuses = Enumeration.GetAll<OrderStatus>();
foreach (var status in statuses)
{
    Console.WriteLine($"{status.Value}: {status.DisplayName}");
}

// Get a status by its value
var shippedStatus = Enumeration.GetByValue<OrderStatus>(2);
Console.WriteLine(shippedStatus.DisplayName); // Output: Shipped

// Get a status by its display name
var deliveredStatus = Enumeration.GetByDisplayName<OrderStatus>("Delivered");
Console.WriteLine(deliveredStatus.Value); // Output: 3

// Compare enumeration values
if (OrderStatus.Pending != OrderStatus.Shipped)
{
    Console.WriteLine("Different statuses");
}
```

## ❗ Error
`Error` is a value object that represents an error message with a specific error code. 

#### **Key Characteristics:**
- Provides structured error handling.
- Can be used for returning meaningful failure states.

#### **Example:**
```csharp
public static class Errors
{
    public static readonly Error NotFound = new("404", "Resource not found");
    public static readonly Error ValidationError = new("400", "Invalid input provided");
}
```

## 🏳️ Unit
`Unit` is a struct representing a "void" return type, commonly used in functional programming.

#### **Key Characteristics:**
- Useful for functional constructs where a return value is required but not meaningful.
- Prevents the use of `null` in method chaining.

#### **Example:**
```csharp
public Task<Unit> SaveChangesAsync()
{
    return Task.FromResult(Unit.value);
}
```

### **Unit Extensions**
```csharp
public static async Task<Unit> AsUnitTask(this Task task)
{
    await task;
    return Unit.value;
}
```

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

