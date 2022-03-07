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
using System.Reflection;
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

        //Metodo para efetuar login
        [HttpGet("Login")]
        public async Task<ActionResult> Login(LoginModel pLogin)
        {
            //Verifica se o usuario é valido
            if (pLogin.Email == "")
                return BadRequest("Email Inválido.");

            //Verifica se a senha é valida
            if (pLogin.Senha == "")
                return BadRequest("Senha Inválida.");

            using (var dbContext = new EfContext())
            {
                if (dbContext.Login.Where(x => x.Email == pLogin.Email && x.Senha == pLogin.Senha).ToList().Count > 0)
                {
                    string tableName = "Empresa";
                    var type = Assembly.GetExecutingAssembly()
                            .GetTypes()
                            .FirstOrDefault(t => t.Name == tableName);

                    //if (type != null)
                    //    DbSet catContext = dbContext.Set(type);

                    var myResult = dbContext.Login.Where(c => c.Email == pLogin.Email && c.Senha  == pLogin.Senha).Select(c => c.Cnpj);
                    string sCnpj = dbContext.Login.Where(x => x.Email == pLogin.Email && x.Senha == pLogin.Senha).Select(x =>  x.Cnpj).ToString();
                    //EmpresaModel Empresa = dbContext.Empresa.Where(x => x.Cnpj == sCnpj);


                    return new ObjectResult(dbContext.Empresa.Where(x => x.Cnpj == sCnpj));
                }
                else
                {
                    return BadRequest("Email ou senha incorretos.");
                }

            }
        }

        //Metodo para cadastrar novos usuarios
        [HttpPost("Cadastro")]
        public async Task<ActionResult> IncluiCadastro(LoginModel pLogin)
        {
            //Verifica se o usuario é valido
            if (pLogin.Usuario == "")
                return BadRequest("Usuario Inválido.");

            //Verifica se a senha é valida
            if (pLogin.Senha == "")
                return BadRequest("Senha Inválida.");

            //Verifica se o email é valido
            if (!ValidaEmail.ValidarEmail(pLogin.Email))
                return BadRequest("Email Inválido.");

            //Verifica se o cnpj é valido
            if (!ValidaCnpj.ValidarCnpj(pLogin.Cnpj))
                return BadRequest("CNPJ Inválido.");

            using (var httpClient = new HttpClient())
            {
                //Remover mascara do Cnpj
                string sCnpj = pLogin.Cnpj;
                sCnpj = sCnpj.Trim();
                pLogin.Cnpj = sCnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                //Recebe o retorno da api
                var retorno = await httpClient.GetAsync($"https://receitaws.com.br/v1/cnpj/{pLogin.Cnpj}");
                var jsonEmpresa = await retorno.Content.ReadAsStringAsync();

                //Converte o retorno ao EmpresaModel
                var empresa = JsonConvert.DeserializeObject<EmpresaModel>(jsonEmpresa);
                empresa.Cnpj = sCnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                using (var dbContext = new EfContext())
                {
                    //Verifica se existe, caso não exista ele cria o banco de dados
                    dbContext.Database.EnsureCreated();

                    //Verifica se o cnpj ja está cadastrado
                    if (dbContext.Login.Where(x => x.Cnpj == pLogin.Cnpj).ToList().Count > 0)
                        return BadRequest("Cnpj já cadastrado.");

                    //Verifica se o email já está cadastrado
                    if (dbContext.Login.Where(x => x.Email == pLogin.Email).ToList().Count > 0)
                        return BadRequest("Email já cadastrado.");

                    //Adiciona o cadastro ao banco
                    dbContext.Login.Add(pLogin);

                    //Verifica se o retorno da api trouxe as informações corretas
                    if (empresa.Status != "ERROR")
                    {
                        //remove a atividade caso ja exista no banco
                        if (dbContext.Atividades.Where(x => x.Code == empresa.AtividadePrincipal[0].Code).ToList().Count > 0)
                        {
                            empresa.AtividadePrincipal.Clear();
                        }

                        List<string> lstEmpresa = new List<string>();

                        //remove as atividades secundarias caso ja exista no banco
                        foreach (var row in empresa.AtividadesSecundarias)
                        {
                            if (dbContext.Atividades.Where(x => x.Code == row.Code).ToList().Count > 0)
                            {
                                lstEmpresa.Add(row.Code);
                            }
                        }

                        empresa.AtividadesSecundarias.RemoveAll(i => lstEmpresa.Contains(i.Code));

                        if (empresa != null)
                            dbContext.Empresa.Add(empresa);
                    }

                    //Aceita as alterações feitas
                    await dbContext.SaveChangesAsync();
                }

                return Ok("Cadastro efetuado com sucesso.");
            }

        }
    }
}
