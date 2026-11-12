# Tribun Data — Entity Framework

Course: https://rocketversity.com/course/view.php?id=68
Sumber: Pro ASP.NET MVC 5, halaman PDF 247–249: context, connection string,
pemetaan class ke database yang ada, dan repository.

Latihan memakai EF Core 8/.NET 8, sedangkan buku memakai EF klasik/MVC 5.
Ini adaptasi konsep; bukan implementasi EF Designer/Model First.

## Menjalankan

```powershell
dotnet run --project dotnet-dojo/course-03/tribun-data/TribunData.csproj
```

Default memakai SQLite relasional in-memory. Schema dibuat melalui EnsureCreated
hanya untuk latihan sementara, bukan migrations. Program memeriksa repository,
relasi ID, SaveChanges, filter di database, serta penolakan foreign key invalid.
SQL terjemahan query ditampilkan tanpa menyertakan connection string.

## SQL Server yang sudah tersedia

Set TRIBUN_PROVIDER=sqlserver dan TRIBUN_CONNECTION melalui environment lokal.
Jangan commit credential. Mode ini hanya membaca tabel Klub dari database yang
sudah disiapkan; tidak otomatis membuat schema atau mengisi data.
SQLite bukan bukti pengujian SQL Server. Model C# tetap menjadi konfigurasi
pemetaan: memakai database yang ada tidak otomatis berarti Database First.

## Tiga pendekatan

- Code First: class/configuration C# menjadi acuan model; perubahan schema dapat
  dikelola melalui migration yang dibuat dan diterapkan secara terpisah.
- Database First: schema yang ada menjadi acuan untuk menghasilkan model.
- Model First: diagram di EF Designer klasik menjadi acuan pembentukan database.

SaveChanges menyimpan perubahan entity; tidak menambahkan kolom baru pada schema.
Repository menyediakan akses data; tidak merender HTML.

Keterbatasan: tidak ada SQL Server lokal yang terverifikasi, tidak ada scaffold
Database First, designer Model First, atau migration yang dieksekusi pada latihan ini.
