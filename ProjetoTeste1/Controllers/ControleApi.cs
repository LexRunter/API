using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetoTeste.CamadaNegocio;
using ProjetoTeste.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjetoTeste.Controllers
{
    [ApiController]
    [Route("")]
    public class ControleApi : ControllerBase
    {
        private readonly ILogger<ControleApi> _logger;

        public ControleApi(ILogger<ControleApi> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        //Cadastrar novos usuarios
        [HttpPost("Cadastro")]
        public async Task<ActionResult> IncluiCadastro(LoginModel pLogin)
        {
            //Verifica se o usuario é valido
            if (pLogin.Usuario == "")
                return BadRequest("Usuario Inválido");

            //Verifica se a senha é valida
            if (pLogin.Senha == "")
                return BadRequest("Senha Inválida");

            //Verifica se o email é valido
            if (!ValidaEmail.ValidarEmail(pLogin.Email))
                return BadRequest("Email Inválido");

            //Verifica se o cnpj é valido
            if (!ValidaCnpj.ValidarCnpj(pLogin.Cnpj))
                return BadRequest("CNPJ Inválido");

            using (var httpClient = new HttpClient())
            {
                //Remover mascara do Cnpj
                string sCnpj = pLogin.Cnpj;
                sCnpj = sCnpj.Trim();
                pLogin.Cnpj = sCnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                var retorno = await httpClient.GetAsync( $"https://receitaws.com.br/v1/cnpj/{pLogin.Cnpj}" );
                var jsonEmpresa = await retorno.Content.ReadAsStringAsync();

                var empresa = JsonConvert.DeserializeObject<EmpresaModel>( jsonEmpresa );

                using (var dbContext = new EfContext())
                {
                    dbContext.EMPRESA.ToList();
                    //dbContext.Database.EnsureDeleted();
                    //dbContext.Database.EnsureCreated();
                    //Verifica se o cnpj ja está cadastrado
                    if (dbContext.LOGIN.Where(x => x.Cnpj == pLogin.Cnpj).ToList().Count > 0)
                        return BadRequest("Cnpj já cadastrado");

                    //Verifica se o email já está cadastrado
                    if (dbContext.LOGIN.Where(x => x.Email == pLogin.Email).ToList().Count > 0)
                        return BadRequest("Email já cadastrado");

                    //Adiciona o cadastro ao banco
                    dbContext.LOGIN.Add(pLogin);
                    dbContext.EMPRESA.Add( empresa );
                    await dbContext.SaveChangesAsync();
                }

                return new ObjectResult(await retorno.Content.ReadAsStringAsync());
            }

        }
    }
}
