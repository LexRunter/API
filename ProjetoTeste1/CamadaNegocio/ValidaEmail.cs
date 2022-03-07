using System.Text.RegularExpressions;

namespace ProjetoTeste.CamadaNegocio
{
    public class ValidaEmail
    {
        public static bool ValidarEmail(string pEmail)
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",RegexOptions.CultureInvariant | RegexOptions.Singleline);
            bool EmailValido = regex.IsMatch(pEmail);
            if (!EmailValido)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
