namespace Server
{
    public partial class ChangePasswordDialog : Form
    {
        public ChangePasswordDialog()
        {
            InitializeComponent();

            PasswordTextBox.MaxLength = Globals.MaxPasswordLength;
            
            // 应用UI语言
            UILanguageManager.ApplyUILanguage(this);
        }
    }
}
