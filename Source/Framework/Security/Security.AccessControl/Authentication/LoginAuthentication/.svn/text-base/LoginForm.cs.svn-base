using System;
using System.ServiceModel;
using System.Windows.Forms;
using System.Configuration;

using HiiP.Framework.Common.ApplicationContexts;
using HiiP.Framework.Security.AccessControl.Properties;
using HiiP.Framework.Common.Client;
using HiiP.Infrastructure.Interface.Miscellaneous;


namespace HiiP.Framework.Security.AccessControl.Authentication
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        internal LoginForm(bool isCookieTimeOut) :this()
        {
            this.IsCookieTimeout = isCookieTimeOut;
        }

        public bool IsCookieTimeout
        {
            get;
            set;
        }

        public string SectionUserName()
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                return this.UserName;
            }

            return string.Empty;
        }

        public string UserName
        { get; set; }

        public void Login(string userName, string password)
        {
            AppContext.Current.UserName = userName;
            this.errorMessageLabel.Text = string.Empty;

            //Modify to renew the cookie time out for web seal on 2008.09.03.
            if (AuthenticationManager.Login(userName, password,this.IsCookieTimeout))
            {
                AuthenticationManager.IsOpenLoginForm = false;
                this.DialogResult = DialogResult.OK;
                this.UserName = userName;
                this.Close();
            }
            else
            {
                this.errorMessageLabel.Text = Resources.InvalidCredential;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {

            string userName = this.userNameTextBox.Text.Trim();
            string password = this.passwordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                this.errorMessageLabel.Text = Resources.EmptyUsername;
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                this.errorMessageLabel.Text = Resources.EmptyPassword;
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            // *** Shell start point *** 
            // *** Write start tick for logging ***
            // *** Form authentication mode
            // *** Write by Ya Ming ***
            //System.Diagnostics.Trace.WriteLine("Client:Start to login...");
            LoggingVariables.SetLoginStartTick();

            try
            {

                this.Login(userName, password);
            }
            catch (ProtocolException)
            {
                try
                {

                    this.Login(userName, password);
                }
                catch (Exception innerEx)
                {
                    ExceptionManager.HandleForLogin(AuthenticationService.HandleWebSealException(innerEx));
                }
                finally
                {
                    this.Cursor = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                ExceptionManager.HandleForLogin(AuthenticationService.HandleWebSealException(ex));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                //System.Diagnostics.Trace.WriteLine("Client:End to login...");

            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.InitTwoLinks();
            this.HanderCookieTimeOut();
        }

        private void HanderCookieTimeOut()
        {
            if ((this.IsCookieTimeout)
                && (AppContext.Current.UserName.Length > 0))
            {
                this.userNameTextBox.Enabled  = false;
                this.userNameTextBox.Text = AppContext.Current.UserName;
            }
        }

        private void InitTwoLinks()
        {
            try
            {
                string usageData = "By continuing you accept the <a title=\"{0}\" href=\"{0}\">Conditions of Usage</a>";
                this.ultraFormattedLinkLabelWorkspace.Value = string.Format(usageData, ConfigurationManager.AppSettings["UsageURL"]);
            }
            catch (Exception ex)
            {
                if (ExceptionManager.Handle(ex)) throw;
            }
        }

        public void SetUsernameReadonly()
        {
            userNameTextBox.Text = AppContext.Current.UserName;
            userNameTextBox.ReadOnly = true;
        }

    }
}
