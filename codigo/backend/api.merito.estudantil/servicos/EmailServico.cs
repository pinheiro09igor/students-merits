using System.Net;
using System.Net.Mail;
using api.merito.estudantil.models;
using api.merito.estudantil.repositorios;

namespace api.merito.estudantil.servicos;

public class EmailServico : IEmail
{
    public void EnviarEmail(Email email)
    {
        using MailMessage mail = new();
        mail.From = new MailAddress(CredenciaisParaEnvioDeEmail.Email);
        mail.To.Add(email.EmailDestino);
        mail.Subject = email.Assunto;
        mail.Body = email.Mensagem;
        mail.IsBodyHtml = true;

        using SmtpClient smtp = new(CredenciaisParaEnvioDeEmail.Server, CredenciaisParaEnvioDeEmail.Port);
        smtp.Credentials = new NetworkCredential(CredenciaisParaEnvioDeEmail.Email, CredenciaisParaEnvioDeEmail.Senha);
        smtp.EnableSsl = true;
        smtp.Send(mail);
    }
}