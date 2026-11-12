using Microsoft.EntityFrameworkCore;

var mode = Environment.GetEnvironmentVariable("TRIBUN_PROVIDER") ?? "sqlite";
var options = new DbContextOptionsBuilder<LigaContext>();
if (mode == "sqlserver")
{
    var connection = Environment.GetEnvironmentVariable("TRIBUN_CONNECTION")
        ?? throw new InvalidOperationException("Isi TRIBUN_CONNECTION untuk database latihan SQL Server.");
    options.UseSqlServer(connection);
}
else if (mode == "sqlite")
{
    options.UseSqlite("Data Source=:memory:;Foreign Keys=True");
}
else throw new InvalidOperationException("TRIBUN_PROVIDER harus sqlite atau sqlserver.");

await using var context = new LigaContext(options.Options);
if (mode == "sqlserver")
{
    // Mode jaringan hanya membaca database yang sudah disiapkan; tidak membuat schema.
    var repo = new KlubRepository(context);
    foreach (var klub in await repo.AmbilSemuaKlub())
        Console.WriteLine($"{klub.Nama}: {klub.Poin}");
    return;
}

// Database relasional sementara untuk latihan; tidak mengubah database pengguna.
await context.Database.OpenConnectionAsync();
await context.Database.EnsureCreatedAsync();
var repository = new KlubRepository(context);
context.Klub.AddRange(new Klub { Id = 7, Nama = "Persib", Poin = 8 },
    new Klub { Id = 12, Nama = "Persija", Poin = 5 });
await repository.Simpan();
context.Pemain.Add(new Pemain { Nama = "Andi", KlubId = 12 });
await repository.Simpan();
var checks = 0;
void Check(string label, bool condition)
{
    if (!condition) throw new InvalidOperationException(label);
    checks++;
    Console.WriteLine($"PASS {label}");
}
Check("repository membaca dua klub", (await repository.AmbilSemuaKlub()).Count == 2);
var pemain = await context.Pemain.Include(p => p.Klub).SingleAsync();
Check("foreign key cocok ke ID, bukan indeks", pemain.Klub.Nama == "Persija");
var persib = await context.Klub.SingleAsync(k => k.Id == 7);
persib.Poin = 10;
await using (var reader = new LigaContext(options.Options))
{
    reader.Database.SetDbConnection(context.Database.GetDbConnection(), contextOwnsConnection: false);
    Check("sebelum SaveChanges database masih 8", await reader.Klub.Where(k => k.Id == 7).Select(k => k.Poin).SingleAsync() == 8);
}
await repository.Simpan();
Check("setelah SaveChanges database menjadi 10", await context.Klub.AsNoTracking().Where(k => k.Id == 7).Select(k => k.Poin).SingleAsync() == 10);
var query = context.Klub.Where(k => k.Poin >= 5).OrderByDescending(k => k.Poin);
Console.WriteLine(query.ToQueryString());
Check("filter sebelum materialisasi", (await query.ToListAsync()).Count == 2);
var salah = new Pemain { Nama = "Budi", KlubId = 99 };
context.Pemain.Add(salah);
try { await repository.Simpan(); throw new InvalidOperationException("Foreign key invalid diterima"); }
catch (DbUpdateException) { context.Entry(salah).State = EntityState.Detached; checks++; Console.WriteLine("PASS foreign key invalid ditolak"); }
Console.WriteLine($"All {checks} checks passed.");

public class Klub
{
    public int Id { get; set; }
    public string Nama { get; set; } = "";
    public int Poin { get; set; }
}
public class Pemain
{
    public int Id { get; set; }
    public string Nama { get; set; } = "";
    public int KlubId { get; set; }
    public Klub Klub { get; set; } = null!;
}
public class LigaContext(DbContextOptions<LigaContext> options) : DbContext(options)
{
    public DbSet<Klub> Klub => Set<Klub>();
    public DbSet<Pemain> Pemain => Set<Pemain>();
    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Klub>().ToTable("Klub");
        model.Entity<Pemain>().ToTable("Pemain");
        model.Entity<Klub>().Property(k => k.Nama).HasMaxLength(100).IsRequired();
        model.Entity<Pemain>().HasOne(p => p.Klub).WithMany().HasForeignKey(p => p.KlubId).OnDelete(DeleteBehavior.Restrict);
    }
}
public class KlubRepository(LigaContext context)
{
    public Task<List<Klub>> AmbilSemuaKlub() => context.Klub.AsNoTracking().OrderBy(k => k.Id).ToListAsync();
    public Task<int> Simpan() => context.SaveChangesAsync();
}
