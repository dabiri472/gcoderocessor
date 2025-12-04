using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GCodeProcessor
{
    public partial class MainForm : Form
    {
        private string inputFile = string.Empty;
        private readonly string configFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GCodeProcessor", "config.json");
        private Config config;

        public MainForm()
        {
            InitializeComponent();
            this.Text = "G-Code Processor - پروسسور جی کد";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(750, 650);
            this.MinimumSize = new System.Drawing.Size(600, 550);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            LoadConfig();
            InitializeUI();
            this.FormClosing += MainForm_FormClosing;
        }

        private void LoadConfig()
        {
            try
            {
                if (File.Exists(configFile))
                {
                    string json = File.ReadAllText(configFile);
                    config = JsonSerializer.Deserialize<Config>(json) ?? new Config();
                }
                else
                {
                    config = new Config();
                }
            }
            catch
            {
                config = new Config();
            }
        }

        private void SaveConfig()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(configFile, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ذخیره تنظیمات: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InitializeUI()
        {
            this.Controls.Clear();
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 35);
            this.Font = new System.Drawing.Font("Segoe UI", 10);

            // Main TableLayoutPanel
            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(15),
                BackColor = System.Drawing.Color.FromArgb(30, 30, 35)
            };
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 75));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 160));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // 1. File Selection Section
            Panel fileContent;
            var filePanel = CreateSection("📁 انتخاب فایل", System.Drawing.Color.FromArgb(0, 122, 204), out fileContent);
            var fileTable = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            fileTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            fileTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            var selectFileBtn = new Button { Text = "📂 انتخاب فایل", Dock = DockStyle.Fill, BackColor = System.Drawing.Color.FromArgb(0, 122, 204), ForeColor = System.Drawing.Color.White, Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            selectFileBtn.FlatAppearance.BorderSize = 0;
            selectFileBtn.Click += SelectFileBtn_Click;
            var fileStatusLabel = new Label { Name = "fileStatusLabel", Text = "هیچ فایلی انتخاب نشده...", Dock = DockStyle.Fill, BackColor = System.Drawing.Color.FromArgb(50, 50, 55), ForeColor = System.Drawing.Color.LightGray, TextAlign = System.Drawing.ContentAlignment.MiddleRight, Padding = new Padding(10), Font = new System.Drawing.Font("Segoe UI", 10) };
            fileTable.Controls.Add(selectFileBtn, 0, 0);
            fileTable.Controls.Add(fileStatusLabel, 1, 0);
            fileContent.Controls.Add(fileTable);

            // 2. Mode Selection Section
            Panel modeContent;
            var modePanel = CreateSection("🔄 حالت پردازش", System.Drawing.Color.FromArgb(156, 39, 176), out modeContent);
            var modeCombo = new ComboBox { Name = "modeCombo", Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new System.Drawing.Font("Segoe UI", 11), BackColor = System.Drawing.Color.FromArgb(50, 50, 55), ForeColor = System.Drawing.Color.White };
            modeCombo.Items.AddRange(new[] { "📊 فقط محور X", "📊 فقط محور Y", "🔲 هر دو محور X و Y (شطرنجی)" });
            modeCombo.SelectedIndex = 0;
            modeCombo.SelectedIndexChanged += ModeCombo_SelectedIndexChanged;
            modeContent.Controls.Add(modeCombo);

            // 3. Parameters Section
            Panel paramContent;
            var paramPanel = CreateSection("⚙️ پارامترها", System.Drawing.Color.FromArgb(76, 175, 80), out paramContent);
            var paramTable = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3 };
            paramTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            paramTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            paramTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            paramTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            paramTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            paramTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            paramTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33));

            paramTable.Controls.Add(CreateLabel("Offset X (mm):"), 0, 0);
            paramTable.Controls.Add(CreateTextBox("offsetXBox", config.OffsetX.ToString()), 1, 0);
            paramTable.Controls.Add(CreateLabel("تعداد X:"), 2, 0);
            paramTable.Controls.Add(CreateTextBox("countXBox", config.CountX.ToString()), 3, 0);

            paramTable.Controls.Add(CreateLabel("Offset Y (mm):"), 0, 1);
            paramTable.Controls.Add(CreateTextBox("offsetYBox", config.OffsetY.ToString()), 1, 1);
            paramTable.Controls.Add(CreateLabel("تعداد Y:"), 2, 1);
            paramTable.Controls.Add(CreateTextBox("countYBox", config.CountY.ToString()), 3, 1);

            paramTable.Controls.Add(CreateLabel("فرمت خروجی:"), 0, 2);
            var formatCombo = new ComboBox { Name = "formatCombo", Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new System.Drawing.Font("Segoe UI", 10), BackColor = System.Drawing.Color.FromArgb(50, 50, 55), ForeColor = System.Drawing.Color.White };
            formatCombo.Items.AddRange(new[] { "cnc", "nc", "gcode", "txt" });
            formatCombo.SelectedItem = config.OutputFormat;
            paramTable.Controls.Add(formatCombo, 1, 2);
            paramContent.Controls.Add(paramTable);

            // 4. Process Button
            var processBtn = new Button { Text = "▶️  شروع پردازش", Dock = DockStyle.Fill, Font = new System.Drawing.Font("Segoe UI", 13, System.Drawing.FontStyle.Bold), BackColor = System.Drawing.Color.FromArgb(0, 200, 83), ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            processBtn.FlatAppearance.BorderSize = 0;
            processBtn.Click += ProcessBtn_Click;

            // 5. Log Section
            Panel logContent;
            var logPanel = CreateSection("📋 وضعیت و لاگ", System.Drawing.Color.FromArgb(255, 152, 0), out logContent);
            var logBox = new TextBox { Name = "logBox", Multiline = true, ReadOnly = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical, Font = new System.Drawing.Font("Consolas", 10), BackColor = System.Drawing.Color.FromArgb(25, 25, 30), ForeColor = System.Drawing.Color.FromArgb(0, 255, 150), BorderStyle = BorderStyle.None };
            logContent.Controls.Add(logBox);

            // Add to main table
            mainTable.Controls.Add(filePanel, 0, 0);
            mainTable.Controls.Add(modePanel, 0, 1);
            mainTable.Controls.Add(paramPanel, 0, 2);
            mainTable.Controls.Add(processBtn, 0, 3);
            mainTable.Controls.Add(logPanel, 0, 4);

            this.Controls.Add(mainTable);
            ModeCombo_SelectedIndexChanged(modeCombo, EventArgs.Empty);
            LogMessage("🚀 برنامه آماده است. یک فایل G-Code انتخاب کنید.");
        }

        private Panel CreateSection(string title, System.Drawing.Color headerColor, out Panel content)
        {
            var section = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, BackColor = System.Drawing.Color.FromArgb(40, 40, 45) };
            section.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            section.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            
            var header = new Label { Text = title, Dock = DockStyle.Fill, BackColor = headerColor, ForeColor = System.Drawing.Color.White, Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold), TextAlign = System.Drawing.ContentAlignment.MiddleRight, Padding = new Padding(0, 0, 10, 0) };
            content = new Panel { Dock = DockStyle.Fill, BackColor = System.Drawing.Color.FromArgb(40, 40, 45), Padding = new Padding(8) };
            
            section.Controls.Add(header, 0, 0);
            section.Controls.Add(content, 0, 1);
            return section;
        }

        private Label CreateLabel(string text)
        {
            return new Label { Text = text, Dock = DockStyle.Fill, ForeColor = System.Drawing.Color.FromArgb(200, 200, 200), Font = new System.Drawing.Font("Segoe UI", 10), TextAlign = System.Drawing.ContentAlignment.MiddleRight, Padding = new Padding(0, 0, 10, 0) };
        }

        private TextBox CreateTextBox(string name, string text)
        {
            return new TextBox { Name = name, Text = text, Dock = DockStyle.Fill, Font = new System.Drawing.Font("Segoe UI", 11), BackColor = System.Drawing.Color.FromArgb(50, 50, 55), ForeColor = System.Drawing.Color.White, BorderStyle = BorderStyle.FixedSingle };
        }

        private void ModeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var modeCombo = sender as ComboBox;
            var offsetXBox = this.Controls.Find("offsetXBox", true).FirstOrDefault() as TextBox;
            var offsetYBox = this.Controls.Find("offsetYBox", true).FirstOrDefault() as TextBox;
            var countXBox = this.Controls.Find("countXBox", true).FirstOrDefault() as TextBox;
            var countYBox = this.Controls.Find("countYBox", true).FirstOrDefault() as TextBox;
            
            // 0 = X only, 1 = Y only, 2 = Both (checkerboard)
            switch (modeCombo.SelectedIndex)
            {
                case 0: // X فقط
                    SetFieldEnabled(offsetXBox, true);
                    SetFieldEnabled(countXBox, true);
                    SetFieldEnabled(offsetYBox, false);
                    SetFieldEnabled(countYBox, false);
                    break;
                case 1: // Y فقط
                    SetFieldEnabled(offsetXBox, false);
                    SetFieldEnabled(countXBox, false);
                    SetFieldEnabled(offsetYBox, true);
                    SetFieldEnabled(countYBox, true);
                    break;
                case 2: // هر دو (شطرنجی)
                    SetFieldEnabled(offsetXBox, true);
                    SetFieldEnabled(countXBox, true);
                    SetFieldEnabled(offsetYBox, true);
                    SetFieldEnabled(countYBox, true);
                    break;
            }
        }

        private void SetFieldEnabled(TextBox box, bool enabled)
        {
            if (box == null) return;
            box.Enabled = enabled;
            if (enabled)
            {
                box.BackColor = System.Drawing.Color.FromArgb(50, 50, 55);
                box.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                box.BackColor = System.Drawing.Color.FromArgb(35, 35, 40);
                box.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            }
        }

        private void SelectFileBtn_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "G-Code Files (*.nc;*.gcode;*.cnc;*.txt)|*.nc;*.gcode;*.cnc;*.txt|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    inputFile = ofd.FileName;
                    config.LastInputFile = inputFile;
                    var fileStatusLabel = this.Controls.Find("fileStatusLabel", true).FirstOrDefault() as Label;
                    if (fileStatusLabel != null)
                        fileStatusLabel.Text = $"✅ {Path.GetFileName(inputFile)}";
                    LogMessage($"✅ فایل انتخاب شد: {Path.GetFileName(inputFile)}");
                }
            }
        }

        private void ProcessBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputFile) || !File.Exists(inputFile))
            {
                MessageBox.Show("لطفاً ابتدا فایل را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var offsetXBox = this.Controls.Find("offsetXBox", true).First() as TextBox;
                var offsetYBox = this.Controls.Find("offsetYBox", true).First() as TextBox;
                var countXBox = this.Controls.Find("countXBox", true).First() as TextBox;
                var countYBox = this.Controls.Find("countYBox", true).First() as TextBox;
                var modeCombo = this.Controls.Find("modeCombo", true).First() as ComboBox;
                var formatCombo = this.Controls.Find("formatCombo", true).First() as ComboBox;

                if (!double.TryParse(offsetXBox.Text, out double offsetX) || !double.TryParse(offsetYBox.Text, out double offsetY))
                {
                    MessageBox.Show("Offset نامعتبر است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(countXBox.Text, out int countX) || !int.TryParse(countYBox.Text, out int countY))
                {
                    MessageBox.Show("تعداد نامعتبر است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int mode = modeCombo.SelectedIndex;
                string format = formatCombo.SelectedItem.ToString();

                // Save config
                config.OffsetX = offsetX;
                config.OffsetY = offsetY;
                config.CountX = countX;
                config.CountY = countY;
                config.Mode = mode.ToString();
                config.OutputFormat = format;
                SaveConfig();

                ProcessGCode(offsetX, offsetY, countX, countY, mode, format);
            }
            catch (Exception ex)
            {
                LogMessage($"❌ خطا: {ex.Message}");
                MessageBox.Show($"خطا: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessGCode(double offsetX, double offsetY, int countX, int countY, int mode, string format)
        {
            try
            {
                var lines = File.ReadAllLines(inputFile);
                LogMessage($"✅ فایل خوانده شد: {lines.Length} خط");

                var xPattern = new Regex(@"([Xx])\s*([+-]?\d*\.?\d+)");
                var yPattern = new Regex(@"([Yy])\s*([+-]?\d*\.?\d+)");

                var outputLines = new List<string>();

                int actualCountX = (mode == 0 || mode == 2) ? countX : 1;
                int actualCountY = (mode == 1 || mode == 2) ? countY : 1;

                for (int iy = 0; iy < actualCountY; iy++)
                {
                    for (int ix = 0; ix < actualCountX; ix++)
                    {
                        double dx = (mode == 0 || mode == 2) ? offsetX * ix : 0.0;
                        double dy = (mode == 1 || mode == 2) ? offsetY * iy : 0.0;

                        if (ix > 0 || iy > 0)
                        {
                            outputLines.Add("G0 Z10.000");
                        }

                        foreach (var line in lines)
                        {
                            var stripped = line.Trim();
                            var processedLine = line;

                            if (!string.IsNullOrEmpty(stripped) && !stripped.StartsWith("%") && !stripped.StartsWith("(") && !stripped.StartsWith(";") && !stripped.StartsWith("N"))
                            {
                                if ((mode == 0 || mode == 2) && Math.Abs(dx) > 1e-6)
                                {
                                    processedLine = xPattern.Replace(processedLine, m =>
                                        $"{m.Groups[1].Value}{double.Parse(m.Groups[2].Value) + dx:F3}");
                                }

                                if ((mode == 1 || mode == 2) && Math.Abs(dy) > 1e-6)
                                {
                                    processedLine = yPattern.Replace(processedLine, m =>
                                        $"{m.Groups[1].Value}{double.Parse(m.Groups[2].Value) + dy:F3}");
                                }
                            }

                            outputLines.Add(processedLine);
                        }
                    }
                }

                if (!outputLines.TakeLast(10).Any(l => l.ToUpper().Contains("M02") || l.ToUpper().Contains("M30")))
                {
                    outputLines.Add("M05");
                    outputLines.Add("M30");
                }

                string inputDir = Path.GetDirectoryName(inputFile);
                string inputName = Path.GetFileNameWithoutExtension(inputFile);
                string outputPath = Path.Combine(inputDir, $"{inputName}_processed.{format}");

                File.WriteAllLines(outputPath, outputLines);

                string fullPath = Path.GetFullPath(outputPath);
                LogMessage($"✅ پردازش انجام شد!");
                LogMessage($"   بلوک‌ها: X={actualCountX}, Y={actualCountY}");
                LogMessage($"   Offset: X={offsetX}mm, Y={offsetY}mm");
                LogMessage($"📍 خروجی: {fullPath}");
                MessageBox.Show($"✅ موفق!\n\n📍 {fullPath}", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"❌ خطا در پردازش: {ex.Message}");
                throw;
            }
        }

        private void LogMessage(string message)
        {
            var logBox = this.Controls.Find("logBox", true).FirstOrDefault() as TextBox;
            if (logBox != null)
            {
                logBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }
    }
}