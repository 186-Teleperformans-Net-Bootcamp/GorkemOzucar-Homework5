using Homework5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HangfireService.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppDbContext _context;
        public EmailService(AppDbContext context)
        {
            _context = context;
        }
        public void SendEmail()
        {
            var users = _context.Users.ToList();
            foreach (var user in users)
            {
                File.AppendAllText(@"C:\Users\sg_oz\Desktop\patikabootcamp\hangfire.txt", $"Kullanıcı Adı: {user.UserName} Email: {user.Email}\n");
            }
            Console.WriteLine("Email gönderildi.");
        }
    }
}
