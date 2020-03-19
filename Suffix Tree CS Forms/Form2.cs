using System;
using System.IO;
using System.Management;
using System.Collections;
using System.Windows.Forms;

namespace Suffix_Tree_CS_Forms
{
    public partial class Form2 : Form
    {
        Hashtable logList = new Hashtable();    //таблица с логинами и паролями
        Stream myStream;    //поток, с помощью которого будут открываться файлы
        bool checkLog = false;  //проверка, вошел ли пользователь
        Form opener;    //форма, из которой была вызвана текущая, чтобы закрыть обе в случае закрытия первой

        public Form2(Form parent)
        {
            InitializeComponent();
            createLogList();    //создание таблицы с юзернеймами и паролями
            opener = parent;
        }

        private void createLogList()
        {
            var fileLogPath = "LogList.txt";                      //получить массив с юзернеймами и паролями

            using (myStream = File.Open(fileLogPath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(myStream);
                string line;
                while (!reader.EndOfStream)                          //добавить его в хэш таблицу
                {
                    line = reader.ReadLine();
                    string[] tempOne = line.Split(new char[] { '=' });
                    logList.Add(tempOne[0], tempOne[1]);
                }

                reader.Close();
                myStream.Close();
            }
        }
        

        private void logIn()    //функция логина
        {
            if (textBox1.Text != "")                                        //если поле юзернейма не пустое, то по файлу с юзернеймами устроить поиск по полю юзернейм
            {
                var fileLogPath = "LogList.txt";
                if (!File.Exists(fileLogPath))  //если файла с зареганными пользователями не существует, то текст ниже
                {
                    MessageBox.Show("Пользователь не зарегистрирован!");
                    return;
                }
                else
                {
                    if (logList.ContainsKey(textBox1.Text))
                    {
                        if (textBox2.Text == (String)logList[textBox1.Text])         //если поле с введенным юзернеймом имеется в хэш таблице, то сравнить введенный пароль с паролем в таблице
                        {
                            if (MessageBox.Show("Вы успешно авторизировались!") == DialogResult.OK)
                            {                                                                   //если пароли совпали, то написать пользователю, что она успешно залогинился
                                if (checkBox1.Checked)                                //если поставлена галочка в чекбоксе, то в файл с хвидами записать новый хвид
                                {
                                    string fileHWIDPath = "HWIDList.txt";

                                    using (myStream = File.Open(fileHWIDPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                    {
                                        StreamWriter writer = new StreamWriter(myStream);
                                        writer.WriteLine(GetHWID());

                                        writer.Close();
                                        myStream.Close();
                                    }
                                }
                                checkLog = true;    //указание того, что пользователь вошел

                                this.Dispose();
                            }
                        }
                        else
                            MessageBox.Show("Неверный пароль!");  //иначе сказать, что пароли не совпадают
                    }
                    else
                        MessageBox.Show("Пользователь не зарегистрирован!");   //иначе сказать, что юзер не существует
                }
            }

            textBox2.Text = "";     //очистить поле с паролем
        }

        private void createNewUser()    //функция создания нового пользователя
        {
            if (!logList.ContainsKey(textBox1.Text))                                    //если пользователь еще не существует
            {
                if (textBox1.Text.Length < 5)   //проверка длины никнейма
                {
                    MessageBox.Show("Слишком короткий логин (минимум 5 символов)");
                }
                else
                {
                    var fileLogPath = "LogList.txt";

                    using (myStream = File.Open(fileLogPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        StreamWriter writer = new StreamWriter(myStream);
                        writer.WriteLine(textBox1.Text + "=" + textBox2.Text);   //если да, то осуществляется добавление нового профиля в файл с полями юзернэйм и пароль
                        logList.Add(textBox1.Text, textBox2.Text);

                        MessageBox.Show("Вы успешно зарегистрировались!");

                        button1.Text = "Войти";                                            //автоматически форма переключается на вид формы для входа
                        button2.Text = "Регистрация";

                        checkBox1.Visible = true;

                        writer.Close();
                        myStream.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Пользователь уже существует"); //юзер уже существует
            }

            textBox2.Text = "";     //очистить поле с паролем
        }

        private string GetHWID()    //функция получения хвида
        {
            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorId"].ToString();
                break;
            }

            return id;
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Регистрация")                                     //если на второй кнопке неписано регистрация, то меняются надписи на кнопках 
            {
                button1.Text = "Создать";                                            //и теперь при нажатии на button1 будет происхдоить регистрация нового пользователя
                button2.Text = "Назад";

                checkBox1.Visible = false;                                //чекбокс будет скрыт
            }
            else
            {                                                                       //если надпись другая, то форма будет приведена в первоначальное состояние входа
                button1.Text = "Войти";
                button2.Text = "Регистрация";

                checkBox1.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Войти")   //если на клацнутой кнопке такой текст, то вызов функции логина пользователя
            {
                logIn();
            }
            else
            {
                createNewUser();    //иначе регистрации
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (checkLog == false)
            {
                if (MessageBox.Show("Вы уверены, что хотите выйти?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Dispose(); //если да, то все закрыть
                    opener.Dispose();
                }
                else
                {
                    e.Cancel = true;    //нет, то отменить закрытие
                }
            }
        }
    }
}
