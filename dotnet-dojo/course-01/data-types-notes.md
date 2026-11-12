# C# data types: football examples

Course: [Part 1](https://rocketversity.com/mod/url/view.php?id=867) · [Part 2](https://rocketversity.com/mod/url/view.php?id=868)

See [DataTypesDemo.cs](klasemen-bola/DataTypesDemo.cs) for independently created examples, written with AI assistance and discussed together.

## Part 1

- `string`, `int`, `bool`, and `decimal` represent text, integers, true/false values, and decimal numbers. The `m` suffix marks a decimal literal.
- `int?` can hold an integer or `null`, which represents a missing value rather than zero.
- `int.TryParse` reports whether text can be converted to an integer. Casting an operand to `double` before division keeps the fractional result; casting the integer-division result afterwards does not recover it.

## Part 2

- An array has a fixed length; a `List<T>` can grow or shrink.
- A `Dictionary<TKey, TValue>` stores typed key–value pairs. `TryGetValue` handles a missing key without an exception.
- `Hashtable` is a non-generic key–value collection. A typed `Dictionary` is usually preferable for new code when the key and value types are known.
