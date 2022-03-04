using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTeste.Models
{
    public class AtividadeModel
    {
        public string Text { get; set; }

        [Key]
        public string Code { get; set; }

        public List<EmpresaModel> EmpresaPrincipal { get; set; }
        public List<EmpresaModel> EmpresaSecundario { get; set; }
    }
}
