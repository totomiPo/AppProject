using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AllLabs
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        public Form5(Form1 f)
        {
            InitializeComponent();
        }

        int[] arrayA; // массив
        int a; // размер массива
        private void button1_Click(object sender, EventArgs e)
        {

            numericUpDown1.Value = a;
            numericUpDown2.Value = 0;
            
            bool correct = true;

            // Ввод пользователем
            if (radioButton2.Checked == true)
            {
                int ind = 0;
                try
                {
                    for (int i = 0; i < a; i++)
                    {
                        int value;
                        if (int.TryParse(dataGridView1.Rows[0].Cells[i].Value.ToString(), out value))
                        {
                            arrayA[i] = value;
                            ind = i;
                        }
                    }
                }
                catch (Exception ex)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[ind];
                    label9.Text = "Введено некорректное\nзначение";
                    button2.Enabled = false;
                    correct = false;

                    // Вывод ошибок
                    // Использую using тк освобождает поток как только завершает действие
                    using (FileStream file = new FileStream(@"C:\Users\daria\OneDrive\Документы\2 курс\4 семестр\Программирование\Все лабораторные\AllLabs\exc.txt", FileMode.Append))
                    using (StreamWriter write = new StreamWriter(file))
                    {
                        write.WriteLine("* --------------------------------------------------------------------- *");
                        write.WriteLine(DateTime.Now.ToString() + " Лабораторная работа 2\n" + ex.ToString());
                    }
                    // Вывод на главную форму
                    Data.Exct += DateTime.Now.ToString() + " Лабораторная работа 2 " + ex.ToString() +'\n';
                }
            }

            /* Сформировать массив D той же размерности по правилу: первые 10 элементов - Di = Ai + i, 
             остальные - Di = Ai - i.Заменить минимальный по модулю положительный элемент 
            массива А нулем. Заменить элементы с k1-го по k2-й на обратные.*/
            if (correct)
            {
                float[] arrayD = new float[a];

                for (int i = 0; i < 10; i++)// если больше 0, то целая часть проверить
                {
                    // первые 10 элементов - Di = Ai + i
                    arrayD[i] = arrayA[i] + i;
                }

                for (int i = 10; i < a; i++)
                {
                    // остальные - Di = Ai - i
                    arrayD[i] = arrayA[i] - i;
                }
                // Заменить минимальный по модулю положительный элемент массива А нулем
                int minPositiveIndex = -1;
                int minPositiveValue = int.MaxValue;

                for (int i = 0; i < a; i++)
                {
                    int modElemArr = Math.Abs(arrayA[i]);
                    if (arrayA[i] > 0 && modElemArr < minPositiveValue)
                    {
                        minPositiveIndex = i;
                        minPositiveValue = arrayA[i];
                    }
                }
                if (minPositiveIndex != -1) { arrayD[minPositiveIndex] = 0; }

                // Заменить элементы с k1-го по k2-й на обратные
                int k1 = (int)numericUpDown3.Value;
                int k2 = (int)numericUpDown4.Value;
               
                if (k1 != k2 && k1 < k2)
                {
                    if (k2 < a && k1 >= 0 && k1 < a - 1 && k2 > 0)
                    {
                        label8.Text = "";
                        int ind = 0;
                        try
                        {
                            for (int i = k1; i <= k2; i++)
                            {
                                //if (arrayA[i] != 0)
                                //{
                                ind = i;
                                int exc = (1 / arrayA[i]);
                                arrayD[i] = (1 / (float)arrayA[i]);
                                //}
                            }
                            try
                            {
                                // Вывод массива результирующего
                                for (int i = 0; i < a; i++)
                                {
                                    dataGridView1.Rows[1].Cells[i].Value = arrayD[i];
                                    dataGridView1[i, 1].ReadOnly = true;
                                }

                                button2.Enabled = true;
                                button1.Enabled = false;
                            }
                            catch (Exception ex)
                            {
                                label10.Text = $"Выход за пределы массива!";
                                using (FileStream file = new FileStream(@"C:\Users\daria\OneDrive\Документы\2 курс\4 семестр\Программирование\Все лабораторные\AllLabs\exc.txt", FileMode.Append))
                                using (StreamWriter write = new StreamWriter(file))
                                {
                                    write.WriteLine("* --------------------------------------------------------------------- *");
                                    write.WriteLine(DateTime.Now.ToString() + " Лабораторная работа 2\n" + ex.ToString());
                                }
                                // Вывод на главную форму
                                Data.Exct += DateTime.Now.ToString() + " Лабораторная работа 2 " + ex.ToString() +'\n';
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            label10.Text = $"Нет обратного 0!";
                            using (FileStream file = new FileStream(@"C:\Users\daria\OneDrive\Документы\2 курс\4 семестр\Программирование\Все лабораторные\AllLabs\exc.txt", FileMode.Append))
                            using (StreamWriter write = new StreamWriter(file))
                            {
                                write.WriteLine("* --------------------------------------------------------------------- *");
                                write.WriteLine(DateTime.Now.ToString() + " Лабораторная работа 2\n" + ex.ToString());
                            }
                            // Вывод на главную форму
                            Data.Exct += DateTime.Now.ToString() + " Лабораторная работа 2 " + ex.ToString() + '\n';
                        }                        
                    }
                    else
                    {
                        numericUpDown3.Value = 0;
                        numericUpDown4.Value = 0;
                        label8.Text = $"k1: [0, {a - 2}]\nk2: [1, {a - 1}]";
                        button2.Enabled = false;
                        button1.Enabled = true;
                    }
                }
                else
                {
                    numericUpDown3.Value = 0;
                    numericUpDown4.Value = 0;
                    label8.Text = $"k1 должно быть меньше k2\nk1 не должно быть равно k2";
                    button2.Enabled = false;
                    button1.Enabled = true;
                }
            }

        }

        // Ввод массива
        private void button2_Click(object sender, EventArgs e)
        {
            // Ширина заголовка строки
            dataGridView1.RowHeadersWidth = 150;

            a = (int)numericUpDown1.Value;
            //numericUpDown1.Enabled = false;

            if (a < 10)
            {
                label4.Text = "Введите число\nбольшее 10!";
                numericUpDown1.Value = 0;
                numericUpDown1.Focus();
            }
            else
            {
                // Случайный ввод массива
                if (radioButton1.Checked == true)
                {
                    // Очистка таблицы от данных
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    // Запрет на изменение таблицы
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.AllowUserToDeleteRows = false;

                    button2.Enabled = false;
                    button1.Enabled = true;

                    label4.Text = "";
                    dataGridView1.ColumnCount = a;
                    Random rnd = new Random();
                    dataGridView1.Rows.Add();
                    arrayA = new int[a]; // сделай проверку выхода за пределы массива
                    for (int j = 0; j < a; j++)
                    {
                        dataGridView1.Rows[0].HeaderCell.Value = "Исходный массив";
                        dataGridView1.Columns[j].HeaderText = (j + 1).ToString();

                        int n = rnd.Next(-1000, 1000);
                        dataGridView1.Rows[0].Cells[j].Value = n;
                        // Блокировать редактирование значений в ячеек
                        dataGridView1[j, 0].ReadOnly = true;
                        arrayA[j] = n;
                    }

                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[1].HeaderCell.Value = "Результат";

                }

                // Ввод пользователем
                if (radioButton2.Checked == true)
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();

                    label4.Text = "";
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.AllowUserToDeleteRows = false;

                    button2.Enabled = false;
                    button1.Enabled = true;

                    dataGridView1.ColumnCount = a;
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[0].HeaderCell.Value = "Исходный массив";

                    arrayA = new int[a];

                    for (int i = 0; i < a; i++)
                    {
                        dataGridView1.Rows[0].HeaderCell.Value = "Исходный массив";
                        dataGridView1.Columns[i].HeaderText = (i + 1).ToString();

                        dataGridView1.Rows[0].Cells[i].Value = 0;
                        // Блокировать редактирование значений в ячеек
                        arrayA[i] = 0;
                    }
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[1].HeaderCell.Value = "Результат";
                }

                // Ввод с заданной частотой
                if (radioButton3.Checked == true)
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();

                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.AllowUserToDeleteRows = false;

                    button2.Enabled = false;
                    button1.Enabled = true;

                    label4.Text = "";
                    dataGridView1.ColumnCount = a;
                    dataGridView1.Rows.Add();
                    arrayA = new int[a];

                    for (int j = 0; j < a; j++)
                    {
                        dataGridView1.Columns[j].HeaderText = (j + 1).ToString();
                    }

                    // Задача вложенная в другую, выполняются независимо друг от друга (ассинхронность)
                    dataGridView1.ReadOnly = true;
                    var freq = Task.Factory.StartNew(() =>
                    {
                        dataGridView1.Rows[0].HeaderCell.Value = "Исходный массив";

                        Random rnd = new Random();
                        int f = (int)numericUpDown2.Value;
                        int del = rnd.Next(0, f);

                        for (int i = 0; i < a; i++)
                        {
                            Thread.Sleep(del * 1000);
                            arrayA[i] = rnd.Next(-10000, 10001);
                            dataGridView1.Rows[0].Cells[i].Value = arrayA[i];
                            
                            del = rnd.Next(0, f);
                        }

                    });
                    freq.Wait();
                    dataGridView1.ReadOnly = true;

                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[1].HeaderCell.Value = "Результат";

                    //for (int i = 0; i < a; i++)
                    //{
                    //    // оформление шапки таблицы
                    //    dataGridView1.Rows[0].HeaderCell.Value = "Исходный массив";
                    //    dataGridView1.Columns[i].HeaderText = (i + 1).ToString();
                    //
                    //    if (interval < 0) { interval--; }
                    //    else
                    //    {
                    //        // заполнение ячеек данными
                    //        arrayA[i] = rnd.Next(-10000, 10001);
                    //    }
                    //
                    //    dataGridView1.Rows[0].Cells[i].Value = arrayA[i];
                    //    // Блокировать редактирование значений в ячеек
                    //    dataGridView1[i, 0].ReadOnly = true;
                    //}

                    //int threshold = (a / f);
                    //for (int j = 0; j < a; j++)
                    //{
                    //    dataGridView1.Rows[0].HeaderCell.Value = "Исходный массив";
                    //    dataGridView1.Columns[j].HeaderText = (j + 1).ToString();
                    //    if (j % threshold == 0)
                    //    {
                    //        int n = rnd.Next(-10, 11);
                    //        dataGridView1.Rows[0].Cells[j].Value = n;
                    //        dataGridView1[j, 0].ReadOnly = true;
                    //        arrayA[j] = n;
                    //    }
                    //    else
                    //    {
                    //        int n = rnd.Next(-1000, 1000);
                    //        dataGridView1.Rows[0].Cells[j].Value = n;
                    //        dataGridView1[j, 0].ReadOnly = true;
                    //        arrayA[j] = n;
                    //    }
                    //
                    //}
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void Form5_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
