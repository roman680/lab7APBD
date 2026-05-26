using System.ComponentModel.DataAnnotations;

namespace lab7.DTOs;

public class PcUpdateDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Range(0.000001, double.MaxValue, ErrorMessage = "Weight must be positive.")]
    public double Weight { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Warranty cannot be negative.")]
    public int Warranty { get; set; }

    [Required]
    public DateTime? CreatedAt { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; }
}
