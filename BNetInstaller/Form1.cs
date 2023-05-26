using System.Diagnostics;
//using System.Reflection.Metadata;
using System;
using System.IO;
using System.Text.RegularExpressions;
//using System.Configuration;
//using System.Data.Common;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using BNetInstaller.Constants;
using BNetInstaller.Endpoints;
//using CommandLine;
//using System.Resources;
//using System.Reflection;
using System.Drawing;


namespace BNetInstaller
{
    public partial class Form1 : Form
    {

        string product = "osi";
        //string product = "fenrisb";
        string uid = "";
        string locale = "";
        bool isRepair = false;
        string dir = "/";
        Image lang_en = Properties.Resources.lang_en;
        Image lang_ru = Properties.Resources.lang_ru;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }
        private async void Form1_Load(object sender, EventArgs e)
        {

            // Чтение настроек
            bool isRussianSelected = Properties.Settings.Default.IsRussianSelected;
            bool isEnglishSelected = Properties.Settings.Default.IsEnglishSelected;

            // Применение выбранного варианта
            if (isRussianSelected)
            {
                ruToolStripMenuItem.Checked = true;
                engToolStripMenuItem.Checked = false;
                toolStripSplitButton1.Image = lang_ru;
            }
            else if (isEnglishSelected)
            {
                ruToolStripMenuItem.Checked = false;
                engToolStripMenuItem.Checked = true;
                toolStripSplitButton1.Image = lang_en;
            }

            if (!(ruToolStripMenuItem.Checked || engToolStripMenuItem.Checked))
            {
                ruToolStripMenuItem.Checked = true;
            }

            checkBox1.Checked = Properties.Settings.Default.CheckBox1State;
            string filePath = ".build.info";
            string lastVersion = GetLastVersionFromBuildInfo(filePath);
            string url = "http://eu.patch.battle.net:1119/" + product + "/versions";
            string version = await GetVersionFromBuildInfo(url);


            checkBox3.CheckedChanged += checkBox3_CheckedChanged; // Добавляем обработчик события
            engToolStripMenuItem.Click += engToolStripMenuItem_Click;
            ruToolStripMenuItem.Click += ruToolStripMenuItem_Click;
            CompareLabelsAndSetButtonAvailability();
            CompareLabelsAndSetButton1Availability();



            label2.Text = lastVersion;
            label4.Text = version;
            if (!File.Exists(filePath))
            {
                // Если файл отсутствует, устанавливаем текст кнопки "Установить"
                button2.Text = "Установить";
                button1.Enabled = false;
                checkBox1.Enabled = false;
            }

        }


        private async Task<string> GetVersionFromBuildInfo(string url)
        {
            string version = string.Empty;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string buildInfo = await response.Content.ReadAsStringAsync();

                        // Используем регулярное выражение для поиска числового значения
                        // в строке формата "цифры.цифры.цифры" (например, "1.6.74264")
                        Regex regex = new Regex(@"\d+\.\d+\.\d+");

                        // Ищем совпадения в строке
                        MatchCollection matches = regex.Matches(buildInfo);

                        // Если найдено хотя бы одно совпадение
                        if (matches.Count > 0)
                        {
                            // Получаем последнее найденное совпадение
                            Match lastMatch = matches[matches.Count - 1];

                            // Получаем значение совпадения
                            version = lastMatch.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка возможных ошибок HTTP-запроса, чтения ответа или работы с регулярными выражениями
                toolStripStatusLabel1.Text = "Ошибка: " + ex.Message;
            }

            return version;
        }

        private string GetLastVersionFromBuildInfo(string filePath)
        {
            string lastVersion = string.Empty;

            try
            {
                if (File.Exists(filePath))
                {
                    string buildInfo = File.ReadAllText(filePath);

                    // Используем регулярное выражение для поиска числового значения
                    // в строке формата "цифры.цифры.цифры" (например, "1.6.74264")
                    Regex regex = new Regex(@"\d+\.\d+\.\d+");

                    // Ищем совпадения в строке
                    MatchCollection matches = regex.Matches(buildInfo);

                    // Если найдено хотя бы одно совпадение
                    if (matches.Count > 0)
                    {
                        // Получаем последнее найденное совпадение
                        Match lastMatch = matches[matches.Count - 1];

                        // Получаем значение совпадения
                        lastVersion = lastMatch.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка возможных ошибок чтения файла или работы с регулярными выражениями
                toolStripStatusLabel1.Text = "Ошибка: " + ex.Message;
            }

            return lastVersion;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string pathToExecutable = "d2r.exe";
            string parameter1 = "-launch";
            string parameter2 = "";
            if (checkBox1.Checked)
            {
                parameter2 = "-sso";
            }

            Process process = new Process();
            process.StartInfo.FileName = pathToExecutable;
            process.StartInfo.Arguments = parameter1 + " " + parameter2;

            process.Start();
            // Завершение выполнения программы
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {


        }

        // Сохранение состояния checkBox1
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CheckBox1State = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CompareLabelsAndSetButtonAvailability();
            isRepair = checkBox3.Checked;
        }

        private void CompareLabelsAndSetButtonAvailability()
        {
            if (label4.Text != label2.Text || checkBox3.Checked)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }
        private void CompareLabelsAndSetButton1Availability()
        {
            if (label4.Text != label2.Text || checkBox3.Checked)
            {
                button1.Enabled = false;
                checkBox1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                checkBox1.Enabled = true;
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void GetLang()
        {
            if (ruToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.IsRussianSelected = true;
                Properties.Settings.Default.IsEnglishSelected = false;
            }
            else if (engToolStripMenuItem.Checked)
            {
                Properties.Settings.Default.IsRussianSelected = false;
                Properties.Settings.Default.IsEnglishSelected = true;
            }

            Properties.Settings.Default.Save();
        }

        private void ruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ruToolStripMenuItem.Checked = true;
            engToolStripMenuItem.Checked = false;
            toolStripSplitButton1.Image = lang_ru;
            locale = "ruRU";
            GetLang();
        }

        private void engToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ruToolStripMenuItem.Checked = false;
            engToolStripMenuItem.Checked = true;
            toolStripSplitButton1.Image = lang_en;
            locale = "enUS";
            GetLang();
        }

        private void ruToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ruToolStripMenuItem.Checked)
            {
                engToolStripMenuItem.Checked = false;
                toolStripSplitButton1.Image = lang_ru;
            }
        }

        private void engToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (engToolStripMenuItem.Checked)
            {
                ruToolStripMenuItem.Checked = false;
                toolStripSplitButton1.Image = lang_en;
            }
        }

        async Task<bool> ProgressLoop(ProductEndpoint endpoint)
        {
            static void Print(string label, object value) =>
                Console.WriteLine("{0,-20}{1,-20}", label, value);

            while (true)
            {
                var stats = await endpoint.Get();

                // check for completion
                var complete = stats.Value<bool?>("download_complete");
                if (complete == true)
                    return true;

                // get progress percentage and playability
                var progress = stats.Value<float?>("progress");
                var playable = stats.Value<bool?>("playable");

                if (!progress.HasValue)
                    return false;
                toolStripProgressBar1.Value = ((int)Math.Round(progress.Value * 100));
                //Console.SetCursorPosition(cursorLeft, cursorTop);
                //Print("Downloading:", options.Product);
                //Print("Language:", locale);
                //Print("Directory:", options.Directory);
                //Print("Progress:", progress.Value.ToString("P4"));
                //Print("Playable:", playable.GetValueOrDefault());
                await Task.Delay(2000);

                // exit @ 100%
                if (progress == 1f)
                    return true;
            }
        }

    }
}
