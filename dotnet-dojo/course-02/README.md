# Course 02 — Git & Version Control

Course: https://rocketversity.com/course/view.php?id=67

Latihan memakai data klub fiktif, bukan hasil pertandingan nyata.

## Lima materi

- Git: riwayat perubahan tersimpan lokal; GitLab menyimpan repository remote.
- Git Workflow: ubah file, staging, commit, push, review MR, merge.
- Basic GIT: status, diff, add, commit, log; diff --staged membaca staging.
- GIT Branch: commit mengikuti branch aktif; konflik perlu keputusan manusia.
- GIT Remote: fetch mengambil pembaruan; pull mengambil dan mengintegrasikan; push mengirim commit.

## Praktik

Branch course/02-git memakai main sebagai dasar. klub.json dan README ini
sengaja dicatat dalam commit terpisah untuk menunjukkan selective staging.
MR dibuat di GitLab, kemudian commit lanjutan dikirim ke branch yang sama.
MR tetap terbuka untuk review sebelum merge ke main.

Konflik disimulasikan pada pilihan-klub.txt: dua branch mengubah baris sama
menjadi Persib dan Persija. Hasil akhirnya dipilih Persib berdasarkan skenario.

File dummy config.local.json sempat dilacak, kemudian dimasukkan ke .gitignore.
Git masih melacaknya sampai git rm --cached dijalankan. File tetap ada lokal;
versi lama tetap ada dalam riwayat. Tidak ada kredensial di file dummy.

## Memeriksa sendiri

```powershell
git status
git log --oneline --graph -15
git diff gitlab/main...HEAD -- dotnet-dojo/course-02
Get-Content dotnet-dojo/course-02/klub.json | ConvertFrom-Json
```

Pekerjaan implementasi dijalankan dengan bantuan agent; pemahaman konsep
dilatih melalui tanya jawab. Checklist Rocketversity perlu ditandai sendiri
setelah sumber materinya benar-benar dipelajari.

## Hasil praktik 11 November 2026

- Commit fixture dan README dipisahkan melalui selective staging.
- Konflik nyata muncul saat merge practice/02-persija, lalu diselesaikan dengan memilih Persib.
- config.local.json masih muncul sebagai modified setelah ditambahkan ke .gitignore.
- Setelah git rm --cached, file tetap ada di disk dan git check-ignore mengenalinya.
- MR GitLab !1 dibuka, lalu Persebaya ditambahkan lewat commit lanjutan pada branch yang sama.
- GitHub menerima branch yang sama untuk sinkronisasi kode; review dilakukan di GitLab.

MR: https://gitlab.com/kuchikamizake05/CommitsuNoYaiba/-/merge_requests/1

## Praktik tambahan

Lihat [hasil praktik tambahan](praktik-tambahan.md) dan [riwayat repo dummy](lab-history.txt).
Operasi yang menulis ulang riwayat dicoba di repo dummy, sehingga commit yang
sudah dipublikasikan pada MR ini tetap utuh.
