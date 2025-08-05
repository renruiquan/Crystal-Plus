using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Server
{
    public static class UILanguageManager
    {
        private static Dictionary<string, string> uiTexts = new Dictionary<string, string>();
        private static Dictionary<Control, string> originalTexts = new Dictionary<Control, string>();
        private static Dictionary<ToolStripItem, string> originalToolStripTexts = new Dictionary<ToolStripItem, string>();
        private static string currentLanguageFile = "";

        public static void LoadUILanguage(string languageIniPath)
        {
            if (!File.Exists(languageIniPath))
            {
                SaveUILanguage(languageIniPath);
                return;
            }

            currentLanguageFile = languageIniPath;
            uiTexts.Clear();

            // 读取语言文件
            var lines = File.ReadAllLines(languageIniPath);
            foreach (var line in lines)
            {
                if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Contains("="))
                {
                    var parts = line.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        uiTexts[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }
        }

        public static void SaveUILanguage(string languageIniPath)
        {
            // 这里可以保存默认的UI文本
            // 暂时不实现，因为我们需要先定义所有UI文本
        }

        public static string GetText(string key, string defaultValue = "")
        {
            if (uiTexts.ContainsKey(key))
                return uiTexts[key];
            
            return defaultValue;
        }

        public static void ApplyUILanguage(Form form)
        {
            // 清除之前的原始文本记录
            originalTexts.Clear();
            originalToolStripTexts.Clear();
            
            // 先保存所有控件的原始文本
            SaveOriginalTextsRecursive(form);
            
            // 然后应用翻译
            ApplyUILanguageRecursive(form);
        }

        private static void ApplyUILanguageRecursive(Control control)
        {
            // 处理当前控件
            if (control is TabPage tabPage)
            {
                string key = tabPage.Text;
                string translatedText = GetText(key, key);
                if (translatedText != key)
                    tabPage.Text = translatedText;
            }
            else if (control is Label label)
            {
                string key = label.Text;
                string translatedText = GetText(key, key);
                if (translatedText != key)
                    label.Text = translatedText;
            }
            else if (control is Button button)
            {
                string key = button.Text;
                string translatedText = GetText(key, key);
                if (translatedText != key)
                    button.Text = translatedText;
            }
            else if (control is CheckBox checkBox)
            {
                string key = checkBox.Text;
                string translatedText = GetText(key, key);
                if (translatedText != key)
                    checkBox.Text = translatedText;
            }
            else if (control is GroupBox groupBox)
            {
                string key = groupBox.Text;
                string translatedText = GetText(key, key);
                if (translatedText != key)
                    groupBox.Text = translatedText;
            }

            // 递归处理子控件
            foreach (Control child in control.Controls)
            {
                ApplyUILanguageRecursive(child);
            }

            // 处理MenuStrip
            if (control is MenuStrip menuStrip)
            {
                foreach (ToolStripItem item in menuStrip.Items)
                {
                    ApplyUILanguageRecursive(item);
                }
            }

            // 处理ListView的ColumnHeaders
            if (control is ListView listView)
            {
                foreach (ColumnHeader columnHeader in listView.Columns)
                {
                    string key = columnHeader.Text;
                    string translatedText = GetText(key, key);
                    if (translatedText != key)
                        columnHeader.Text = translatedText;
                }
            }
        }

        private static void ApplyUILanguageRecursive(ToolStripItem toolStripItem)
        {
            string key = toolStripItem.Text;
            string translatedText = GetText(key, key);
            if (translatedText != key)
                toolStripItem.Text = translatedText;

            // 处理下拉菜单
            if (toolStripItem is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem subItem in menuItem.DropDownItems)
                {
                    ApplyUILanguageRecursive(subItem);
                }
            }
        }

        public static void RestoreOriginalTexts(Form form)
        {
            RestoreOriginalTextsRecursive(form);
        }

        private static void RestoreOriginalTextsRecursive(Control control)
        {
            // 恢复当前控件的原始文本
            if (originalTexts.ContainsKey(control))
            {
                if (control is TabPage tabPage)
                    tabPage.Text = originalTexts[control];
                else if (control is Label label)
                    label.Text = originalTexts[control];
                else if (control is Button button)
                    button.Text = originalTexts[control];
                else if (control is CheckBox checkBox)
                    checkBox.Text = originalTexts[control];
                else if (control is GroupBox groupBox)
                    groupBox.Text = originalTexts[control];
            }

            // 递归处理子控件
            foreach (Control child in control.Controls)
            {
                RestoreOriginalTextsRecursive(child);
            }

            // 处理MenuStrip
            if (control is MenuStrip menuStrip)
            {
                foreach (ToolStripItem item in menuStrip.Items)
                {
                    RestoreOriginalTextsRecursive(item);
                }
            }
        }

        private static void RestoreOriginalTextsRecursive(ToolStripItem toolStripItem)
        {
            if (originalToolStripTexts.ContainsKey(toolStripItem))
            {
                toolStripItem.Text = originalToolStripTexts[toolStripItem];
            }

            // 处理下拉菜单
            if (toolStripItem is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem subItem in menuItem.DropDownItems)
                {
                    RestoreOriginalTextsRecursive(subItem);
                }
            }
        }

        public static void RefreshUILanguage()
        {
            if (!string.IsNullOrEmpty(currentLanguageFile))
            {
                LoadUILanguage(currentLanguageFile);
            }
        }

        private static void SaveOriginalTextsRecursive(Control control)
        {
            // 保存当前控件的原始文本
            if (control is TabPage tabPage)
            {
                originalTexts[control] = tabPage.Text;
            }
            else if (control is Label label)
            {
                originalTexts[control] = label.Text;
            }
            else if (control is Button button)
            {
                originalTexts[control] = button.Text;
            }
            else if (control is CheckBox checkBox)
            {
                originalTexts[control] = checkBox.Text;
            }
            else if (control is GroupBox groupBox)
            {
                originalTexts[control] = groupBox.Text;
            }

            // 递归处理子控件
            foreach (Control child in control.Controls)
            {
                SaveOriginalTextsRecursive(child);
            }

            // 处理MenuStrip
            if (control is MenuStrip menuStrip)
            {
                foreach (ToolStripItem item in menuStrip.Items)
                {
                    SaveOriginalTextsRecursive(item);
                }
            }
        }

        private static void SaveOriginalTextsRecursive(ToolStripItem toolStripItem)
        {
            originalToolStripTexts[toolStripItem] = toolStripItem.Text;

            // 处理下拉菜单
            if (toolStripItem is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem subItem in menuItem.DropDownItems)
                {
                    SaveOriginalTextsRecursive(subItem);
                }
            }
        }
    }
} 