using System.ComponentModel.DataAnnotations;

namespace Lab6.Models
{
    public class Prediction
    {
        [Key]
        public int PredictionId { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "File name cannot exceed 255 characters.")]
        public string FileName { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string Url { get; set; }

        [Required]
        public Question Question { get; set; }
    }

    public enum Question
    {
        Earth,
        Computer
    }
}
