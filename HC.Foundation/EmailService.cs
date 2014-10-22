using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using HC.Foundation.Log;
using HC.Framework.Extension;

namespace HC.Foundation
{
    /// <summary>
    ///     整站邮件服务类
    /// </summary>
    public class EmailService
    {
        /// <summary>
        ///     发送邮件
        /// </summary>
        /// <param name="mailTo">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        public static bool Send(string mailTo, string subject, string body)
        {
            return Send(new[] { mailTo }, null, subject, body, true, null);
        }

        /// <summary>
        ///     发送邮件
        /// </summary>
        /// <param name="mailTo">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        public static bool Send(string[] mailTo, string subject, string body)
        {
            return Send(mailTo, null, subject, body, true, null);
        }

        /// <summary>
        ///     发送邮件
        /// </summary>
        /// <param name="mailTo">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="attachmentsPath">附件</param>
        /// <returns></returns>
        public static bool Send(string[] mailTo, string subject, string body, string[] attachmentsPath)
        {
            return Send(mailTo, null, subject, body, true, attachmentsPath);
        }


        /// <summary>
        ///     发送邮件
        /// </summary>
        /// <param name="mailTo">收件人</param>
        /// <param name="mailCcArray">抄送</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isBodyHtml">是否Html</param>
        /// <param name="attachmentsPath">附件</param>
        /// <returns></returns>
        public static bool Send(string[] mailTo, string[] mailCcArray, string subject, string body, bool isBodyHtml,
                                string[] attachmentsPath)
        {
            try
            {
                EmailConfig config = SiteConfig.EmailConfig;
                if (string.IsNullOrEmpty(config.Host) || string.IsNullOrEmpty(config.UserName) ||
                    string.IsNullOrEmpty(config.Port) || string.IsNullOrEmpty(config.Password))
                {
                    LogService.Instance.Log("邮件发送配置错误", "邮件发送配置错误，请检查系统邮件发送配置。", LogCategory.System);
                    return false;
                }
                var @from = new MailAddress(config.MailFrom); //使用指定的邮件地址初始化MailAddress实例
                var message = new MailMessage(); //初始化MailMessage实例 
                //向收件人地址集合添加邮件地址
                if (mailTo != null)
                {
                    foreach (string t in mailTo)
                    {
                        message.To.Add(t);
                    }
                }

                //向抄送收件人地址集合添加邮件地址
                if (mailCcArray != null)
                {
                    foreach (string t in mailCcArray)
                    {
                        message.CC.Add(t);
                    }
                }
                //发件人地址
                message.From = @from;

                //电子邮件的标题
                message.Subject = subject;

                //电子邮件的主题内容使用的编码
                message.SubjectEncoding = Encoding.UTF8;

                //电子邮件正文
                message.Body = body;

                //电子邮件正文的编码
                message.BodyEncoding = Encoding.Default;
                message.Priority = MailPriority.High;
                message.IsBodyHtml = isBodyHtml;

                //在有附件的情况下添加附件
                try
                {
                    if (attachmentsPath != null && attachmentsPath.Length > 0)
                    {
                        foreach (string path in attachmentsPath)
                        {
                            var attachFile = new Attachment(path);
                            message.Attachments.Add(attachFile);
                        }
                    }
                }
                catch (Exception err)
                {
                    LogService.Instance.Log("在添加附件时有错误", err.Message, LogCategory.System);
                }
                try
                {
                    var smtp = new SmtpClient
                        {
                            Credentials = new NetworkCredential(config.UserName, config.Password),
                            Host = config.Host,
                            Port = config.Port.ToInt()
                        };

                    //将邮件发送到SMTP邮件服务器
                    smtp.Send(message);
                    string to = "";
                    if (mailTo != null)
                    {
                        to = mailTo.Aggregate(to, (current, s) => current + (s + ","));
                        LogService.Instance.Log("邮件发送成功", "往" + to.TrimEnd(',') + "邮箱发送邮件成功，邮件内容：" + body,
                                                LogCategory.System);
                    }

                    return true;
                }
                catch (SmtpException ex)
                {
                    LogService.Instance.Log("邮件发送时异常", ex.Message + ex.InnerException, LogCategory.System);
                    return false;
                }
            }
            catch (SmtpException ex)
            {
                LogService.Instance.Log("邮件发送异常", ex.Message, LogCategory.System);
                return false;
            }
        }
    }
}