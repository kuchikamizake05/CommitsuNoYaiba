# Praktik tambahan Git — 11 November 2026

Praktik dijalankan agent di repository dummy terpisah. Reset, amend, dan rebase
tidak diterapkan pada riwayat Commitsu yang sudah di-push. Ini bukti eksekusi
latihan, bukan klaim telah menonton semua video atau menguasai semua materi.

| Operasi | Hasil yang diverifikasi |
| --- | --- |
| restore --staged | Edit Persib 99 tetap ada; staging kembali kosong |
| restore | Isi file kembali menjadi Persib 3 |
| stash push/pop | Edit Persib 4 disimpan sementara lalu berhasil dikembalikan |
| commit --amend | Pesan diganti dan hash commit berubah |
| reset --soft HEAD~1 | Perubahan tetap di staging |
| reset --mixed HEAD~1 | Staging kosong; perubahan file tetap ada |
| revert | Commit pembalik baru mengembalikan poin menjadi 3 |
| cherry-pick | Commit penambahan Persebaya diterapkan ke branch main dummy |
| tag -a | Penanda lokal lab-course-02 dibuat pada commit tertentu |
| mv | klub.txt berganti menjadi daftar-klub.txt |
| show HASH:klub.txt | Isi file pada commit awal terbaca tanpa mengubah file kerja |
| blame | Commit terakhir pengubah baris berhasil ditelusuri |
| config --local alias.st status | Alias st berjalan hanya pada repository dummy |
| rebase main | Commit branch pribadi diterapkan ulang; hash berubah |
| merge --squash | Perubahan fitur disimpan dalam satu commit di main dummy |
| clone | Repository dummy disalin dari remote bare lokal |
| fetch | Remote-tracking branch diperbarui tanpa memunculkan file baru di working tree |
| pull --ff-only | File dari commit remote masuk melalui fast-forward |

Remote latihan berupa bare repository di disk lokal; ini menguji mekanisme Git,
bukan autentikasi jaringan. Push jaringan GitLab dan GitHub sudah dilakukan
pada branch course/02-git. Tag latihan tidak dipublikasikan sebagai penanda
course selesai. reset --hard tidak diperlukan untuk latihan ini.

## Batas cakupan

SSH, fork, submodule, dan strategi branching dibahas secara konseptual.
Sumber Git Workflow di Teamwork belum terbaca karena memerlukan login;
sumber Dropbox belum dibaca. Daftar bab video dipakai memetakan kekurangan,
bukan sebagai bukti menonton seluruh isi video.

## Perintah untuk mengulang pada repository dummy

Gunakan file dummy, kemudian periksa git status dan git diff di setiap langkah.
Jangan menjalankan reset atau rebase pada branch bersama untuk meniru latihan.

```bash
git restore --staged klub.txt
git restore klub.txt
git stash push -m "latihan sementara"
git stash pop
git commit --amend -m "pesan diperbaiki"
git reset --soft HEAD~1
git reset --mixed HEAD~1
git revert --no-edit HEAD
git cherry-pick HASH
git tag -a lab-course-02 -m "milestone lokal"
git mv klub.txt daftar-klub.txt
git show HASH:klub.txt
git blame daftar-klub.txt
git config --local alias.st status
git rebase main
git merge --squash NAMA_BRANCH
git fetch latihan
git pull --ff-only latihan main
```
