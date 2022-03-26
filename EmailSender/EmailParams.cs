namespace EmailSender
{
    public class EmailParams
    {
        public string HostSmtp { get; set; }
        public bool EnableSssl { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderEmailPassword { get; set; }
        public string SenderName { get; set; }


    }
}
