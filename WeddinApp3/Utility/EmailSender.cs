using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WeddinApp3.Utility
{
    public class EmailSender : IEmailSender
    {

        //Mailjet method to send notification emails 
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient("8a68ec497fc48b9d260704825f756c78", "8bf551434cdbb4e34ab37794bee88698")
            {
                
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
           .Property(Send.FromEmail, "lauratinn3y@hotmail.co.uk")
           .Property(Send.FromName, "Booking")
           .Property(Send.Subject, subject)
           .Property(Send.HtmlPart, htmlMessage)
           .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                 }
               });
            MailjetResponse response = await client.PostAsync(request);
     
        }
    }
}
