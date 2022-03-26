using Cipher;
using EmailSender;
using RaportService.Core;
using RaportService.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RaportService.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var stringCipher = new StringCipher("1");
            var encryptedPassword = stringCipher.Encrypt("hasło");
            var decryptedPassword = stringCipher.Decrypt(encryptedPassword);

            Console.WriteLine(encryptedPassword);
            Console.WriteLine(decryptedPassword);

            return;

            var emailReciver = "aniabaj@gmail.com";
            var htmlEmail = new GenerateHtmlEmail();

            var email = new Email(new EmailParams
            {
                HostSmtp = "smtp.gmail.com",
                Port = 587,
                EnableSssl = true,
                SenderName = "Anna Frączek",
                SenderEmail = "afraczekszkolenie.net@gmail.com",
                SenderEmailPassword = "idmgxcurizwwyzqi"
            });

            var raport = new Raport
            {
                Id = 1,
                Title = "R/1/2022",
                Date = new DateTime(2022, 1, 1, 12, 0, 0),
                Positions = new List<RaportPosition>
                {
                    new RaportPosition {Id = 1, RaportId = 1, Title = "Position 1", Description = "Description 1", Value = 44.99m},
                    new RaportPosition {Id = 2, RaportId = 1, Title = "Position 2", Description = "Description 2", Value = 23.99m},
                    new RaportPosition {Id = 3, RaportId = 1, Title = "Position 3", Description = "Description 3", Value = 98.99m}
                }
            };


            var errors = new List<Error>
            {
                new Error {Message = "Błąd testowy 1", Date = DateTime.Now},
                new Error {Message = "Błąd testowy 2", Date = DateTime.Now}
            };

            Console.WriteLine(DateTime.Now + " Wysyłanie email (Raport dobowy)....");

            email.Send("Błedy w aplikacji", htmlEmail.GenerateErrors(errors, 10), emailReciver).Wait();

            Console.WriteLine(DateTime.Now + " Wysłano raport (Raport dobowy)...");

            Console.WriteLine(DateTime.Now + " Wysyłanie email (Raport błedy)....");

            email.Send("Raport dobowy", htmlEmail.GenerateRaport(raport), emailReciver).Wait();

            Console.WriteLine(DateTime.Now + " Wysłano raport (Raport błędy)...");
        }
    }
}
