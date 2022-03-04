using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTeste.Models
{
    public class EmpresaModel
    {
        [JsonProperty( "atividade_principal" )]
        public List<AtividadeModel> AtividadePrincipal { get; set; }

        [JsonProperty( "data_situacao" )]
        public string DataSituacao { get; set; }

        public string Complemento { get; set; }

        public string Tipo { get; set; }

        public string Nome { get; set; }

        public string Uf { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        [JsonProperty( "atividades_secundarias" )]
        public List<AtividadeModel> AtividadesSecundarias { get; set; }

        public List<QsaModel> Qsa { get; set; }

        public string Situacao { get; set; }

        public string Bairro { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Cep { get; set; }

        public string Municipio { get; set; }

        public string Porte { get; set; }

        public string Abertura { get; set; }

        [JsonProperty( "natureza_juridica" )]
        public string NaturezaJuridica { get; set; }

        [Key]
        public string Cnpj { get; set; }

        [JsonProperty( "ultima_atualizacao" )]
        public string UltimaAtualizacao { get; set; }

        public string Status { get; set; }

        public string Fantasia { get; set; }

        public string Efr { get; set; }

        [JsonProperty( "motivo_situacao" )]
        public string MotivoSituacao { get; set; }

        [JsonProperty( "situacao_especial" )]
        public string SituacaoEspecial { get; set; }

        [JsonProperty( "data_situacao_especial" )]
        public string DataSituacaoEspecial { get; set; }

        [JsonProperty( "capital_social" )]
        public string CapitalSocial { get; set; }

        public BillingModel Billing { get; set; }
    }
}
