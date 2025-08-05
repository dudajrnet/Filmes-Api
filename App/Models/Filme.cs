using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models
{
    public class Filme
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage ="O título deve ser preenchido")]
        public string Titulo { get; set; }
        
        [Required(ErrorMessage ="O gênero deve ser preenchido")]
        [MaxLength(50, ErrorMessage ="O tamanho não deve exceder 50 caracteres")]
        public string Genero { get; set; }

        [Required]
        [Range(70, 600, ErrorMessage ="A duração dever ser entre 70 e 600 minutos")]
        public int Duracao { get; set; }
    }
}
