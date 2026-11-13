# Course 04 — Tribun Auth

Course reference: https://rocketversity.com/course/view.php?id=70

Latihan mandiri .NET 8 untuk alur login, API, JWT, dan role. Nama akun dan data klub sintetis. Contoh dibuat dengan bantuan AI untuk dibahas bersama; adanya kode dan test bukan bukti seluruh materi course sudah dipelajari.

## Alur yang dibaca

1. `POST /login`: JSON `username` dan `password` → validasi input → cocokkan hash melalui `PasswordHasher<DemoUser>` dari ASP.NET Core Identity → JWT berlaku 15 menit.
2. `GET /profil`: kirim `Authorization: Bearer <token>` → verifikasi signature, issuer, audience, dan expiration → baca claims `sub` dan `role`.
3. `POST /admin/pertandingan`: token valid masih harus punya role `Admin`. Hanya mengembalikan pesan latihan, tidak menyimpan pertandingan.

401 berarti tidak terautentikasi; 403 berarti terautentikasi tetapi tidak berizin. Role berasal dari akun di server, bukan dari body login. JWT ditandatangani, bukan dienkripsi; password tidak dimasukkan dalam JWT. Middleware authentication dipasang sebelum authorization.

## Jalankan dari Ubuntu WSL

Di terminal Ubuntu, masuk folder repo, lalu set konfigurasi sementara sesi shell. Jangan commit nilai asli atau membagikan token.

```bash
export Jwt__Key="$(openssl rand -base64 48)"
read -rsp 'Password demo pemain (minimal 12 karakter): ' Demo__PlayerPassword; echo
export Demo__PlayerPassword
read -rsp 'Password demo admin (minimal 12 karakter): ' Demo__AdminPassword; echo
export Demo__AdminPassword
dotnet run --project dotnet-dojo/course-04/tribun-auth --urls http://127.0.0.1:5190
```

Akun `pemain` ber-role `Player`; akun `admin` ber-role `Admin`. Isi password sesuai konfigurasi sesi tadi. HTTP hanya untuk localhost pada latihan; komunikasi login/token yang dipublikasikan wajib dilindungi HTTPS.

```bash
dotnet test dotnet-dojo/course-04/tribun-auth.tests --collect:"XPlat Code Coverage"
```

Test membuat signing key dan password acak sendiri, tidak memerlukan secret pribadi.

## Batas latihan

Menggunakan komponen hash password Identity, **bukan implementasi lengkap ASP.NET Core Identity** dengan UserManager/RoleManager, database, registrasi, reset password, lockout, atau refresh token. Akun hanya ada di memori. Login lokal yang menerbitkan JWT ini bukan implementasi OAuth/OIDC dan tidak untuk deployment produksi; aplikasi nyata sebaiknya memakai penyedia identitas yang sesuai. Tidak ada pembatasan percobaan login pada latihan lokal ini.

Materi course: intro → project/API → JWT → RBAC. Checklist platform baru ditandai setelah materi dan contoh benar-benar dipahami. Latihan memakai ASP.NET Core 8; jika sumber memakai MVC 5/.NET Framework, konsep dipelajari bersama tanpa menyamakan API framework-nya.

Rujukan resmi: [JWT bearer validation](https://learn.microsoft.com/aspnet/core/security/authentication/configure-jwt-bearer-authentication), [role authorization](https://learn.microsoft.com/aspnet/core/security/authorization/roles).

## Verifikasi 13 November 2026

Ubuntu 24.04 WSL, .NET SDK 8.0.134: 13/13 HTTP integration tests lolos. Line coverage 97.22% (70/72), branch coverage 81.81%. Kasus: input kosong, password salah/akun tidak dikenal, tanpa token, profil pemain, penolakan role pemain, izin admin, signature diubah, issuer/audience salah, dan token kedaluwarsa. Audit NuGet termasuk dependency transitif tidak menemukan paket rentan pada sumber saat pemeriksaan. Pengujian memakai in-process TestServer, bukan browser atau deployment jaringan. Tidak ada pengaturan keamanan Windows yang diubah.
