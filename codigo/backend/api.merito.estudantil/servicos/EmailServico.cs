using System.Net;
using System.Net.Mail;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;

namespace api.merito.estudantil.servicos;

public class EmailServico : IEmail
{
    public async Task EnviarEmail(Email email)
    {
        var cliente = new SmtpClient(CredenciaisParaEnvioDeEmail.Server, CredenciaisParaEnvioDeEmail.Port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(
                CredenciaisParaEnvioDeEmail.Email, 
                CredenciaisParaEnvioDeEmail.Senha)
        };

        await cliente.SendMailAsync(
            new MailMessage(
                from:CredenciaisParaEnvioDeEmail.Email, 
                to:email.EmailDestino, 
                subject:email.Assunto, 
                email.Mensagem));
    }
}