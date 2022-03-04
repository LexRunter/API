using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTeste.Models
{
    public class LoginModel
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public string Usuario { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Cnpj { get; set; }
    }
}
