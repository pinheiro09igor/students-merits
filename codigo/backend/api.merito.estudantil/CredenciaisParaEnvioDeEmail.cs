using Microsoft.AspNetCore.Hosting.Server;

namespace api.merito.estudantil
{
    public class CredenciaisParaEnvioDeEmail
    {
        public static string Server => "smtp.gmail.com";
        public static int Port => 587;
        public static string Email => "educationdiogo@gmail.com";
        public static string Senha => "npxs ykcb qvdj avmc";
    }
}
