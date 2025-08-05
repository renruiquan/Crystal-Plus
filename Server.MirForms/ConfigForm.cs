using Server.MirEnvir;
using System.Net;
using System.Text.RegularExpressions;
using System.Linq;

namespace Server
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();

            VPathTextBox.Text = Settings.VersionPath;
            VersionCheckBox.Checked = Settings.CheckVersion;
            RelogDelayTextBox.Text = Settings.RelogDelay.ToString();
            
            // 初始化语言下拉框
            InitializeLanguageComboBox();

            IPAddressTextBox.Text = Settings.IPAddress;
            PortTextBox.Text = Settings.Port.ToString();
            TimeOutTextBox.Text = Settings.TimeOut.ToString();
            MaxUserTextBox.Text = Settings.MaxUser.ToString();

            StartHTTPCheckBox.Checked = Settings.StartHTTPService;
            HTTPIPAddressTextBox.Text = Settings.HTTPIPAddress;
            HTTPTrustedIPAddressTextBox.Text = Settings.HTTPTrustedIPAddress;

            AccountCheckBox.Checked = Settings.AllowNewAccount;
            PasswordCheckBox.Checked = Settings.AllowChangePassword;
            LoginCheckBox.Checked = Settings.AllowLogin;
            NCharacterCheckBox.Checked = Settings.AllowNewCharacter;
            DCharacterCheckBox.Checked = Settings.AllowDeleteCharacter;
            StartGameCheckBox.Checked = Settings.AllowStartGame;
            AllowAssassinCheckBox.Checked = Settings.AllowCreateAssassin;
            AllowArcherCheckBox.Checked = Settings.AllowCreateArcher;
            Resolution_textbox.Text = Settings.AllowedResolution.ToString();
            ObserveCheckBox.Checked = Settings.AllowObserve;

            SafeZoneBorderCheckBox.Checked = Settings.SafeZoneBorder;
            SafeZoneHealingCheckBox.Checked = Settings.SafeZoneHealing;
            gameMasterEffect_CheckBox.Checked = Settings.GameMasterEffect;
            lineMessageTimeTextBox.Text = Settings.LineMessageTimer.ToString();

            SaveDelayTextBox.Text = Settings.SaveDelay.ToString();

            ServerVersionLabel.Text = Application.ProductVersion;
            DBVersionLabel.Text = MirEnvir.Envir.LoadVersion.ToString() + ((MirEnvir.Envir.LoadVersion < MirEnvir.Envir.Version) ? " (Update needed)" : "");
            maxConnectionsPerIP.Text = Settings.MaxIP.ToString();
            expRateInput.Value = Math.Round((decimal)Settings.ExpRate, 2);
            dropRateInput.Value = Math.Round((decimal)Settings.DropRate, 2);
            tbRestedPeriod.Text = Settings.RestedPeriod.ToString();
            tbRestedBuffLength.Text = Settings.RestedBuffLength.ToString();
            tbRestedExpBonus.Text = Settings.RestedExpBonus.ToString();
            tbMaxRestedBonus.Text = Settings.RestedMaxBonus.ToString();
            
            // 加载并应用UI语言
            UILanguageManager.LoadUILanguage(Settings.LanguageFilePath);
            UILanguageManager.ApplyUILanguage(this);
        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Save();
            Settings.LoadVersion();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        public void Save()
        {
            Settings.VersionPath = VPathTextBox.Text;
            Settings.CheckVersion = VersionCheckBox.Checked;

            IPAddress tempIP;
            if (IPAddress.TryParse(IPAddressTextBox.Text, out tempIP))
                Settings.IPAddress = tempIP.ToString();

            Settings.StartHTTPService = StartHTTPCheckBox.Checked;
            if (tryParseHttp())
                Settings.HTTPIPAddress = HTTPIPAddressTextBox.Text.ToString();

            if (tryParseTrustedHttp())
                Settings.HTTPTrustedIPAddress = HTTPTrustedIPAddressTextBox.Text.ToString();

            ushort tempshort;
            int tempint;

            if (ushort.TryParse(PortTextBox.Text, out tempshort))
                Settings.Port = tempshort;

            if (ushort.TryParse(TimeOutTextBox.Text, out tempshort))
                Settings.TimeOut = tempshort;

            if (ushort.TryParse(MaxUserTextBox.Text, out tempshort))
                Settings.MaxUser = tempshort;

            if (ushort.TryParse(RelogDelayTextBox.Text, out tempshort))
                Settings.RelogDelay = tempshort;

            if (ushort.TryParse(SaveDelayTextBox.Text, out tempshort))
                Settings.SaveDelay = tempshort;

            Settings.AllowNewAccount = AccountCheckBox.Checked;
            Settings.AllowChangePassword = PasswordCheckBox.Checked;
            Settings.AllowLogin = LoginCheckBox.Checked;
            Settings.AllowNewCharacter = NCharacterCheckBox.Checked;
            Settings.AllowDeleteCharacter = DCharacterCheckBox.Checked;
            Settings.AllowStartGame = StartGameCheckBox.Checked;
            Settings.AllowCreateAssassin = AllowAssassinCheckBox.Checked;
            Settings.AllowCreateArcher = AllowArcherCheckBox.Checked;
            Settings.AllowObserve = ObserveCheckBox.Checked;

            if (int.TryParse(Resolution_textbox.Text, out tempint))
                Settings.AllowedResolution = tempint;

            Settings.SafeZoneBorder = SafeZoneBorderCheckBox.Checked;
            Settings.SafeZoneHealing = SafeZoneHealingCheckBox.Checked;
            Settings.GameMasterEffect = gameMasterEffect_CheckBox.Checked;
            if (int.TryParse(lineMessageTimeTextBox.Text, out tempint))
                Settings.LineMessageTimer = tempint;
            if (ushort.TryParse(maxConnectionsPerIP.Text, out tempshort))
                Settings.MaxIP = tempshort;
            Settings.ExpRate = (float)expRateInput.Value;
            Settings.DropRate = (float)dropRateInput.Value;
            Settings.RestedPeriod = Convert.ToInt32(tbRestedPeriod.Text);
            Settings.RestedBuffLength = Convert.ToInt32(tbRestedBuffLength.Text);
            Settings.RestedExpBonus = Convert.ToInt32(tbRestedExpBonus.Text);
            Settings.RestedMaxBonus = Convert.ToInt32(tbMaxRestedBonus.Text);
            
            // 保存语言设置
            string languageFile;
            switch (LanguageComboBox.SelectedIndex)
            {
                case 1: // 中文
                    languageFile = "Language.zh-CN.ini";
                    break;
                case 2: // 俄语
                    languageFile = "Language.ru-RU.ini";
                    break;
                case 3: // 韩语
                    languageFile = "Language.ko-KR.ini";
                    break;
                default: // 英语或其他
                    languageFile = "Language.ini";
                    break;
            }
            Settings.LanguageFilePath = Path.Combine(Settings.ConfigPath, languageFile);
        }

        private void IPAddressCheck(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            IPAddress temp;

            ActiveControl.BackColor = !IPAddress.TryParse(ActiveControl.Text, out temp) ? Color.Red : SystemColors.Window;
        }

        private void CheckUShort(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            ActiveControl.BackColor = !ushort.TryParse(ActiveControl.Text, out temp) ? Color.Red : SystemColors.Window;
        }

        private void VPathBrowseButton_Click(object sender, EventArgs e)
        {
            if (VPathDialog.ShowDialog() == DialogResult.OK)
            {
                VPathTextBox.Text = string.Join(",", VPathDialog.FileNames);
            }
        }

        private void Resolution_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            ActiveControl.BackColor = !int.TryParse(ActiveControl.Text, out temp) ? Color.Red : SystemColors.Window;

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void SafeZoneBorderCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SafeZoneHealingCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void HTTPIPAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ActiveControl.BackColor = !tryParseHttp() ? Color.Red : SystemColors.Window;
        }


        private void HTTPTrustedIPAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ActiveControl.BackColor = !tryParseTrustedHttp() ? Color.Red : SystemColors.Window;
        }

        bool tryParseHttp()
        {
            if ((HTTPIPAddressTextBox.Text.StartsWith("http://") || HTTPIPAddressTextBox.Text.StartsWith("https://")) && HTTPIPAddressTextBox.Text.EndsWith("/"))
            {
                return true;
            }
            return false;
        }

        bool tryParseTrustedHttp()
        {
            string pattern = @"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}";
            return Regex.IsMatch(HTTPTrustedIPAddressTextBox.Text, pattern);
        }

        private void StartHTTPCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.StartHTTPService = StartHTTPCheckBox.Checked;
        }

        private void expRateInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
                e.Handled = true;
        }

        private void dropRateInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
                e.Handled = true;
        }

        private void expRateInput_ValueChanged(object sender, EventArgs e)
        {
            expRateInput.Value = Math.Round(expRateInput.Value, 2);
        }

        private void dropRateInput_ValueChanged(object sender, EventArgs e)
        {
            dropRateInput.Value = Math.Round(dropRateInput.Value, 2);
        }

        private void tbRestedPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void tbRestedBuffLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void tbRestedExpBonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void tbMaxRestedBonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        #region Drop Adjuster
        private Envir Envir => SMain.EditEnvir;
        private void ProcessFiles(RequiredClass targetClass, bool comment)
        {
            string dropPath = Path.Combine(Application.StartupPath, "Envir", "Drops");

            if (!Directory.Exists(dropPath))
            {
                MessageBox.Show("Drops directory not found!");
                return;
            }

            try
            {
                var itemLookup = Envir.ItemInfoList.ToLookup(
                    i => i.Name.Trim(),
                    StringComparer.OrdinalIgnoreCase
                );

                int totalModified = 0;

                foreach (string filePath in Directory.GetFiles(dropPath, "*.txt", SearchOption.AllDirectories))
                {
                    var lines = File.ReadAllLines(filePath);
                    bool modified = false;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string originalLine = lines[i].Trim();
                        if (string.IsNullOrWhiteSpace(originalLine)) continue;

                        bool isCommented = originalLine.StartsWith(";");
                        string workingLine = isCommented ? originalLine.Substring(1).TrimStart() : originalLine;

                        string itemName = workingLine.Split()
                            .Select(part => part.Trim())
                            .FirstOrDefault(part => itemLookup.Contains(part));

                        if (string.IsNullOrEmpty(itemName)) continue;

                        foreach (ItemInfo item in itemLookup[itemName])
                        {
                            if (item.RequiredClass == RequiredClass.None) continue;
                            if (!item.RequiredClass.HasFlag(targetClass)) continue;

                            if (comment && !isCommented)
                            {
                                lines[i] = ";" + lines[i];
                                modified = true;
                                totalModified++;
                            }
                            else if (!comment && isCommented)
                            {
                                lines[i] = workingLine;
                                modified = true;
                                totalModified++;
                            }
                            break;
                        }
                    }

                    if (modified)
                    {
                        File.WriteAllLines(filePath, lines);
                    }
                }

                MessageBox.Show($"Processed files. Modified {totalModified} entries.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void RemoveSinDrops_Click(object sender, EventArgs e)
        {
            ProcessFiles(RequiredClass.Assassin, true);
        }

        private void ReaddSinDrops_Click(object sender, EventArgs e)
        {
            ProcessFiles(RequiredClass.Assassin, false);
        }

        private void RemoveArcDrops_Click(object sender, EventArgs e)
        {
            ProcessFiles(RequiredClass.Archer, true);
        }

        private void ReaddArcDrops_Click(object sender, EventArgs e)
        {
            ProcessFiles(RequiredClass.Archer, false);
        }
        #endregion
        
        #region Language Settings
        private void InitializeLanguageComboBox()
        {
            // 添加支持的语言
            LanguageComboBox.Items.Add("English");
            LanguageComboBox.Items.Add("Chinese");
            LanguageComboBox.Items.Add("Russian");
            LanguageComboBox.Items.Add("Korean");
            
            // 根据当前语言文件名确定选择的语言
            string currentLanguageFile = Path.GetFileName(Settings.LanguageFilePath);
            if (string.IsNullOrEmpty(currentLanguageFile) || currentLanguageFile.Equals("Language.ini", StringComparison.OrdinalIgnoreCase))
            {
                LanguageComboBox.SelectedIndex = 0; // 默认英语
            }
            else if (currentLanguageFile.Equals("Language.zh-CN.ini", StringComparison.OrdinalIgnoreCase))
            {
                LanguageComboBox.SelectedIndex = 1; // 中文
            }
            else if (currentLanguageFile.Equals("Language.ru-RU.ini", StringComparison.OrdinalIgnoreCase))
            {
                LanguageComboBox.SelectedIndex = 2; // 俄语
            }
            else if (currentLanguageFile.Equals("Language.ko-KR.ini", StringComparison.OrdinalIgnoreCase))
            {
                LanguageComboBox.SelectedIndex = 3; // 韩语
            }
            else
            {
                LanguageComboBox.SelectedIndex = 0; // 默认英语
            }
            
            // 添加选择变更事件
            LanguageComboBox.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
        }
        
        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string languageFile;
            
            switch (LanguageComboBox.SelectedIndex)
            {
                case 1: // 中文
                    languageFile = "Language.zh-CN.ini";
                    break;
                case 2: // 俄语
                    languageFile = "Language.ru-RU.ini";
                    break;
                case 3: // 韩语
                    languageFile = "Language.ko-KR.ini";
                    break;
                default: // 英语或其他
                    languageFile = "Language.ini";
                    break;
            }
            
            // 更新语言文件路径
            Settings.LanguageFilePath = Path.Combine(Settings.ConfigPath, languageFile);
            
            // 重新加载语言文件
            GameLanguage.LoadServerLanguage(Settings.LanguageFilePath);
            
            // 对所有打开的表单应用语言切换
            if (Application.OpenForms.Count > 0)
            {
                foreach (Form form in Application.OpenForms)
                {
                    // 先恢复到原始文本
                    UILanguageManager.RestoreOriginalTexts(form);
                    
                    // 如果不是英语，应用新语言
                    if (LanguageComboBox.SelectedIndex != 0)
                    {
                        UILanguageManager.LoadUILanguage(Settings.LanguageFilePath);
                        UILanguageManager.ApplyUILanguage(form);
                    }
                }
            }
            
            // 显示语言已更改的消息
            string languageName = "English";
            if (LanguageComboBox.SelectedIndex == 1)
                languageName = "Chinese";
            else if (LanguageComboBox.SelectedIndex == 2)
                languageName = "Russian";
            else if (LanguageComboBox.SelectedIndex == 3)
                languageName = "Korean";
                
            MessageBox.Show($"Language changed to {languageName}.", "Language Setting", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}
