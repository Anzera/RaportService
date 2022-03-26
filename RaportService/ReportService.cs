﻿using Cipher;
using EmailSender;
using RaportService.Core;
using RaportService.Core.Repositories;
using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace RaportService
{
    public partial class ReportService : ServiceBase
    {
        private const int SendHour = 8;
        private const int IntervalMinutes = 1;
        private Timer _timer = new Timer(IntervalMinutes * 60000);//60000 ms czyli 1min 
        private ErrorRepository _errorReporitory = new ErrorRepository();
        private RaportRepository _raportReporitory = new RaportRepository();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private Email _email;
        private GenerateHtmlEmail _htmlEmail = new GenerateHtmlEmail();
        private string _emailReciver;
        private StringCipher _stringCipher = new StringCipher("4A782160-5858-42BE-A10C-71316055C4AD");
        private const string NotEncriptedPassPrefix = "encrypt:";

        public ReportService()
        {
            InitializeComponent();

            try
            {
                _emailReciver = ConfigurationManager.AppSettings["ReciverEmail"];        

                _email = new Email(new EmailParams
                {
                    HostSmtp = ConfigurationManager.AppSettings["HostSmtp"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    EnableSssl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSssl"]),
                    SenderName = ConfigurationManager.AppSettings["SenderName"],
                    SenderEmail = ConfigurationManager.AppSettings["SenderEmail"],
                    SenderEmailPassword = DecriptSenderEmailPassword()
            });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
           
        }

        private string DecriptSenderEmailPassword()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["SenderEmailPassword"];

            if (encryptedPassword.StartsWith(NotEncriptedPassPrefix))
            {
                encryptedPassword = _stringCipher.Encrypt(encryptedPassword.Replace(NotEncriptedPassPrefix, ""));

                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.AppSettings.Settings["SenderEmailPassword"].Value = encryptedPassword;

                configFile.Save();
            }

            return _stringCipher.Decrypt(encryptedPassword);
        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();
            Logger.Info("Service start...");
        }

        private async void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                await SendError();
                await SendRaport();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task SendError()
        {
            var errors = _errorReporitory.GetLastErrors(IntervalMinutes);

            if (errors == null || !errors.Any())
                return;

            await _email.Send("Błedy w aplikacji", _htmlEmail.GenerateErrors(errors, IntervalMinutes), _emailReciver);


            Logger.Info("Error sent.");

        }
        private async Task SendRaport()
        {
            var actualHour = DateTime.Now.Hour;

            if (actualHour < SendHour)
                return;

            var raport = _raportReporitory.GetLastNotSendRaport();

            if (raport == null)
                return;

            await _email.Send("Raport dobowy", _htmlEmail.GenerateRaport(raport), _emailReciver);

            _raportReporitory.RaportSend(raport);
            
            Logger.Info("Report sent.");


        }
        protected override void OnStop()
        {
            Logger.Info("Service stopped...");
        }
    }
}
