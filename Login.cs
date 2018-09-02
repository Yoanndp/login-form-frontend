using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginFormTutorial
{
    public partial class Login : Form
    {
        public static string Username;
        public static string Password;

        public Login()
        {
            InitializeComponent();
        }

        //The main function
        public static int Execute(string action, string args)
        {
            WebClient requests = new WebClient();
            string url = "https://yoanndp.tk/tutorial/execute.php";
            string urlaction = "?action=" + action;
            string urlargs = "&" + args;
            string buildurl = url + urlaction + urlargs;

            string response = requests.DownloadString(buildurl);
            if(response == null)
            {
                return 0;
            }

            if (!response.StartsWith("OK"))
            {
                CheckError(response);
                return 0;
            }

            return 1;
        }

        public static void RaiseError(string error)
        {
            MessageBox.Show(error, "An error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static int CheckError(string error)
        {
            Dictionary<string, string> Errors = new Dictionary<string, string>();
            Errors.Add("MISSING_PARAMETERS", "Missing parameters");
            Errors.Add("INVALID_KEY", "The registration key is not valid");
            Errors.Add("USERNAME_TOO_SHORT", "Your username is too short");
            Errors.Add("PASSWORD_TOO_SHORT", "Your password is too short");
            Errors.Add("USERNAME_TAKEN", "The username you choose is already taken");
            Errors.Add("PASSWORDS_NOT_MATCH", "Passwords do not match");
            Errors.Add("USER_BANNED", "You are banned");
            Errors.Add("NO_ACTION", "No action");
            Errors.Add("NOT_ENOUGH_PRIVILEGES", "You do not have enough privileges");
            Errors.Add("INVALID_CREDENTIALS", "Invalid credentials");

            if (!error.StartsWith("ERROR"))
            {
                RaiseError(error);
                return 0;
            }

            string message = "Undefined error";
            string[] array = error.Split(':');
            if(array.Length == 2 && Errors.ContainsKey(array[1]))
            {
                string key = array[1];
                message = Errors[key];
            }

            RaiseError(message);
            return 1;
        }

        private void textBoxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Execute("accessAccount", "userName=" + textBoxUsername.Text + "&password=" + textBoxPassword.Text) == 1)
            {
                Username = textBoxUsername.Text; //To pass it to any other froms (=global)
                //same for pass
                WebClient fetchInfo = new WebClient();
                string premiumState = fetchInfo.DownloadString("https://yoanndp.tk/tutorial/execute.php?action=isPremium&userName="  +Username);
                if(premiumState == "1")
                {
                    MessageBox.Show("You are logged in! Thank you for premium!", "Logged in", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("You are logged in!", "Logged in", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(null, null);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Register rg = new Register();
            rg.ShowDialog();
        }
    }
}
