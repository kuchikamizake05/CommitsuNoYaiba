using System.Collections;

static class DataTypesDemo
{
    public static void ShowPartOne()
    {
        string nama = "Persib";
        int poin = 7;
        bool pertandinganSelesai = true;
        decimal hargaTiket = 75000.50m;
        int? jumlahPenonton = null;

        if (int.TryParse("25000", out int penonton))
        {
            jumlahPenonton = penonton;
        }

        double rataRata = (double)poin / 2;
        Console.WriteLine($"{nama}: {poin} poin, rata-rata {rataRata}");
        Console.WriteLine($"Selesai: {pertandinganSelesai}, harga: {hargaTiket}, penonton: {jumlahPenonton}");
    }

    public static void ShowPartTwo()
    {
        string[] klubTetap = { "Persib", "Persija" };
        List<string> daftarKlub = new(klubTetap);
        daftarKlub.Add("Persebaya");

        Dictionary<string, int> poinKlub = new()
        {
            ["Persib"] = 3,
            ["Persija"] = 1
        };

        Console.WriteLine($"Jumlah klub: {daftarKlub.Count}");
        if (poinKlub.TryGetValue("Persebaya", out int poin))
            Console.WriteLine($"Poin Persebaya: {poin}");
        else
            Console.WriteLine("Persebaya belum terdaftar di klasemen");

        Hashtable contohHashtable = new() { ["Persib"] = 3 };
        Console.WriteLine($"Contoh Hashtable: {contohHashtable["Persib"]}");
    }
}
