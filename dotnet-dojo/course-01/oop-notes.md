# OOP practice: Indonesian football clubs

Course: [C# Introduction: Object-Oriented Programming](https://rocketversity.com/mod/url/view.php?id=866)

This exercise was written with AI assistance and reviewed through discussion. It uses fictional people and simplified rules.

- `FootballClub` is a class; `persib` and `persija` are separate objects with their own names and points.
- The constructor initializes each club's name and starts its points at zero.
- `Points` has a public getter and a private setter. `AddPoints` can update it from within the class. The example does not yet validate negative values.
- `Player` and `Coach` inherit `Name` from `Person`; their constructors use `base(name)` to initialize it.
- `Introduce` is virtual in `Person` and overridden by both subclasses. A `Person` reference to a `Player` calls the player's version.
- `AddPoints(int)` and `AddPoints(int, string)` overload the same method name. The second calls the first and prints a note; it does not store match history.

Expected output for the current example:

```text
Persija: Hasil imbang
Persib: 3 poin
Persija: 1 poin
Saya Andi, pemain nomor 10.
Saya Budi, pelatih berlisensi A.
```

The project builds on .NET 8. Runtime execution still needs verification on this machine because Windows application control blocked a recent attempt.
