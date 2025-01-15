namespace Core.Packages.Application.CrossCuttingConcerns.Email;

public class EmailMessage
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsBodyHtml { get; set; }
    public List<string>? CC { get; set; }
    public List<string>? BCC { get; set; }
    public List<EmailAttachment>? Attachments { get; set; }

    public EmailMessage()
    {
        To = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsBodyHtml = true;
    }

    public EmailMessage(string to, string subject, string body, bool isBodyHtml = true)
    {
        To = to;
        Subject = subject;
        Body = body;
        IsBodyHtml = isBodyHtml;
    }
}

public class EmailAttachment
{
    public string FileName { get; set; }
    public byte[] Content { get; set; }
    public string ContentType { get; set; }

    public EmailAttachment(string fileName, byte[] content, string contentType)
    {
        FileName = fileName;
        Content = content;
        ContentType = contentType;
    }
} 