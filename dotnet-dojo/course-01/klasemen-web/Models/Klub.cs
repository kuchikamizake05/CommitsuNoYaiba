using System.ComponentModel.DataAnnotations;

namespace KlasemenWeb.Models;

public class Klub
{
    [Required(ErrorMessage = "Nama klub wajib diisi.")]
    public string Nama { get; set; } = "";

    [Range(0, 100, ErrorMessage = "Poin harus antara 0 dan 100.")]
    public int Poin { get; set; }
}
