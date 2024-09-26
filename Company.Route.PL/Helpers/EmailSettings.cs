using Company.Route.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.Route.PL.Helpers
{
	public static class EmailSettings
	{

		public static void SendEmail(Email email)
		{
			// Mail Server : gmail.com	

			// Smtp
			var client  = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;

			// the account will send the email 
			client.Credentials = new NetworkCredential("atosha1fathy@gmail.com", "pwheaooskhbxrvvm");


			client.Send("atosha1fathy@gmail.com", email.To, email.Subject, email.Body);

		}

    }
}
