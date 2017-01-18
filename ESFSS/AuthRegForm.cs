using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ESFSS
{
    public partial class AuthRegForm : Form
    {
        private string dbName; // Переменная для хранения названия БД(таблица юзеров или производителей)
        private string column1; // ид
        private string column2; // имя
        private string column3; // фамилия или адрес
        private string column4; // email
        private string column5; // логин
        private string column6; // пароль

        public AuthRegForm()
        {
            InitializeComponent();
        }
        
        // Функция принимает название БД(юзеры или производители)
        private void setDbName(string value)
        {
            dbName = value;
        }

        // Функция возвращает название БД(юзеры или производители)
        private string getDbName()
        {
            return dbName;
        }

        // Функция принимает значение столбца ид
        private void setColumn1(string value)
        {
            column1 = value;
        }

        // Функция возвращает значение столбца ид
        private string getColumn1()
        {
            return column1;
        }

        // Функция принимает значение столбца имени юзера или производителя
        private void setColumn2(string value)
        {
            column2 = value;
        }

        // Функция возвращает значение столбца имени юзера или производителя
        private string getColumn2()
        {
            return column2;
        }

        // Функция принимает значение столбца фамилии юзера или адрес производителя
        private void setColumn3(string value)
        {
            column3 = value;
        }

        // Функция возвращает значение столбца фамилии юзера или адрес производителя
        private string getColumn3()
        {
            return column3;
        }

        // Функция принимает значение столбца email юзера или производителя
        private void setColumn4(string value)
        {
            column4 = value;
        }

        // Функция возвращает значение столбца email юзера или производителя
        private string getColumn4()
        {
            return column4;
        }

        // Функция принимает значение столбца логина юзера или производителя
        private void setColumn5(string value)
        {
            column5 = value;
        }

        // Функция возвращает значение столбца логина юзера или производителя
        private string getColumn5()
        {
            return column5;
        }

        // Функция принимает значение столбца пароля юзера или производителя
        private void setColumn6(string value)
        {
            column6 = value;
        }

        // Функция возвращает значение столбца пароля юзера или производителя
        private string getColumn6()
        {
            return column6;
        }

        // Функция определяющая был ли выбран режим пользователя
        private bool isUserSelected()
        {
            if (uOrPCB.Text == "Я - пользователь")
            {
                setDbName("users");
                setColumn1("userId");
                setColumn2("userName");
                setColumn3("userSurname");
                setColumn4("userEmail");
                setColumn5("userLogin");
                setColumn6("userPass");
                return true;
            }
            else return false;
        }

        // Функция определяющая был ли выбран режим производителя
        private bool isProducerSelected()
        {
            if (uOrPCB.Text == "Я - поставщик")
            {
                setDbName("producers");
                setColumn1("prodId");
                setColumn2("prodName");
                setColumn3("prodLocation");
                setColumn4("prodEmail");
                setColumn5("prodLogin");
                setColumn6("prodPass");
                return true;
            }
            else return false;
        }

        // Функция определяющая был ли выбран режим администратора
        private bool isAdminSelected()
        {
            if (uOrPCB.Text == "Я - администратор")
                return true;
            else return false;
        }

        // Функция для проверки логина на уникальность
        private bool isUniqueLogin()
        {
            int count; // Переменная длч счетчика
            // Sql скрипт сравнивает каждый логин в БД с логином введенный юзером
            // Этот скрипт возращает кол-во записей удовлетворяющих условию где логин в БД равен логину введенный юзером
            // Принимаем кол-во записей возращенный скриптом Sql
            count = Convert.ToInt32(StatClass.getExecScalarQuery(String.Format("SELECT COUNT(*) FROM " + getDbName() +
                " WHERE " + getColumn5() + "='{0}'",
                       login2TB.Text)));
            // Если кол-во записей больше 0 то
            if (count == 0)
                // Этот логин уникален
                return true;
            // Иначе он не уникален
            else return false;
        }


        // Событие возникающее при клике на кнопку регистрации
        private void signUpBtn_Click(object sender, EventArgs e)
        {
            if (isUserSelected())
            {
                nameLb.Text = "Имя:";
                surNLb.Text = "Фамилия:";
                emailLb.Text = "Email:";
                login2Lb.Text = "Логин:";
                pass2Lb.Text = "Пароль:";
                regLb.Text = "Регистрация пользователя:";
                // Отображаем панель регистрации
                regPan.Visible = true;
                
            }
            else if (isProducerSelected())
            {
                nameLb.Text = "Название поставщика:";
                surNLb.Text = "Адрес поставщика:";
                emailLb.Text = "Email или телефон:";
                login2Lb.Text = "Логин:";
                pass2Lb.Text = "Пароль:";
                regLb.Text = "Регистрация поставщика:";
                // Отображаем панель регистрации
                regPan.Visible = true;
            }
            else MessageBox.Show("Выберите режим входа/регистрации", "Не выбран режим входа/регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Событие возникающее при закрытии формы регистрации/входа
        private void AuthRegForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // При закрытии формы, закрывает приложение
            Application.Exit();
        }

        // Событие возникающее при клике на кнопку зарегистрироваться
        private void regBtn_Click(object sender, EventArgs e)
        {
            // Если поля не заполнены то
            if (nameTB.TextLength == 0 || surNTB.TextLength == 0 || emailTB.TextLength == 0 || login2TB.TextLength == 0
                || pass2TB.TextLength == 0)
                // Выводим сообщение об ошибке
                MessageBox.Show("Одно из полей не заполнено!", "Пустое поле", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Если длина логина меньше или равна 3 символам то
            else if (login2TB.TextLength <= 3)
                // Выводим сообщение об ошибке
                MessageBox.Show("Логин слишком короткий!", "Некорректный логин", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Если длина пароля меньше или равна 3 символам то
            else if (pass2TB.TextLength <= 3)
                // Выводим сообщение об ошибке
                MessageBox.Show("Пароль слишком короткий!", "Некорректный пароль", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Если логин не уникален
            else if (isUniqueLogin() == false)
                // То выводим сообщение о существовании такого логина
                MessageBox.Show("Такой логин уже существует!", "Некорректный логин", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Если юзер все заполнил нормально то выполняем код снизу
            else
            {
                // наш Sql запрос для записи в базу то что ввел пользователь в текстбоксы
                StatClass.execNonQuery(String.Format("INSERT INTO " + getDbName() +
                    "(" + getColumn1() + "," + getColumn2() + "," +
                    getColumn3() + "," + getColumn4() + "," + getColumn5() +
                    "," + getColumn6() + ") VALUES({0}, '{1}', '{2}', '{3}', '{4}', '{5}')"
                    , 0, nameTB.Text, surNTB.Text, emailTB.Text, login2TB.Text, pass2TB.Text));
                // Если юзер нажмет на Ок то
                if (MessageBox.Show("Регистрация успешно завершена!", "Успешная регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    // Очищаем наши текстбоксы
                    nameTB.Clear();
                    surNTB.Clear();
                    emailTB.Clear();
                    login2TB.Clear();
                    pass2TB.Clear();
                    // Скрываем панель регистрации
                    regPan.Visible = false;
                }
            }
        }

        // Событие возникающее при попытке ввести текст в ComboBox
        private void uOrPCB_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Не даем юзеру вводить что нибудь в Combobox, т.к ему дается возможность выбрать режим из списка
            e.Handled = true;
        }

        // Событие возникающее при клике на надпись отмены
        private void сancelLb_Click(object sender, EventArgs e)
        {
            // Очищаем наши текстбоксы
            nameTB.Clear();
            surNTB.Clear();
            emailTB.Clear();
            login2TB.Clear();
            pass2TB.Clear();
            // Скрываем панель регистрации
            regPan.Visible = false;
        }

        // Событие возникающее при наведении на надпись отмены
        private void сancelLb_MouseEnter(object sender, EventArgs e)
        {
            // Изменеяет цвет шрифта на бирюзовый
            cancelLb.ForeColor = Color.Teal;
            //Изменяет курсор на Hand
            cancelLb.Cursor = Cursors.Hand;
            // Изменяет стиль шрифта на подчеркнутый
            cancelLb.Font = new Font(cancelLb.Font, FontStyle.Underline);
        }

        // Событие возникающее при отведении курсора от надписи отмены
        private void cancelLb_MouseLeave(object sender, EventArgs e)
        {
            // Возращает на черный цвет
            cancelLb.ForeColor = SystemColors.ControlText;
            // Возращает стиль шрифта на простой
            cancelLb.Font = new Font(cancelLb.Font, FontStyle.Regular);
        }

        // Событие возникающее при клике на кнопку войти
        private void signInBtn_Click(object sender, EventArgs e)
        {
            if (isUserSelected() || isProducerSelected() || isAdminSelected())
            {
                // Создаем объект формы производителя
                ProdMainForm prodMainForm = new ProdMainForm();
                // Создаем объект формы пользователя
                UserMainForm userMainForm = new UserMainForm();
                // Создаем объект формы администратора
                AdminMainForm adminMainForm = new AdminMainForm();
                // Cоздаем объект для соединения с базой, в качестве параметра вызываем функция getConnecttoDB
                MySqlConnection myConnection = new MySqlConnection(StatClass.getConnectToDb());
                // Если поля логина или пароля не заполнены то
                if (loginTB.TextLength == 0 || passTB.TextLength == 0)
                {
                    // Выводим сообщение об ошибке
                    MessageBox.Show("Одно из полей не заполнено!", "Пустое поле", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Очищаем текстбоксы
                    loginTB.Clear();
                    passTB.Clear();
                }
                else
                {
                    try
                    {
                        // Sql скрипт для получения ид юзера или производителя. Этот скрипт сравнивает лог и пасс ввеедный юзером с записями в бд
                        MySqlCommand sqlComForId = new MySqlCommand(String.Format("SELECT " + getColumn1() + " FROM " +
                            getDbName() + " WHERE " + getColumn5() + "='{0}' AND " + getColumn6() + "='{1}'",
                            loginTB.Text, passTB.Text), myConnection);
                        // Открываем соединение с базой
                        myConnection.Open();
                        if(isUserSelected() || isProducerSelected())
                        {
                        // Если результат sql запроса с ид не пустой
                        if (sqlComForId.ExecuteScalar() != null)
                        {
                            // Sql скрипт для получения ид юзера или производителя. Этот скрипт сравнивает лог и пасс ввеедный юзером с записями в бд
                            StatClass.setUserOrProdId(Convert.ToInt32(StatClass.getExecScalarQuery(String.Format("SELECT " +
                                getColumn1() + " FROM " + getDbName() + " WHERE " + getColumn5() + "='{0}' AND " +
                            getColumn6() + "='{1}'", loginTB.Text, passTB.Text))));
                            // Sql скрипт для получения названия юзера или производителя. Этот скрипт сравнивает лог и пасс ввеедный юзером с записями в бд
                            StatClass.setUserOrProdName(StatClass.getExecScalarQuery(String.Format("SELECT " +
                                getColumn2() + " FROM " + getDbName() + " WHERE " + getColumn5() + "='{0}' AND " +
                            getColumn6() + "='{1}'", loginTB.Text, passTB.Text)));
                            // Sql скрипт для получения фамилии юзера. Этот скрипт сравнивает лог и пасс ввеедный юзером с записями в бд
                            StatClass.setUserSurname(StatClass.getExecScalarQuery(String.Format("SELECT " +
                                getColumn3() + " FROM " + getDbName() + " WHERE " + getColumn5() + "='{0}' AND " +
                            getColumn6() + "='{1}'", loginTB.Text, passTB.Text)));
                            // Скрываем эту форму
                            this.Hide();
                            // Если вошел производитель
                            if (isProducerSelected())
                                // Показываем форму производителя
                                prodMainForm.Show();
                            
                            // Иначе, если вошел пользователь
                            else if (isUserSelected())
                                // Показываем форму пользователя
                                userMainForm.Show();
                        }
                        else
                        {
                            // Если нет комбинаций лога и пасса в базе выводим ошибку
                            MessageBox.Show("Вы ввели неверный логин или пароль!", "Неверный логин или пароль", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // Очищаем текстбоксы
                            loginTB.Clear();
                            passTB.Clear();
                        }
                        }
                        // Если вошел администратор
                        else if (isAdminSelected() && (loginTB.Text == "admin" || loginTB.Text == "Admin") && (passTB.Text == "admin" || passTB.Text == "Admin"))
                        {
                            // Скрываем эту форму
                            this.Hide();
                            // И показываем форму администратора
                            adminMainForm.Show();
                        }
                        else
                        {
                            // Если нет комбинаций лога и пасса в базе выводим ошибку
                            MessageBox.Show("Вы ввели неверный логин или пароль!", "Неверный логин или пароль", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // Очищаем текстбоксы
                            loginTB.Clear();
                            passTB.Clear();
                        }
                    }
                    // Обрабатываем исключения
                    catch (Exception exc)
                    {
                        // Выводим сообщение исключения
                        MessageBox.Show(exc.Message);
                    }
                    finally
                    {
                        // Закрываем соединение с базой
                        myConnection.Close();
                    }


                }
            }
            else MessageBox.Show("Выберите режим входа/регистрации", "Не выбран режим входа/регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void просмотретьСправкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Help.chm");
        }

        private void просмотретьИнфюОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutProgForm aboutProg = new AboutProgForm();
            aboutProg.ShowDialog();
        }
    }
}

// Наш статичный класс. Его создали для того чтобы, передавать ид, имя, фамилию юзера, ид и названия датчиков между формами
public static class StatClass
{
    //Эти переменные статического класса
    private static int userOrProdId; // Переменная для хранения ид юзера или производителя
    private static string userOrProdName; // Переменная для хранения имени юзера или производителя
    private static string userSurname; // Переменная для хранения фамилии юзера
    private static string connStr; // Переменная для хранения строки с параметрами БД
    private static string resultOfQuery; // Переменная для хранния результата Sql запроса
    private static int sensIdFMF; // Переменная для хранения ид датчика с главной формы
    //private static int sensIdFCF; // Переменная для хранения ид датчика с формы сравнения проектов
    private static string sensNameFMF; // Переменная для хранения названия датчика с главной формы

    // Функция возращающая строку с параметрами БД
    public static string getConnectToDb()
    {
        string serverName = "localhost"; // Адрес сервера (для локальной базы "localhost")
        string userName = "admin"; // Имя пользователя
        string dbName = "esfssdb"; //Имя базы данных
        string port = "3306"; // Порт для подключения
        string password = "root"; // Пароль
        // Объеденяем всю информацию сверху в одну строку
        connStr = "server=" + serverName +
            ";user=" + userName +
            ";database=" + dbName +
            ";port=" + port +
            ";password=" + password + ";";
        // И возращаем эту строку
        return connStr;
    }

    // Функция для выполнения Sql запроса с результатом. Используется для извлечения данных из БД
    public static string getExecScalarQuery(string query)
    {
        // Создаем объект подключения к БД, в качестве параметра вызываем функция getConnecttoDB
        MySqlConnection myConnection = new MySqlConnection(getConnectToDb());
        // Создаем объект выполнения Sql команды
        MySqlCommand sqlCom = new MySqlCommand(query, myConnection);
        // Открываем соединение с БД
        myConnection.Open();
        // Записываем результат выполнения Sql команды в переменную
        resultOfQuery = Convert.ToString(sqlCom.ExecuteScalar());
        // Закрываем соединение с БД
        myConnection.Close();
        // Возращаем резульат нашего запроса
        return resultOfQuery;
    }

    // Функция для выполнения Sql запроса без результата. Используется для внесения или обновления данных в БД
    public static void execNonQuery(string query)
    {
        // Создаем объект подключения к БД, в качестве параметра вызываем функция getConnecttoDB
        MySqlConnection myConnection = new MySqlConnection(getConnectToDb());
        // Создаем объект выполнения Sql команды
        MySqlCommand sqlCom = new MySqlCommand(query, myConnection);
        // Открываем соединение с БД
        myConnection.Open();
        // Выполняем Sql команду, которая ничего не возращает
        sqlCom.ExecuteNonQuery();
        // Закрываем соединение с БД
        myConnection.Close();
    }

    // Функция для вывода данных из БД в DataGridView(таблицу)
    public static void showDataToDGV(string selectCommand, DataGridView dataGrid)
    {
        // Создаем объект подключения к БД, в качестве параметра вызываем функция getConnecttoDB
        MySqlConnection myConnection = new MySqlConnection(getConnectToDb());
        try
        {
            // Открываем соединение с БД
            myConnection.Open();
            // Создаем объект dataAdapter
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            // Наш dataAdapter хранит результат Sql запроса
            dataAdapter.SelectCommand = new MySqlCommand(selectCommand, myConnection);
            // Создаем объект table
            DataTable table = new DataTable();
            // Локализируем нашу таблицу
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            // Заполняем таблицу данными из нашего Sql запроса
            dataAdapter.Fill(table);
            // Создаем объект ресурса
            BindingSource bindingSource1 = new BindingSource();
            // Заполняем наш ресурс данными из таблицы
            bindingSource1.DataSource = table;
            // И выводим его в таблицу
            dataGrid.DataSource = bindingSource1;
            // Если в таблице нету проектов
            if (dataGrid.RowCount == 0)
                // Скрываем шапку таблицу
                dataGrid.ColumnHeadersVisible = false;
            // Иначе, если есть проекты в таблице
            else if (dataGrid.RowCount > 0)
                // Показываем шапку
                dataGrid.ColumnHeadersVisible = true;
        }
        catch (Exception exc)
        {
            // Выводим сообщение о необработанном исключении
            MessageBox.Show(exc.Message);
        }
        finally
        {
            // Закрываем соединение с БД
            myConnection.Close();
        }
    }

    // Функция принимает значение ид юзера
    public static void setUserOrProdId(int value)
    {
        userOrProdId = value;
    }

    // Функция для возращения ид юзера
    public static int getUserOrProdId()
    {
        return userOrProdId;
    }

    // Функция принимает имя юзера
    public static void setUserOrProdName(string value)
    {
        userOrProdName = value;
    }

    // Функция для возращения имени юзера
    public static string getUserOrProdName()
    {
        return userOrProdName;
    }

    // Функция принимает фамилию юзера
    public static void setUserSurname(string value)
    {
        userSurname = value;
    }

    // Функция для возращения фамилии юзера
    public static string getUserSurname()
    {
        return userSurname;
    }

    // Функция принимает значение ид проекта с главной формы
    public static void setSensIdFMF(int value)
    {
        sensIdFMF = value;
    }

    // Функция для возращения ид проекта с главной формы
    public static int getSensIdFMF()
    {
        return sensIdFMF;
    }

    // Функция принимает название проекта с главной формы
    public static void setSensNameFMF(string value)
    {
        sensNameFMF = value;
    }

    // Функция возращает название проекта с главной формы
    public static string getSensNameFMF()
    {
        return sensNameFMF;
    }
}