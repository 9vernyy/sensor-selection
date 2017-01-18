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
using System.IO;

namespace ESFSS
{
    public partial class ProdMainForm : Form
    {
        private bool flag = false;
        private string imgLoc = "";
        private byte[] byteArray = null;

        public ProdMainForm()
        {
            InitializeComponent();
        }

        // Событие возникающее при клике на кнопку выйти из уч. записи
        private void exitBtn_Click(object sender, EventArgs e)
        {
            // Закрываем эту форму
            this.Close();
        }

        // Событие возникающее при закрытии формы
        private void ProdMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AuthRegForm authRegForm = new AuthRegForm();
            // Выводим соответствующее сообщение
            DialogResult result = MessageBox.Show("Вы уверены что хотите выйти из учетной записи? ", "Выход из учетной записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Если юзер нажал да
            if (result == DialogResult.Yes)
            {
                authRegForm.Show();
                StatClass.setSensIdFMF(0);
                StatClass.setSensNameFMF("");
            }
            // Если юзер нажал нет
            else if (result == DialogResult.No)
                // То отменяем событие закрытии формы
                e.Cancel = true;
        }

        // Событие возникающее при загрузке формы
        private void ProdMainForm_Load(object sender, EventArgs e)
        {
            // Выводим производителя из БД в Label
            prodNameLb.Text = "Вы вошли как поставщик: " + StatClass.getUserOrProdName();

            StatClass.showDataToDGV(String.Format("SELECT sensId, sensName, sensType, applying, cost, quality, prodName, date FROM sensors WHERE prodId={0}", StatClass.getUserOrProdId()), dataGridView1);
            // Устанавливаем ширину 60 для столбца ид проекта
            dataGridView1.Columns[0].Width = 40;
            // Устанавливаем соответствующие имена столбцов датчика
            dataGridView1.Columns[0].HeaderText = "Id";
            dataGridView1.Columns[1].HeaderText = "Название датчика";
            dataGridView1.Columns[2].HeaderText = "Тип датчика";
            dataGridView1.Columns[3].HeaderText = "Область применения датчика";
            dataGridView1.Columns[4].HeaderText = "Цена";
            dataGridView1.Columns[5].HeaderText = "Качество";
            dataGridView1.Columns[6].HeaderText = "Поставщик";
            dataGridView1.Columns[7].HeaderText = "Дата/время";
            // Снимаем выделения с таблицы
            dataGridView1.ClearSelection();
        }

        // Событие возникающее при клике на кнопку добавить
        private void createSensBtn_Click(object sender, EventArgs e)
        {
            // Делает видимым панель добавления нового датчика
            newSensPan.Visible = true;
            titleLb.Text = "Добавление нового датчика в БД. Заполните все поля!";
            // Меняет название кнопки
            saveBtn.Text = "Добавить";
            // Меняет заголовок формы
            this.Text = "Добавления нового датчика в БД";
        }

        // Событие возникающее при клике на Label отмены
        private void cancelLb_Click(object sender, EventArgs e)
        {
            // Скрываем панель создания проекта
            newSensPan.Visible = false;
            // Очищаем текстбоксы/комбобоксы
            sensNameTB.Clear();
            applyingTB.Clear();
            sensCategCB.Text = "";
            sensTypeCB.Text = "";
            costTB.Clear();
            currCB.Text = "";
            qualityCB.Text = "";
            measRangeCB.Text = "";
            outSignalCB.Text = "";
            accuracyCB.Text = "";
            bodyMatCB.Text = "";
            additInfTB.Clear();

            flag = false;
            // Обнуляем ид датчика
            StatClass.setSensIdFMF(0);
            // Снимаем выделения с таблицы
            dataGridView1.ClearSelection();
            this.Text = "Экспертная система подбора датчиков BTS";
        }

        // Событие возникающее при клике на кнопку добавить
        private void saveBtn_Click(object sender, EventArgs e)
        {
            //Проверка целостности заполнения полей
            if (sensNameTB.Text == "" || applyingTB.Text == "" || sensCategCB.Text == ""
                || sensTypeCB.Text == "" || costTB.Text == "" || currCB.Text == ""
                || qualityCB.Text == "" || measRangeCB.Text == "" || measRangeSymCB.Text == ""
                || outSignalCB.Text == "" || outSignalSymCB.Text == "" || accuracyCB.Text == ""
                || bodyMatCB.Text == "" || pictureBox1.Image == null)
                MessageBox.Show("Одно из полей не заполнено!", "Пустое поле", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                // Если пользователь нажал на кнопку изменения проекта
                if (flag == true)
                {
                    using (Bitmap image = new Bitmap(imgLoc))
                    {
                        MemoryStream stream = new MemoryStream();
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        byteArray = stream.ToArray();
                    }
                    // Создаем объект подключения к БД, в качестве параметра вызываем функция getConnecttoDB
                    MySqlConnection myConnection = new MySqlConnection(StatClass.getConnectToDb());
                    string query = String.Format("UPDATE sensors SET sensName='{0}', sensCateg='{1}', " +
                        "sensType='{2}', measRange='{3}', outSignal='{4}', accuracy='{5}', bodyMat='{6}', " +
                        "applying='{7}', cost='{8}', quality='{9}', prodName='{10}', additInf='{11}', curr='{12}', measRangeSym='{13}', " +
                        "outSignalSym='{14}', sensClass='{15}', date='{16}', image=@image WHERE sensId={17}"
                    , sensNameTB.Text, sensCategCB.Text, sensTypeCB.Text, measRangeCB.Text, outSignalCB.Text,
                    accuracyCB.Text, bodyMatCB.Text, applyingTB.Text, costTB.Text, qualityCB.Text,
                    StatClass.getUserOrProdName(), additInfTB.Text, currCB.Text, measRangeSymCB.Text, outSignalSymCB.Text, sensClassCB.Text, Convert.ToString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()), StatClass.getSensIdFMF());
                    // Создаем объект выполнения Sql команды
                    MySqlCommand sqlCom = new MySqlCommand(query, myConnection);
                    MySqlParameter imageParameter = sqlCom.Parameters.Add("@image", MySqlDbType.LongBlob);//sqlCom.Parameters.Add("@image", SqlDbType.Binary);
                    imageParameter.Value = byteArray;
                    imageParameter.Size = byteArray.Length;
                    // Открываем соединение с БД
                    myConnection.Open();
                    // Выполняем Sql команду, которая ничего не возращает
                    sqlCom.ExecuteNonQuery();
                    // Закрываем соединение с БД
                    myConnection.Close();
                    
                    flag = false;
                    // Выводим сообщение о успешном изменении датчика
                    DialogResult result = MessageBox.Show("Информация о датчике " + sensNameTB.Text + " успешно изменена!", "Изменение информации о датчике", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Если пользователь нажмет ОК в окне сообщения
                    if (result == DialogResult.OK)
                        // Скрываем панель изменения проекта
                        newSensPan.Visible = false;
                }
                // Если пользователь нажал на создание нового проекта
                else if (flag == false)
                {
                    using (Bitmap image = new Bitmap(imgLoc))
                    {
                        MemoryStream stream = new MemoryStream();
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        byteArray = stream.ToArray();
                    }
                    // Создаем объект подключения к БД, в качестве параметра вызываем функция getConnecttoDB
                    MySqlConnection myConnection = new MySqlConnection(StatClass.getConnectToDb());
                    string query = String.Format("INSERT INTO sensors(sensId, prodId, " +
                        "sensName, sensCateg, sensType, measRange, outSignal, accuracy, bodyMat, " +
                        "applying, cost, quality, prodName, additInf, curr, measRangeSym, outSignalSym, sensClass, date, image)" +
                        " VALUES({0}, {1}, '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}'," +
                        "'{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', @image)"
                    , 0, StatClass.getUserOrProdId(), sensNameTB.Text, sensCategCB.Text, sensTypeCB.Text, measRangeCB.Text, outSignalCB.Text,
                    accuracyCB.Text, bodyMatCB.Text, applyingTB.Text, costTB.Text, qualityCB.Text,
                    StatClass.getUserOrProdName(), additInfTB.Text, currCB.Text, measRangeSymCB.Text, outSignalSymCB.Text, sensClassCB.Text, Convert.ToString(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));
                    // Создаем объект выполнения Sql команды
                    MySqlCommand sqlCom = new MySqlCommand(query, myConnection);
                    MySqlParameter imageParameter = sqlCom.Parameters.Add("@image", MySqlDbType.LongBlob);//sqlCom.Parameters.Add("@image", SqlDbType.Binary);
                    imageParameter.Value = byteArray;
                    imageParameter.Size = byteArray.Length;
                    // Открываем соединение с БД
                    myConnection.Open();
                    // Выполняем Sql команду, которая ничего не возращает
                    sqlCom.ExecuteNonQuery();
                    // Закрываем соединение с БД
                    myConnection.Close();
                    
                    // Выводим сообщение о успешном создании проекта
                    DialogResult result = MessageBox.Show("Датчик " + sensNameTB.Text + " успешно добавлен в БД!", "Добавление в БД", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Если пользователь нажмет ОК в окне сообщения
                    if (result == DialogResult.OK)
                        // Скрываем панель создания проекта
                        newSensPan.Visible = false;
                }
                // Выводим данные о проекте в таблицу
                StatClass.showDataToDGV(String.Format("SELECT sensId, sensName, sensType, applying, cost, quality, prodName, date FROM sensors WHERE prodId={0}", StatClass.getUserOrProdId()), dataGridView1);
                // Снимаем выделения с таблицы
                dataGridView1.ClearSelection();
                // Обнуляем ид проекта
                StatClass.setSensIdFMF(0);
            }

            // Очищаем текстбоксы/комбобоксы
            sensNameTB.Clear();
            applyingTB.Clear();
            sensCategCB.Text = "";
            sensTypeCB.Text = "";
            costTB.Clear();
            currCB.Text = "";
            qualityCB.Text = "";
            measRangeCB.Text = "";
            measRangeSymCB.Text = "";
            outSignalCB.Text = "";
            outSignalSymCB.Text = "";
            accuracyCB.Text = "";
            bodyMatCB.Text = "";
            sensClassCB.Text = "";
            additInfTB.Clear();
            pictureBox1.Image = null;
            this.Text = "Экспертная система подбора датчиков BTS";
            imgLoc = ""; 
        }

        // Событие возникающее при активации формы
        private void ProdMainForm_Activated(object sender, EventArgs e)
        {
            // Выводим в таблицу информацию о датчике
            StatClass.showDataToDGV(String.Format("SELECT sensId, sensName, sensType, applying, cost, quality, prodName, date FROM sensors WHERE prodId={0}", StatClass.getUserOrProdId()), dataGridView1);
            // Снимаем выделения с таблицы
            dataGridView1.ClearSelection();
        }

        // Событие возникающее при клике на кнопку изменить проект
        private void changeSensBtn_Click(object sender, EventArgs e)
        {
            // Если пользователь выбрал хоть один проект в таблице
            if (StatClass.getSensIdFMF() > 0)
            {
                // Создаем объект для соединения с базой, в качестве параметра вызываем функция getConnecttoDB
                MySqlConnection myConnection = new MySqlConnection(StatClass.getConnectToDb());
                // Sql скрипт для получения вышеуказанных столбцов из БД и внесения их в TextBox'ы панели изменения проекта
                MySqlCommand sqlComForSelect = new MySqlCommand(String.Format("SELECT sensName, sensCateg, sensType, " +
                    "measRange, outSignal, accuracy, bodyMat, applying, cost, quality, additInf, curr, measRangeSym, outSignalSym, sensClass, image" + 
                    " FROM sensors WHERE sensId={0}",
                            StatClass.getSensIdFMF()), myConnection);
                // Открываем соединение с БД
                myConnection.Open();
                // Создаем объект dataReader для выполнения Sql запроса, который возратит несколько значений
                MySqlDataReader datareader = sqlComForSelect.ExecuteReader();
                // Пока выполняется Sql запрос
                while (datareader.Read())
                {
                    // Извлекаем в текстбоксы соответствующую информацию о проекте
                    sensNameTB.Text = datareader["sensName"].ToString();
                    sensCategCB.Text = datareader["sensCateg"].ToString();
                    sensTypeCB.Text = datareader["sensType"].ToString();
                    measRangeCB.Text = datareader["measRange"].ToString();
                    outSignalCB.Text = datareader["outSignal"].ToString();
                    accuracyCB.Text = datareader["accuracy"].ToString();
                    bodyMatCB.Text = datareader["bodyMat"].ToString();
                    applyingTB.Text = datareader["applying"].ToString();
                    costTB.Text = datareader["cost"].ToString();
                    qualityCB.Text = datareader["quality"].ToString();
                    additInfTB.Text = datareader["additInf"].ToString();
                    currCB.Text = datareader["curr"].ToString();
                    measRangeSymCB.Text = datareader["measRangeSym"].ToString();
                    outSignalSymCB.Text = datareader["outSignalSym"].ToString();
                    sensClassCB.Text = datareader["sensClass"].ToString();
                    MemoryStream stream = new MemoryStream((byte[])datareader["image"]);
                    pictureBox1.Image = Image.FromStream(stream);
                }
                // Как только Sql запрос завершил выполнение
                datareader.Close();
                // Закрываем соединение с БД
                myConnection.Close();
                flag = true;
                // Отображаем панель изменения проекта
                newSensPan.Visible = true;
                titleLb.Text = "Изменение информации о датчике " + StatClass.getSensNameFMF() + ". Заполните все поля!";
                saveBtn.Text = "Изменить";
                this.Text = "Изменение информации о датчике " + StatClass.getSensNameFMF();
            }
        }

        // Событие возникающее при клике на кнопку удалить датчик
        private void deleteSensBtn_Click(object sender, EventArgs e)
        {
            // Если пользователь выбрал хоть один датчик в таблице
            if (StatClass.getSensIdFMF() > 0)
            {
                // То выводим сообщение о намерении удалить датчик
                DialogResult result = MessageBox.Show("Вы уверены что хотите удалить информацию о датчике " + StatClass.getSensNameFMF() + "?", "Удаление инфрмации о датчике", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // Если пользователь нажал на кнопку да, в сообщении
                if (result == DialogResult.Yes)
                {
                    // Sql скрипт для удаления записи из БД
                    StatClass.execNonQuery(String.Format("DELETE FROM sensors WHERE sensId={0}",
                            StatClass.getSensIdFMF()));

                    // Выводим в таблицу все датчики, кроме удаленного
                    StatClass.showDataToDGV(String.Format("SELECT sensId, sensName, sensType, applying, cost, quality, prodName, date FROM sensors WHERE prodId={0}", StatClass.getUserOrProdId()), dataGridView1);
                    // Снимаем выделения с таблицы
                    dataGridView1.ClearSelection();
                    // Обнуляем ид и имя датчика
                    StatClass.setSensIdFMF(0);
                    StatClass.setSensNameFMF("");
                }
                else
                    // Обнуляем ид
                    StatClass.setSensIdFMF(0);
            }
        }

        // Событие возникающее при клике по шапке таблицы
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Обнуляем ид и имя датчика с главной формы
            StatClass.setSensIdFMF(0);
            StatClass.setSensNameFMF("");
            // Снимаем выделения с таблицы
            dataGridView1.ClearSelection();
        }

        // Событие возникающее при клике по записи в таблице
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Записываем ид выбранного датчика
            StatClass.setSensIdFMF(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
            // Записываем имя выбранного датчика
            StatClass.setSensNameFMF(dataGridView1.CurrentRow.Cells[1].Value.ToString());
        }

        // Событие возникающее при наведении на надпись отмены
        private void cancelLb_MouseEnter(object sender, EventArgs e)
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

        // Событие возникающее при попытке ввести что нибудь в Combobox
        private void qualityCB_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Не даем юзеру вводить что нибудь в Combobox, т.к ему дается возможность выбрать валюту из списка
            e.Handled = true;
        }

        // Событие возникающее при попытке ввести что нибудь в Combobox
        private void currCB_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Не даем юзеру вводить что нибудь в Combobox, т.к ему дается возможность выбрать валюту из списка
            e.Handled = true;
        }

        // Событие возникающее при попытке ввести что нибудь в Textbox
        private void costTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            // По таблице ASCII символам от 0 до 9 эквивалентны 48-57 включительно. Для Backspace эквивалент 8.
            // Если пользоваель введет что то кроме цифр
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                // То не даем пользователю вводить в текстбокс что то крое цифр
                e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|All Files (*.)|*";
                dlg.Title = "Выберите изображение датчика";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    imgLoc = dlg.FileName.ToString();
                    pictureBox1.ImageLocation = imgLoc;
                }
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Событие возникающее при клике на кнопку о программе
        private void aboutProgBtn_Click(object sender, EventArgs e)
        {
            // Создаем объект класса AboutProgForm
            AboutProgForm aboutProg = new AboutProgForm();
            // И отображаем форму о программе модально
            aboutProg.ShowDialog();
        }

        // Событие возникающее при клике на кнопку справка
        private void helpBtn_Click(object sender, EventArgs e)
        {
            // Открываем файл Help.chm
            Help.ShowHelp(this, "Help.chm");
        }
    }
}
