using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTeste.Models
{
    public class QsaModel
    {
        [Key]
        public int? Id { get; set; }
        public string Qual { get; set; }
        public string Nome { get; set; }
    }
}
