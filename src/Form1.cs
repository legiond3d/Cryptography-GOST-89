using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace lab_2
{
    /*!
        \brief Класс, реализующий интерфейс программы с помощью WindowsForms
    
        Реализует обработчики событий, вызываемых пользовательским интерфейсом
    */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*!
            Обработчик события - нажатие на кнопку путь для шифрования
        */
        private void button1_Click(object sender, EventArgs e)//указание пути к файлу для шифрование
        {
            label4.Visible = false;
            using (var fileDlg = new OpenFileDialog())
            {
                fileDlg.Filter = "Text files(*.txt)|*.txt";
                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = fileDlg.FileName;
                }
                else
                    return;
            }
        }

        /*!
            Обработчик события - нажатие на кнопку путь для дешифрования
        */
        private void button2_Click(object sender, EventArgs e)//указание пути к файлу для расшифровки
        {
            label5.Visible = false;
            using (var fileDlg = new OpenFileDialog())
            {
                fileDlg.Filter = "Text files(*.txt)|*.txt";
                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    textBox3.Text = fileDlg.FileName;
                }
            }
        }

        /*!
            Обработчик события - изменение textBox с ключом шифра
            
            Заменяет все вводимые символы на знак *
        */
        private void textBox1_TextChanged(object sender, EventArgs e)//ввод ключа (пароля)
        {
            label3.Visible = false;
            textBox1.PasswordChar = '*';
        }

        /*!
            Обработчик события - нажатие на кнопку зашифровать

            Осуществляет проверку корректности ввода пользователя и создаёт объект класса для шифрования
        */
        private void button3_Click(object sender, EventArgs e)//шифрование файла
        {
            string path = textBox2.Text;
            string pass = textBox1.Text;
            if (path != null && pass != null && pass.Length == 16)
            {
                StreamReader sr = new StreamReader(path);
                string mes = sr.ReadToEnd();
                sr.Close();
                GOST_encoder gost = new GOST_encoder();
                byte[] res = gost.encode(mes, pass);
                SaveFileDialog filedlg = new SaveFileDialog();
                filedlg.Filter = "Text files(*.txt)|*.txt";
                if (filedlg.ShowDialog() == DialogResult.Cancel)
                    return;
                // получаем выбранный файл
                string filename = filedlg.FileName;
                // сохраняем текст в файл
                System.IO.File.WriteAllText(filename, Encoding.Unicode.GetString(res), Encoding.Unicode);
                MessageBox.Show("Файл успешно сохранен");
            }
            else
            {
                if (pass.Length != 16)
                {
                    label3.Text = "(Ключ должен состоять из 16 символов)";
                    label3.Visible = true;
                }
                if (pass == null)
                {
                    label3.Text = "(Введите ключ шифрования)";
                    label3.Visible = true;
                }
                if (path == null)
                {
                    label4.Visible = true;
                }
            }
        }

        /*!
            Обработчик события - нажатие на кнопку расшифровать

            Осуществляет проверку корректности ввода пользователя и создаёт объект класса для дешифрования
        */
        private void button4_Click(object sender, EventArgs e)//расшифровать
        {
            string pass = textBox1.Text;
            string path = textBox3.Text;
            if (path != null && pass != null && pass.Length == 16)
            {
                StreamReader sr = new StreamReader(path);
                string mes = sr.ReadToEnd();
                sr.Close();
                GOST_decoder gost = new GOST_decoder();
                byte[] res = gost.decode(mes, pass);
                SaveFileDialog filedlg = new SaveFileDialog();
                filedlg.Filter = "Text files(*.txt)|*.txt";
                if (filedlg.ShowDialog() == DialogResult.Cancel)
                    return;
                // получаем выбранный файл
                string filename = filedlg.FileName;
                // сохраняем текст в файл
                System.IO.File.WriteAllText(filename, Encoding.Unicode.GetString(res), Encoding.Unicode);
                MessageBox.Show("Файл успешно сохранен");
            }
            else
            {
                if (pass.Length != 16)
                {
                    label3.Text = "(Ключ должен состоять из 16 символов)";
                    label3.Visible = true;
                }
                if (pass == null)
                {
                    label3.Text = "(Введите ключ шифрования)";
                    label3.Visible = true;
                }
                if (path == null)
                {
                    label5.Visible = true;
                }
            }
        }
    }
}
