using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Mail;
/// <summary>
/// 建立郵件服務類別: 我會建立一個郵件服務類別 EmailService，這個類別會封裝發送郵件的邏輯。
/// </summary>
public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly bool _enableSsl;

    public EmailService()
    {
        _smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        _smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
        _smtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
        _smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
        _enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
    }

    public void SendEmail(string to, string subject, string body)
    {
        try
        {
            var mailMessage = new System.Net.Mail.MailMessage()
            {
                From = new MailAddress(_smtpUsername),
                //To.Add(to),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            using (var smtp = new SmtpClient(_smtpServer))
            {
                smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                smtp.EnableSsl = _enableSsl;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.UseDefaultCredentials = false;
                smtp.Port = 587; // 如果你使用  的話
                //smtp.Host = _smtpServer; // 設定郵件伺服器
                smtp.Timeout = 90000000;  // 設定逾時時間 (毫秒)
                smtp.Send(mailMessage);
            }
        }
        catch (Exception ex)
        {
            // 處理例外
            throw new Exception($"郵件發送失敗: {ex.ToString()}");
           
        }
    }
}
/// <summary>
/// 立郵件設定類別: 建立郵件設定的類別，從 appsettings.json 讀取相關設定
/// </summary>
public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public bool EnableSsl { get; set; }
}