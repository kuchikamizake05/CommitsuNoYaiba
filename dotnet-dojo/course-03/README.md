# Course 03 — SQL Server & .NET Integration

Course: https://rocketversity.com/course/view.php?id=68

## LINQ: Tribun LINQ

Contoh mandiri .NET 8 dengan data klub sintetis. Setiap hasil diperiksa dengan
nilai harapan; exception yang diharapkan juga diperiksa. Program keluar gagal
jika hasil tidak sesuai. Tidak memakai database atau dependency tambahan.

```powershell
dotnet run --project dotnet-dojo/course-03/tribun-linq/TribunLinq.csproj
```

Cakupan: Where, OrderBy/Descending, Select (nilai dan objek baru), query syntax,
Any/All/Contains, Count/Sum/Min/Max, First/Last dan OrDefault, Reverse,
Skip/Take/SkipWhile/TakeWhile, ToArray/ToList/ToDictionary, key duplikat,
GroupBy, Join, serta deferred dan immediate execution.

Rujukan course: subbab LINQ menunjuk Pro ASP.NET MVC 5 (Adam Freeman), halaman
cetak 95; bagian LINQ pada salinan PDF dibaca sekitar halaman PDF 134–140.
Contoh ditulis ulang dengan data klub, bukan menyalin kode buku.

Materi buku memakai MVC 5/.NET Framework; latihan ini mengisolasi konsep LINQ
pada .NET 8. GroupBy dan Join adalah tambahan pembahasan. Query saat ini LINQ
to Objects (data memori), bukan bukti query SQL atau integrasi Entity Framework.

## Status

- LINQ: pembahasan dan contoh executable tersedia.
- Entity Framework: konsep tiga pendekatan dibahas; latihan context/repository tersedia di tribun-data.
- Koneksi SQL Server: konfigurasi provider tersedia, belum diuji ke server nyata.

Satu MR dipakai untuk course ini; tahap berikutnya ditambahkan pada branch yang
sama. Checklist dan durasi Basecamp mengikuti aktivitas yang benar-benar selesai.

## Verifikasi lingkungan

Build pada 12 November 2026 berhasil menghasilkan assembly. Eksekusi diblokir
Windows Application Control (4551 pada apphost, 0x800711C7 pada assembly).
Program berisi 29 pengecekan, tetapi hasil runtime belum terverifikasi.
Tidak ada pengaturan keamanan Windows yang diubah.

## Update verifikasi 12 November 2026

[Tribun Data](tribun-data/README.md) menambahkan entity, context, connection
configuration, repository, dan relasi. Build 0 warning/error; enam pengecekan
runtime pada SQLite in-memory lolos. SQL Server belum diuji ke server nyata.
Sumber EF dibaca pada halaman PDF 247–249, tanpa menggeser nomor dengan offset.
