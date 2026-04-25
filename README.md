# Yoyoyopo5.ValueWrapper

![NuGet Version](https://img.shields.io/nuget/v/Yoyoyopo5.ValueWrapper)
![Build Status](https://github.com/Yoyoyopo5/ValueWrapper/actions/workflows/ci.yml/badge.svg)
![License](https://img.shields.io/github/license/Yoyoyopo5/ValueWrapper)
![Target](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-blue)
---

Eliminate primitive obsession in your codebase without unnecessary heap allocations or boilerplate. ValueWrapper makes it easy.

## Usage

### Create a Wrapper Type

```cs
[Wrapper<string>]
public readonly partial record struct ColorName; 
```

Generally, wrappers should be `record struct` types, but a wrapper can be any partial type and can wrap any type:

```cs
public record Person(string Name, uint Age);

[Wrapper<Person>]
public partial class InactivePerson;
```

### Wrapper Features

#### Value Property

A required `Value` property is created by default:

```cs
ColorName color = new() { Value = "red" };
string name = color.Value;
```

#### Explicit In, Implicit Out

Wrapper types can be freely used as their wrapped type (but not the other way around).

```cs
ColorName color = new() { Value = "orange" };
string.IsNullOrEmpty(color);

ColorName badColor = "orange" // Does not compile!
```

#### ToString() Passthrough

```cs
ColorName color = new() { Value = "red" };
Console.WriteLine(color); // "red"
```

#### System.Text.Json Passthrough

```cs
ColorName color = new() { Value = "green" };
string json = JsonSerializer.Serialize(color);
Console.WriteLine(json); // "green"

ColorName deserializedColor = JsonSerializer.Deserialize<ColorName>(json);
Console.WriteLine(deserializedColor); // "green"
```

#### TypeConverter Support

A `TypeConverter` is added to the wrapper that can create the wrapper to and from its wrapped type. Conversion from `string` is also supported via `TypeConverter` passthrough.

This allows wrappers to be used as their wrapped type in many scenarios, like `IConfigurationBinder` binding or ASP.NET Core query parameter binding.

#### Nested Wrappers

Wrappers inside other classes are supported, as long as each containing class is marked `partial`:

```cs
public partial class Container
{
    [Wrapper<int>]
    private readonly partial record struct Cents;
}
```

### Wrapper Customization

#### Record Primary Constructor

A record primary constructor can define the `Value` property, meaning the required property will not be automatically generated:

```cs
[Wrapper<Guid>]
public partial record UserId(Guid Value);
```

#### Disable Required Value Property

You can also disable the required property manually:

```cs
[Wrapper<double>]
public readonly partial record struct FeetPerSecond
{
    public double Value { get; init; }
}
```

#### Override Wrapper Features

You can override any generated wrapper behavior:

```cs
[Wrapper<string>]
[JsonConverter(typeof(IdeaConverter))] // Disables the default WrapperJsonConverter
public partial class Idea(string Value)
{
    public string? Value { get; set; } // Overrides the default Value property
    public override string ToString() => "Nobody can know!" // Overrides the ToString passthrough
    public static implicit operator string(Idea idea) => "Hey, that's my idea!" // Overrides the implicit out operator
}
```

#### Override JSON Creation Behavior

Think you need a custom JsonConverter? Maybe not! The converter will use a constructor with a single wrapped type parameter:

```cs
[Wrapper<int>]
public partial record NegativeNumber 
{
    public int Value { get; }

    public NegativeNumber(int value)
    {
        if (value > 0)
            throw new ArgumentException("Number must be negative!");
        Value = value;
    }
}
```

Don't like constructors? No problem. You can configure the static `Create(value)` method, and the default `WrapperJsonConverter` will use it:

```cs
[Wrapper<int>]
public readonly partial record struct NegativeNumber
{
    public int Value { get; private init; }

    public static NegativeNumber Create(int value)
        => value switch 
        {
            _ when value > 0 => throw new ArgumentException("Number must be negative!"),
            _ => new() { Value = value }
        };
}
```

## Use Cases

#### Reduce Bugs with Constrained Signatures

Turn runtime exceptions into compile-time errors:

Before:
```cs
void UpdateColorBuggy(string colorName) { }
string username = "xX_username_Xx";

...

UpdateColorBuggy(username); // Compiles with bug!
```

After:
```cs
// I can only take a ColorName type.
void UpdateColor(ColorName color) { }
Username username = new("xX_username_Xx");

...

UpdateColor(username); // Compiler error! 
```

### Create Enumerations

Have a set of possible values? Don't use an `enum`, use a wrapper with static definitions:

```cs
[Wrapper<string>]
public readonly partial record struct ReportStatus
{
    public static ReportStatus Accepted { get; } = new("accepted");
    public static ReportStatus Rejected { get; } = new("rejected");
    public static ReportStatus Unknown { get; } = new("unknown");

    public string Value { get; }
    private ReportStatus(string value) => Value = value;

    internal static ReportStatus Create(string value) => new(value);
}
```

You can also create unbounded value sets:

```cs
[Wrapper<string>]
public readonly partial record struct ColorName(string Value)
{
    public static ColorName SkyBlue { get; } = new("sky_blue");
    public static ColorName EggplantPurple { get; } = new("eggplant_purple");
}

ColorName _customColor = new("very_red");
```

### Add Validation

Wrapper types can be used to put validation at the type level, instead of littered around your codebase:

```cs
[Wrapper<int>]
public readonly partial record struct EvenNumber
{
    public int Value { get; private init; }

    public static EvenNumber? ParseOrNull(int value)
        => value switch 
        {
            _ when value % 2 != 0 => null,
            _ => new() { Value = value }
        };

    internal static EvenNumber Create(int value)
        => ParseOrNull(value) is EvenNumber even ? even : throw new ArgumentException("value must be even.");
} 
```

### Create Bounded Extensions

Don't extend primitives; extend wrappers instead:

```cs
public static BadExtensions
{
    public static decimal ConvertToMiles(this decimal kilometers) { }
}

[Wrapper<decimal>]
public partial record Kilometers;

[Wrapper<decimal>]
public partial record Miles;

public static KilometersExtensions
{
    public static Miles ConvertToMiles(this Kilometers kilometers) { }
}
```

## Benchmarks

### ToString Passthrough

#### Wrapped `struct`

| Method            | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| WrapperToString   | 47.31 ns | 0.730 ns | 0.683 ns |  1.04 |    0.02 | 0.0172 |     288 B |        1.00 |
| PrimitiveToString | 45.67 ns | 0.694 ns | 0.615 ns |  1.00 |    0.02 | 0.0172 |     288 B |        1.00 |

#### Wrapped `double`

| Method            | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| WrapperToString   | 39.27 ns | 0.647 ns | 0.605 ns |  1.00 |    0.02 | 0.0014 |      24 B |        1.00 |
| PrimitiveToString | 39.19 ns | 0.509 ns | 0.425 ns |  1.00 |    0.01 | 0.0014 |      24 B |        1.00 |

#### Wrapped `string`

| Method            | Mean      | Error     | StdDev    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------ |----------:|----------:|----------:|------:|--------:|----------:|------------:|
| WrapperToString   | 0.1560 ns | 0.0190 ns | 0.0177 ns |  0.32 |    0.04 |         - |          NA |
| PrimitiveToString | 0.4907 ns | 0.0186 ns | 0.0165 ns |  1.00 |    0.05 |         - |          NA |

### System.Text.Json Round-Trip

Non-built-in wrapped types incur a penalty.

#### Wrapped `struct`

| Method                 | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
| WrapperRoundTripJson   | 410.1 ns | 5.10 ns | 4.52 ns |  1.82 |    0.06 | 0.0057 |      96 B |        1.00 |
| PrimitiveRoundTripJson | 225.4 ns | 4.00 ns | 7.01 ns |  1.00 |    0.04 | 0.0057 |      96 B |        1.00 |

#### Wrapped `double`

| Method                 | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
| WrapperRoundTripJson   | 160.5 ns | 2.49 ns | 2.33 ns |  1.04 |    0.02 | 0.0019 |      32 B |        1.00 |
| PrimitiveRoundTripJson | 154.3 ns | 1.26 ns | 1.18 ns |  1.00 |    0.01 | 0.0019 |      32 B |        1.00 |

#### Wrapped `string`

| Method                 | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------------- |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
| WrapperRoundTripJson   | 136.1 ns | 2.08 ns | 1.95 ns |  1.00 |    0.02 | 0.0043 |      72 B |        1.00 |
| PrimitiveRoundTripJson | 136.3 ns | 1.49 ns | 1.32 ns |  1.00 |    0.01 | 0.0043 |      72 B |        1.00 |

### TypeConverter Round-Trip (to/from `string`)

#### Wrapped `struct`

| Method                   | Mean     | Error   | StdDev  | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------------- |---------:|--------:|--------:|------:|-------:|----------:|------------:|
| WrapperRoundTripString   | 248.5 ns | 3.40 ns | 3.01 ns |  1.05 | 0.0086 |     144 B |        1.20 |
| PrimitiveRoundTripString | 235.9 ns | 1.84 ns | 1.54 ns |  1.00 | 0.0072 |     120 B |        1.00 |

#### Wrapped `double`

| Method                   | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| WrapperRoundTripString   | 76.56 ns | 1.262 ns | 1.054 ns |  1.03 |    0.02 | 0.0057 |      96 B |        1.33 |
| PrimitiveRoundTripString | 74.03 ns | 0.826 ns | 0.773 ns |  1.00 |    0.01 | 0.0043 |      72 B |        1.00 |

#### Wrapped `string`

| Method                   | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| WrapperRoundTripString   | 14.56 ns | 0.169 ns | 0.149 ns |  1.27 |    0.02 | 0.0029 |      48 B |        2.00 |
| PrimitiveRoundTripString | 11.43 ns | 0.175 ns | 0.155 ns |  1.00 |    0.02 | 0.0014 |      24 B |        1.00 |

### Source Generator

| Method             | Mean     | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|------------------- |---------:|---------:|---------:|-------:|-------:|----------:|
| RunGenerator       | 76.26 us | 0.734 us | 0.613 us | 4.8828 | 0.4883 |  81.71 KB |
| RunGeneratorCached | 29.58 us | 0.115 us | 0.090 us | 1.3123 | 0.0305 |  21.85 KB |