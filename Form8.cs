using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using System.Globalization;
using static System.Windows.Forms.AxHost;
//using WinFormsLibrary1;
//using ClassLibrary1;
using myLib4;

namespace AllLabs
{
    public partial class Form8 : Form
    {
        Form1 f1;
        public string s;
        public MatchCollection myMatch;
        public string signatura;
        public string fileText;
        public string fileName;

        public Form8()
        {
            InitializeComponent();
        }

        // Перекрытие конструктора для связи в первой форме
        public Form8(Form1 f)
        {
            InitializeComponent();
            f1 = f;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            // Установка режима автозаполнения для combobox
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            button2.Enabled = false;
            button1.Enabled = true;
            richTextBox1.ReadOnly = true;
        }

        // Открытие файла
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string fileName = openFileDialog1.FileName;
            fileText = System.IO.File.ReadAllText(fileName);
            button2.Enabled = true;
            richTextBox1.Text = fileText;
        }

        /*-------------------Режим ввода--------------------*/

        // Режим ввод с файла
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
            richTextBox1.ReadOnly = true;
        }

        // Режим ввод ручной
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = false;
            richTextBox1.ReadOnly = false;
        }

        /*-------------------Активные кнопки--------------------*/

        // Кнопка выделение включений
        private void button2_Click(object sender, EventArgs e)
        {
            // Выбор выделения цвета включения
            Random rnd = new Random();
            int R = rnd.Next(20, 255);
            int G = rnd.Next(20, 255);
            int B = rnd.Next(20, 255);
            foreach (Match m in myMatch)
            Parser.SetSelectionStyle(m.Index, m.Length, R, G, B, richTextBox1, signatura);
        }

        // Кнопка поиска
        private void button3_Click(object sender, EventArgs e)
        {            
            textBox1.Text = "";
            s = fileText;
            signatura = comboBox1.Text;

            // Проверка на то, что сигнатура не является спец символом
            string[] simbols = { ".", "\\", "$", "/" };
            bool containsString = false;
            foreach (string simbol in simbols)
            {
                if (signatura.Equals(simbol))
                {
                    containsString = true;
                    break;
                }
            }

            // Проверка ввода сигнатуры
            if (signatura != "" && !containsString)
            {
                // Сохранение сигнатуры в историю запросов
                comboBox1.Items.Add(signatura);
                Regex regex = new Regex(signatura);
                // Поиск совпадений и возвращает колекцию совпадений
                // Каждый элемент коллекции есть объект  Match
                MatchCollection matches = Regex.Matches(richTextBox1.Text, signatura, RegexOptions.IgnoreCase);
                myMatch = matches;

                textBox1.Text = "Все вхождения строки " + signatura + " в исходоном тексте: " + "\r\n";

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                        textBox1.Text += match.Index + "-ая позиция" + "\t" + match.Value + "\r\n";
                }
                else
                    textBox1.Text = "Совпадений не найдено";
                button2.Enabled = true;
            }
            else
                textBox1.Text = "Вы ввели не корректное значение сигнатуры!";
        }

        // Последовательный поиск сигнатур. Кнопка далее
        int start = 0;
        int R = 0; int G = 0; int B = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            s = richTextBox1.Text;
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionBackColor = Color.White; 
            if (comboBox1.Text != "")
            {
                signatura = comboBox1.Text;
                textBox1.Text = "";
                int index = s.IndexOf(signatura, start);

                // Генерация цвета для выделения
                Random r = new Random();
                if (start == 0)
                {
                    R = r.Next(0, 256);
                    G = r.Next(0, 256);
                    B = r.Next(0, 256);
                }

                if (index >= 0)
                {
                    Parser.SetSelectionStyleEach(index, signatura.Length, R, G, B, richTextBox1);
                    start = index + signatura.Length;
                }
                else
                {
                    start = 0;
                    textBox1.Text = "Поиск окончен. Для начала поиска сначала, нажмите снова на кнопку.";
                }
            }
        }

        // Открытие несколько файлов
        private void button6_Click(object sender, EventArgs e)
        {
            signatura = comboBox1.Text;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Text files (*.txt)|*.txt|All files(*.*)|*.*";
            openFile.Multiselect = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = "";
                string[] files = openFile.FileNames;
                string[] res = new string[files.Length];
                Stopwatch startTime = Stopwatch.StartNew();
                for (int i = 0; i < files.Length; i++)
                {
                    string s = File.ReadAllText(files[i]);
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    richTextBox1.Text += $"Файл №{i+1}\n\n"+ s + "\n\n";

                    Regex.Matches(s, signatura, RegexOptions.IgnoreCase);

                    stopwatch.Stop();
                    textBox1.Text += $"Парсинг файла {i + 1}" + " завершился за " + stopwatch.ElapsedMilliseconds + " миллисекунд \r\n";
                }
                startTime.Stop();
                textBox1.Text += "Парсинг всех файлов занял " + startTime.ElapsedMilliseconds + " миллисекунд" + "\r\n";
                // Найдено время парсинга всех файлов по отдельности и вместе
                // Весь текст перенесён в ричтекстбокс

                Regex regex = new Regex(signatura);
                MatchCollection matches = Regex.Matches(richTextBox1.Text, signatura, RegexOptions.IgnoreCase);
                myMatch = matches;

                textBox1.Text += "Все вхождения строки " + signatura + " в исходоном тексте: " + "\r\n";

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                        textBox1.Text += match.Index + "-ая позиция" + "\t" + match.Value + "\r\n";
                }
                else
                    textBox1.Text = "Совпадений не найдено";
                button2.Enabled = true;
            }
        }

        /*-------------------Доп и индивид--------------------*/

        /*В результате парссинга выделить в лог-журнале все включения со словоформами
        string без учета регистра, добавив к ним иным цветом свой электронный адрес, если эти
        вхождения, попали времени, введенный пользователем (от <чч.мм,сек> до <чч.мм,сек>)*/
        private void button4_Click(object sender, EventArgs e)
        {
            string s = richTextBox1.Text;
            string email = " dariadub17@gmail.com ";
            Match stringMatch = new Regex("string").Match(s);                

            Regex timeRegex = new Regex(@"([2][0-3]|[01]\d|\s\d):[0-5]\d:[0-5]\d");
            DateTime timeStart = dateTimePicker1.Value;
            DateTime timeEnd = dateTimePicker2.Value;

            if (timeEnd < timeStart) { MessageBox.Show("Некорректный временной интервал"); return; }
            Match timeMatch1 = timeRegex.Match(s);
            Match timeMatch2; // Для поиска начала следующей записи
            int counter = 0;
            while (timeMatch1.Success && stringMatch.Success)
            {
                // Проверка часов от 1 до 9
                string str = timeMatch1.Value;
                
                if (str[0] == ' ')
                {
                    str = str.Replace(' ', '0');
                }
                    
                DateTime time = DateTime.ParseExact(str, "HH:mm:ss", CultureInfo.InvariantCulture);
                if (time >= timeStart && time <= timeEnd)
                {
                    timeMatch2 = timeMatch1.NextMatch();
                    while (stringMatch.Success)
                    {
                        if (timeMatch2.Success && stringMatch.Index > timeMatch2.Index)
                            break;
                        if (stringMatch.Index > timeMatch1.Index && (stringMatch.Index < timeMatch2.Index || !timeMatch2.Success))
                        {
                            // stringMatch - подходит, с ним работаем
                            // иногда добавляет не туда
                            s = s.Insert(stringMatch.Index + stringMatch.Length + (counter * email.Length), email);
                            counter++;

                        }
                        stringMatch = stringMatch.NextMatch();
                    }
                }
                timeMatch1 = timeMatch1.NextMatch();
            }
            richTextBox1.Text = s;
            Regex regex = new Regex(email);
            MatchCollection matches = Regex.Matches(s, email, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                int p = match.Index;
                richTextBox1.Find(email, p, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.FromArgb(255, 0, 0);
            }
        }

        // Дополнительное задание
        // выделение дат от 29.11.2022 до 03.12.2022 и добавление к этим логам фамилии
        private void button7_Click(object sender, EventArgs e)
        {
            string s = richTextBox1.Text;
            string lastName = " Дубровина ";

            DateTime timeStart = DateTime.Parse("22.04.2023");
            DateTime timeEnd = DateTime.Parse("23.04.2023");

            Regex timeRegex = new Regex(@"([0-9]{2}).[0-9]{2}.[0-9]{4}");
            Match timeMatch1 = timeRegex.Match(s);
            int counter = 0;
            while (timeMatch1.Success)
            {
                DateTime time = DateTime.ParseExact(timeMatch1.Value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                if (time >= timeStart && time <= timeEnd)
                {
                    
                    s = s.Insert(timeMatch1.Index + timeMatch1.Length + (counter * lastName.Length), lastName);
                    counter++;
                }
                timeMatch1 = timeMatch1.NextMatch();
            }
            richTextBox1.Text = s;
            Regex regex = new Regex(lastName);
            MatchCollection matches = Regex.Matches(s, lastName, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                int p = match.Index;
                richTextBox1.Find(lastName, p, RichTextBoxFinds.None);
                richTextBox1.SelectionColor = Color.FromArgb(255, 0, 0);
            }
        }
    }
}
