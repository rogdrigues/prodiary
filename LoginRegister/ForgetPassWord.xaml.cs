using MimeKit;
using MailKit.Net.Smtp;
using ProDiaryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ProDiaryApplication.LoginRegister
{
    /// <summary>
    /// Interaction logic for ForgetPassWord.xaml
    /// </summary>
    public partial class ForgetPassWord : Window
    {
        public ForgetPassWord()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;

            if (button.Name == "btnEmail")
            {
                emailPage();
            }
            else if (button.Name == "btnResend")
            {
                resendEmail();
            }
            else if (button.Name == "btnChangePassword")
            {
                passwordPage();
            }
            else if (button.Name == "btnVerification")
            {
                verifyPage();
            }
        }

        private void emailPage()
        {
            string emailText = txtSearchEmail.Text;

            if (!IsValidEmail(emailText))
            {
                MessageBox.Show("Invalid email format.", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (DiaryNoteContext noteContext = new DiaryNoteContext())
            {
                bool isEmailExist = noteContext.Accounts.Any(a => a.Email == emailText);
                if (!isEmailExist)
                {
                    MessageBox.Show("Email does not exist in the system.", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string verificationCode = GenerateVerificationCode();
                SendVerificationCodeToEmail(emailText, verificationCode);

                VerificationCode verification = new VerificationCode
                {
                    Email = emailText,
                    Code = verificationCode,
                    Created = DateTime.Now
                };
                noteContext.VerificationCodes.Add(verification);
                noteContext.SaveChanges();

                EmailPage.Visibility = Visibility.Hidden;
                VerificationPage.Visibility = Visibility.Visible;
            }
        }
        private void verifyPage()
        {
            string verificationCode = txtVerificationCode.Text;

            using (DiaryNoteContext noteContext = new DiaryNoteContext())
            {
                bool isCodeCorrect = noteContext.VerificationCodes.Any(v => v.Code == verificationCode);
                if (!isCodeCorrect)
                {
                    MessageBox.Show("Incorrect verification code, please try again.", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var verification = noteContext.VerificationCodes.FirstOrDefault(v => v.Code == verificationCode);
                if (verification.Created == null || DateTime.Now - verification.Created.Value > TimeSpan.FromMinutes(3))
                {
                    MessageBox.Show("Verification code has expired.", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                    noteContext.VerificationCodes.RemoveRange(noteContext.VerificationCodes.Where(v => v.Created != null && DateTime.Now - v.Created.Value > TimeSpan.FromMinutes(3)));
                    noteContext.SaveChanges();
                    return;
                }

                noteContext.VerificationCodes.Remove(verification);
                noteContext.SaveChanges();

                VerificationPage.Visibility = Visibility.Hidden;
                PasswordPage.Visibility = Visibility.Visible;
            }
        }
        private void resendEmail()
        {
            string email = txtSearchEmail.Text;

            using (DiaryNoteContext context = new DiaryNoteContext())
            {
                var existingAccount = context.Accounts.FirstOrDefault(a => a.Email == email);
                if (existingAccount == null)
                {
                    MessageBox.Show("Email does not exist in the system.", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string verificationCode = GenerateVerificationCode();

                VerificationCode newVerificationCode = new VerificationCode
                {
                    Email = email,
                    Code = verificationCode,
                    Created = DateTime.Now
                };
                context.VerificationCodes.Add(newVerificationCode);
                context.SaveChanges();

                SendVerificationCodeToEmail(email, verificationCode);

                MessageBox.Show("A new verification code has been sent to your email.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void passwordPage()
        {
            string passwordText = txtPassword.Text;

            if (!IsValidPassword(passwordText))
            {
                MessageBox.Show("Invalid password format.", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (DiaryNoteContext noteContext = new DiaryNoteContext())
            {
                var verification = noteContext.VerificationCodes.FirstOrDefault(v => v.Email == txtSearchEmail.Text);
                if (verification != null)
                {
                    var account = noteContext.Accounts.FirstOrDefault(a => a.Email == verification.Email);
                    if (account != null)
                    {
                        account.Password = passwordText;
                        noteContext.Accounts.Update(account);
                        noteContext.SaveChanges();

                        MessageBox.Show("Password has been changed successfully.", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }
        private bool IsValidPassword(string password)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
            Regex regex = new Regex(passwordPattern);
            return regex.IsMatch(password);
        }
        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        private void SendVerificationCodeToEmail(string recipientEmail, string verificationCode)
        {
            using (DiaryNoteContext noteContext = new DiaryNoteContext())
            {
                Account account = noteContext.Accounts.FirstOrDefault(i => i.Email == recipientEmail);

                if (account != null)
                {
                    string subject = "Pin code verify";
                    string body = emailHTML(account.FullName, verificationCode, recipientEmail, account.Id);

                    SendEmail(recipientEmail, subject, body);
                }
            }
        }
        private void SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                message.From.Add(new MailboxAddress("Piligon C.", "lifeofcoffie@gmail.com"));

                message.To.Add(new MailboxAddress("", recipientEmail));

                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("lifeofcoffie@gmail.com", "shgwxqwapkgwzqkk");

                    client.Send(message);
                    client.Disconnect(true);
                }

                MessageBox.Show("Email sent successfully.", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static String emailHTML(String fullName, String pinCode, String emailSend, int accountID)
        {
            String email = "    <div style=\"margin: 0; padding: 0; width: 100%; word-break: break-word; -webkit-font-smoothing: antialiased;  background-color: #eceff1; background-color: rgba(236, 239, 241, 1);\">\n"
                    + "        <div style=\"display: none;\">A request to receive pin code was received from your Hilfmotor Account</div>\n"
                    + "        <div role=\"article\" aria-roledescription=\"email\" aria-label=\"Pincode login\" lang=\"en\">\n"
                    + "            <table style=\"font-family: Montserrat, -apple-system, 'Segoe UI', sans-serif; width: 100%;\" width=\"100%\"\n"
                    + "                cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n"
                    + "                <tr>\n"
                    + "                    <td align=\"center\"\n"
                    + "                        style=\" background-color: #eceff1; background-color: rgba(236, 239, 241, 1); font-family: Montserrat, -apple-system, 'Segoe UI', sans-serif;\"\n"
                    + "                        bgcolor=\"rgba(236, 239, 241, 1)\">\n"
                    + "                        <table class=\"sm-w-full\" style=\"font-family: 'Montserrat',Arial,sans-serif; width: 600px;\"\n"
                    + "                            width=\"600\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n"
                    + "                            <tr>\n"
                    + "                                <td class=\"sm-py-32 sm-px-24\"\n"
                    + "                                    style=\"font-family: Montserrat, -apple-system, 'Segoe UI', sans-serif; padding: 48px; text-align: center;\"\n"
                    + "                                    align=\"center\">\n"
                    + "                                    <a>\n"
                    + "                                        <img src=\"https://i.pinimg.com/736x/b3/a6/a1/b3a6a10a79908d5399673e4de0d89b80.jpg\"\n"
                    + "                                            style=\"border-radius:50%;\" width=\"155\" alt=\"Hilfmotor Admin\"\n"
                    + "                                            style=\"border: 0; max-width: 100%; line-height: 100%; vertical-align: middle;\">\n"
                    + "                                    </a>\n"
                    + "                                </td>\n"
                    + "                            </tr>\n"
                    + "                            <tr>\n"
                    + "                                <td align=\"center\" class=\"sm-px-24\" style=\"font-family: 'Montserrat',Arial,sans-serif;\">\n"
                    + "                                    <table style=\"font-family: 'Montserrat',Arial,sans-serif; width: 100%;\" width=\"100%\"\n"
                    + "                                        cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n"
                    + "                                        <tr>\n"
                    + "                                            <td class=\"sm-px-24\"\n"
                    + "                                                style=\" background-color: #ffffff; background-color: rgba(255, 255, 255, 1); border-radius: 4px; font-family: Montserrat, -apple-system, 'Segoe UI', sans-serif; font-size: 14px; line-height: 24px; padding: 48px; text-align: left;  color: #626262; color: rgba(98, 98, 98, 1);\"\n"
                    + "                                                bgcolor=\"rgba(255, 255, 255, 1)\" align=\"left\">\n"
                    + "                                                <p style=\"font-weight: 600; font-size: 18px; margin-bottom: 0;\">Hey</p>\n"
                    + "                                                <p\n"
                    + "                                                    style=\"font-weight: 700; font-size: 20px; margin-top: 0;  color: #ff5850; color: rgba(255, 88, 80, 1);\">\n"
                    + "                                                     " + fullName + "</p>\n"
                    + "                                                <p style=\"margin: 0 0 24px;\">\n"
                    + "                                                    A request to receive your pin code from\n"
                    + "                                                    <span style=\"font-weight: 600;\">Piligon C.</span> Account -\n"
                    + "                                                    <a href=\"mailto:" + emailSend + "\" class=\"hover-underline\"\n"
                    + "                                                        style=\" color: #7367f0; color: rgba(115, 103, 240, 1); text-decoration: none;\">" + emailSend + "</a>\n"
                    + "                                                    (ID: " + accountID + ").\n"
                    + "                                                </p>\n"
                    + "                                                <p style=\"margin: 0 0 24px;\">Use this pin code to change your password.\n"
                    + "                                                </p>\n"
                    + "                                                <table style=\"font-family: 'Montserrat',Arial,sans-serif; margin:auto;\"\n"
                    + "                                                    cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n"
                    + "                                                    <tr>\n"
                    + "                                                        <td style=\"mso-padding-alt: 16px 24px;  background-color: #7367f0; background-color: rgba(115, 103, 240, 1); border-radius: 4px; font-family: Montserrat, -apple-system, 'Segoe UI', sans-serif;\"\n"
                    + "                                                            bgcolor=\"rgba(115, 103, 240, 1)\">\n"
                    + "                                                            <a href=\"#/\"\n"
                    + "                                                                style=\"display: block; font-weight: 600; font-size: 14px; line-height: 100%; padding: 16px 24px;  color: #ffffff; color: rgba(255, 255, 255, 1); text-decoration: none;\">Your\n"
                    + "                                                                Pin Code: " + pinCode + " </a>\n"
                    + "                                                        </td>\n"
                    + "                                                    </tr>\n"
                    + "                                                </table>\n"
                    + "                                                <p style=\"margin: 24px 0;\">\n"
                    + "                                                    <span style=\"font-weight: 600;\">Note:</span> This pin code is valid\n"
                    + "                                                    for 1 hour from the time it was\n"
                    + "                                                    sent to you and can be used to login to admin page.\n"
                    + "                                                </p>\n"
                    + "                                                <p style=\"margin: 0;\">\n"
                    + "                                                    If you did not intend to deactivate your account or need our help\n"
                    + "                                                    keeping the account, please\n"
                    + "                                                    contact us at\n"
                    + "                                                    <a href=\"mailto:piligoncounterlogic@gmail.com\" class=\"hover-underline\"\n"
                    + "                                                        style=\"color: #7367f0; color: rgba(115, 103, 240, 1); text-decoration: none;\">piligoncounterlogic@gmail.com</a>\n"
                    + "                                                </p>\n"
                    + "                                                <table style=\"font-family: 'Montserrat',Arial,sans-serif; width: 100%;\"\n"
                    + "                                                    width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\n"
                    + "                                                    <tr>\n"
                    + "                                                        <td\n"
                    + "                                                            style=\"font-family: 'Montserrat',Arial,sans-serif; padding-top: 32px; padding-bottom: 32px;\">\n"
                    + "                                                            <div\n"
                    + "                                                                style=\"background-color: #eceff1; background-color: rgba(236, 239, 241, 1); height: 1px; line-height: 1px;\">\n"
                    + "                                                                &zwnj;</div>\n"
                    + "                                                        </td>\n"
                    + "                                                    </tr>\n"
                    + "                                                </table>\n"
                    + "                                                <p style=\"margin: 0 0 16px;\">\n"
                    + "                                                    Not sure why you received this email? Please\n"
                    + "                                                    <a href=\"mailto:piligoncounterlogic@gmail.com\" class=\"hover-underline\"\n"
                    + "                                                        style=\"color: #7367f0; color: rgba(115, 103, 240, 1); text-decoration: none;\">let\n"
                    + "                                                        us know</a>.\n"
                    + "                                                </p>\n"
                    + "                                                <p style=\"margin: 0 0 16px;\">Thanks, <br>The Piligon C. Team</p>\n"
                    + "                                            </td>\n"
                    + "                                        </tr>\n"
                    + "                                        <tr>\n"
                    + "                                            <td style=\"font-family: 'Montserrat',Arial,sans-serif; height: 20px;\"\n"
                    + "                                                height=\"20\"></td>\n"
                    + "                                        </tr>\n"
                    + "                                        <tr>\n"
                    + "                                            <td style=\"font-family: 'Montserrat',Arial,sans-serif; height: 16px;\"\n"
                    + "                                                height=\"16\"></td>\n"
                    + "                                        </tr>\n"
                    + "                                    </table>\n"
                    + "                                </td>\n"
                    + "                            </tr>\n"
                    + "                        </table>\n"
                    + "                    </td>\n"
                    + "                </tr>\n"
                    + "            </table>\n"
                    + "        </div>\n"
                    + "    </div>";
            return email;
        }
    }
}
