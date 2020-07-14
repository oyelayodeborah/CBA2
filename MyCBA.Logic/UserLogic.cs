using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MyCBA.Core.Models;

namespace MyCBA.Logic
{
    public class UserLogic
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public string GeneratePassword()
        {
            var PasswordLength = 9;
            string allowedChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*<>?";
            Random randChars = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharC = allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = allowedChars[(int)((allowedChars.Length) * randChars.NextDouble())];
            }
            var password = new string(chars);
            return password.ToString();
        }

        public string SendingEmail(string email, string password, string fullName)
        {
            var fromEmail = new MailAddress("oyelayodeborah@gmail.com", "DebsCBA");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "15ch03774";
            string subject = "Login details !!!";

            string body = "<br/><br/>Dear " + fullName + ",<br/> We are happy to tell you your account has been created successfully<br/> " +
                "Username: " + email + "<br/>Password: " + password + "";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    smtp.Send(message);
                    return "Successful";
                }
                catch (SmtpException)
                {
                    //SmtpException output= new SmtpException("An error ocurred while sending the mail");
                    //throw output;
                    var output = "Error";
                    return output;
                }
      
        }
        public bool IsDetailsExist(string email,string phoneNumber, string username)
        {
            var findDetails = _context.Users.Where(a => a.email == email || a.phoneNumber==phoneNumber || a.username== username).Count();

            if (findDetails >= 1)
            {
                return true;
            }
            return false;
        }
        public bool IsEditDetailsExist(string email, string phoneNumber)
        {

            var findDetails = _context.Users.Where(a => a.email == email || a.phoneNumber == phoneNumber).Count();
            if (findDetails > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsRoleCheck(object role)
        {
            var Role = (string)role;
            var findRole = _context.Roles.Where(a => a.name == Role).FirstOrDefault();

            var findUserRole = _context.Users.Where(a => a.roleId == findRole.id).FirstOrDefault();

            if (findUserRole == null)
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
