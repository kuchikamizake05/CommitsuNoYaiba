FootballClub persib = new FootballClub("Persib");
FootballClub persija = new FootballClub("Persija");

persib.AddPoints(3);
persija.AddPoints(1, "Hasil imbang");
persib.ShowPoints();
persija.ShowPoints();

Person pemain = new Player("Andi", 10);
Person pelatih = new Coach("Budi", "A");

pemain.Introduce();
pelatih.Introduce();

DataTypesDemo.ShowPartOne();
DataTypesDemo.ShowPartTwo();

class FootballClub
{
    public string Name { get; }
    public int Points { get; private set; }

    public FootballClub(string name)
    {
        Name = name;
        Points = 0;
    }

    public void AddPoints(int points)
    {
        Points += points;
    }

    public void AddPoints(int points, string note)
    {
        AddPoints(points);
        Console.WriteLine($"{Name}: {note}");
    }

    public void ShowPoints()
    {
        Console.WriteLine($"{Name}: {Points} poin");
    }
}

class Person
{
    public string Name { get; }

    public Person(string name)
    {
        Name = name;
    }

    public virtual void Introduce()
    {
        Console.WriteLine($"Saya {Name}.");
    }
}

class Player : Person
{
    public int ShirtNumber { get; }

    public Player(string name, int shirtNumber) : base(name)
    {
        ShirtNumber = shirtNumber;
    }

    public override void Introduce()
    {
        Console.WriteLine($"Saya {Name}, pemain nomor {ShirtNumber}.");
    }
}

class Coach : Person
{
    public string License { get; }

    public Coach(string name, string license) : base(name)
    {
        License = license;
    }

    public override void Introduce()
    {
        Console.WriteLine($"Saya {Name}, pelatih berlisensi {License}.");
    }
}
