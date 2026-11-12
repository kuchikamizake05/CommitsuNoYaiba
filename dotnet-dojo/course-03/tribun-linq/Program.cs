using System.Linq;

var checks = 0;
void Check<T>(string label, T actual, T expected)
{
    if (!EqualityComparer<T>.Default.Equals(actual, expected))
        throw new InvalidOperationException($"{label}: expected {expected}, got {actual}");
    checks++;
    Console.WriteLine($"PASS {label}: {actual}");
}
void Throws<T>(string label, Action action) where T : Exception
{
    try { action(); }
    catch (T) { checks++; Console.WriteLine($"PASS {label}: {typeof(T).Name}"); return; }
    throw new InvalidOperationException($"{label}: expected {typeof(T).Name}");
}

var klub = new[]
{
    new Klub(1, "Persib", "Bandung", 8),
    new Klub(2, "Persija", "Jakarta", 5),
    new Klub(3, "Persebaya", "Surabaya", 2),
    new Klub(4, "Klub Latihan", "Bandung", 0)
};

// Query syntax dan method syntax memilih data yang sama.
var method = klub.Where(k => k.Poin >= 5).OrderByDescending(k => k.Poin).Select(k => k.Nama);
var query = from k in klub where k.Poin >= 5 orderby k.Poin descending select k.Nama;
Check("filter/sort/select", string.Join(", ", method), "Persib, Persija");
Check("query syntax", query.SequenceEqual(method), true);
Check("ascending", string.Join(",", klub.OrderBy(k => k.Poin).Select(k => k.Poin)), "0,2,5,8");
Check("Any", klub.Any(k => k.Poin >= 5), true);
Check("All", klub.All(k => k.Poin >= 5), false);
Check("Contains", klub.Select(k => k.Nama).Contains("Persija"), true);
Check("Count", klub.Count(k => k.Poin >= 5), 2);
Check("Sum", klub.Sum(k => k.Poin), 15);
Check("Min", klub.Min(k => k.Poin), 0);
Check("Max", klub.Max(k => k.Poin), 8);
Check("First", klub.First().Nama, "Persib");
Check("Last", klub.Last().Nama, "Klub Latihan");
Check("FirstOrDefault object", klub.FirstOrDefault(k => k.Poin > 10) is null, true);
Check("LastOrDefault object", klub.LastOrDefault(k => k.Poin > 10) is null, true);
Check("FirstOrDefault int", new[] { 8, 5, 2 }.FirstOrDefault(p => p > 10), 0);
Throws<InvalidOperationException>("First no match", () => { _ = klub.First(k => k.Poin > 10); });
Throws<InvalidOperationException>("Last empty", () => { _ = Array.Empty<Klub>().Last(); });
Check("Reverse is not sorting", string.Join(",", new[] { 5, 8, 2 }.Reverse()), "2,8,5");
Check("Skip/Take", string.Join(",", new[] { 10, 8, 6, 4, 2 }.Skip(2).Take(2)), "6,4");
Check("TakeWhile stops", string.Join(",", new[] { 8, 5, 2, 10 }.TakeWhile(p => p >= 5)), "8,5");
Check("SkipWhile stops testing", string.Join(",", new[] { 8, 5, 2, 10 }.SkipWhile(p => p >= 5)), "2,10");
var ringkas = klub.Select(k => new { NamaKlub = k.Nama, TotalPoin = k.Poin }).ToArray();
Check("projection/ToArray", ringkas[0].TotalPoin, 8);
var kamus = klub.ToDictionary(k => k.Nama, k => k.Poin);
Check("ToDictionary", kamus["Persib"], 8);
Throws<ArgumentException>("duplicate dictionary key", () => { _ = new[] { klub[0], klub[0] }.ToDictionary(k => k.Nama); });
Check("GroupBy", string.Join(",", klub.GroupBy(k => k.Kota).Single(g => g.Key == "Bandung").Select(k => k.Nama)), "Persib,Klub Latihan");
var poinTerpisah = new[] { new { KlubId = 2, Poin = 5 }, new { KlubId = 1, Poin = 8 } };
var gabungan = klub.Join(poinTerpisah, k => k.Id, p => p.KlubId, (k, p) => new { k.Nama, p.Poin });
Check("Join uses ID", gabungan.Single(k => k.Nama == "Persib").Poin, 8);

// Where ditunda; ToList dan Sum langsung membaca data saat dipanggil.
var poin = new List<int> { 8, 5, 2 };
var ditunda = poin.Where(p => p >= 5);
var snapshot = ditunda.ToList();
var totalAwal = poin.Sum();
poin.Add(10);
Check("deferred Where", string.Join(",", ditunda), "8,5,10");
Check("materialized ToList", string.Join(",", snapshot), "8,5");
Check("immediate Sum", totalAwal, 15);
Console.WriteLine($"All {checks} checks passed. Data klub adalah data latihan.");

record Klub(int Id, string Nama, string Kota, int Poin);
