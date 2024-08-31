
    using MimeKit;

    namespace Models
    {
        public class Messager
        {
            public List<MailboxAddress> To { get; set; }
            public string Subject { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
            public Messager(IEnumerable<string> to, string subject, string content)
            {
                To = new List<MailboxAddress>();
                To.AddRange(to.Select(x => new MailboxAddress("email", x)));
                Subject = subject;
                Content = content;
            }
        }
    }

