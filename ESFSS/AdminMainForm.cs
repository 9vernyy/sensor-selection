using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESFSS
{
    public partial class AdminMainForm : Form
    {
        private int userId;
        private int prodId;
        private string userName;
        private string prodName;

        public AdminMainForm()
        {
            InitializeComponent();
        }

        public void setUserId(int userId)
        {
            this.userId = userId;
        }

        public int getUserId()
        {
            return userId;
        }

        public void setProdId(int prodId)
        {
            this.prodId = prodId;
        }

        public int getProdId()
        {
            return prodId;
        }

        public void setUserName(string userName)
        {
            this.userName = userName;
        }

        public string getUserName()
        {
            return userName;
        }

        public void setProdName(string prodName)
        {
            this.prodName = prodName;
        }

        public string getProdName()
        {
            return prodName;
        }
        
        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            StatClass.showDataToDGV(String.Format("SELECT userId, userName, userSurname, userEmail, userLogin, userPass FROM users"), dataGridView1);
            // Устанавливаем ширину 60 для столбца ид проекта
            dataGridView1.Columns[0].Width = 40;
            // Устанавливаем соответствующие имена столбцов датчика
            dataGridView1.Columns[0].HeaderText = "Id";
            dataGridView1.Columns[1].HeaderText = "Имя";
            dataGridView1.Columns[2].HeaderText = "Фамилия";
            dataGridView1.Columns[3].HeaderText = "E-mail";
            dataGridView1.Columns[4].HeaderText = "Логин";
            dataGridView1.Columns[5].HeaderText = "Пароль";
            // Снимаем выделения с таблицы
            dataGridView1.ClearSelection();

            StatClass.showDataToDGV(String.Format("SELECT prodId, prodName, prodLocation, prodEmail, prodLogin, prodPass FROM producers"), dataGridView2);
            // Устанавливаем ширину 60 для столбца ид проекта
            dataGridView2.Columns[0].Width = 40;
            // Устанавливаем соответствующие имена столбцов датчика
            dataGridView2.Columns[0].HeaderText = "Id";
            dataGridView2.Columns[1].HeaderText = "Название поставщика";
            dataGridView2.Columns[2].HeaderText = "Адрес поставщика";
            dataGridView2.Columns[3].HeaderText = "Email или телефон";
            dataGridView2.Columns[4].HeaderText = "Логин";
            dataGridView2.Columns[5].HeaderText = "Пароль";
            // Снимаем выделения с таблицы
            dataGridView2.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            setUserId(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            setProdId(Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value));
        }

        // Событие возникающее при клике на кнопку удалить пользователя
        private void deleteUserBtn_Click(object sender, EventArgs e)
        {
            // Если пользователь выбрал хоть одного пользователя
            if (getUserId() > 0)
            {
                // То выводим сообщение о намерении удалить пользователя
                DialogResult result = MessageBox.Show("Вы уверены что хотите удалить пользователя " + getUserName() + "?", "Удаление пользователя", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // Если пользователь нажал на кнопку да, в сообщении
                if (result == DialogResult.Yes)
                {
                    // Sql скрипт для удаления записи из БД
                    StatClass.execNonQuery(String.Format("DELETE FROM users WHERE userId={0}",
                            getUserId()));

                    // Выводим в таблицу всех пользователей, кроме удаленного
                    StatClass.showDataToDGV(String.Format("SELECT userId, userName, userSurname, userEmail, userLogin, userPass FROM users"), dataGridView1);
                    // Снимаем выделения с таблицы
                    dataGridView1.ClearSelection();
                    // Обнуляем ид с главной формы, ид с формы сравнения и имя проекта
                    setUserId(0);
                    setUserName("");
                }
                else
                    // Обнуляем ид с главной формы, ид с формы сравнения и имя проекта
                    setUserId(0);
            }
        }

        // Событие возникающее при клике на кнопку удалить поставщика
        private void deleteProdBtn_Click(object sender, EventArgs e)
        {
            // Если пользователь выбрал хоть одного поставщика
            if (getProdId() > 0)
            {
                // То выводим сообщение о намерении удалить поставщика
                DialogResult result = MessageBox.Show("Вы уверены что хотите удалить постатвщика " + getProdName() + "?", "Удаление поставщика", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // Если пользователь нажал на кнопку да, в сообщении
                if (result == DialogResult.Yes)
                {
                    // Sql скрипт для удаления записи из БД
                    StatClass.execNonQuery(String.Format("DELETE FROM producers WHERE prodId={0}",
                            getProdId()));

                    // Выводим в таблицу всех поставщиков, кроме удаленного
                    StatClass.showDataToDGV(String.Format("SELECT prodId, prodName, prodLocation, prodEmail, prodLogin, prodPass FROM producers"), dataGridView2);
                    // Снимаем выделения с таблицы
                    dataGridView2.ClearSelection();
                    // Обнуляем ид с главной формы, ид с формы сравнения и имя проекта
                    setProdId(0);
                    setProdName("");
                }
                else
                    // Обнуляем ид с главной формы, ид с формы сравнения и имя проекта
                    setProdId(0);
            }
        }

        // Событие возникающее при клике на кнопку выйти из уч. записи
        private void exitBtn_Click(object sender, EventArgs e)
        {
            // Закрываем эту форму
            this.Close();
        }

        // Событие возникающее при закрытии формы
        private void AdminMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AuthRegForm authRegForm = new AuthRegForm();
            // Выводим соответствующее сообщение
            DialogResult result = MessageBox.Show("Вы уверены что хотите выйти из учетной записи? ", "Выход из учетной записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Если юзер нажал да
            if (result == DialogResult.Yes)
                authRegForm.Show();
            // Если юзер нажал нет
            else if (result == DialogResult.No)
                // То отменяем событие закрытии формы
                e.Cancel = true;
        }

        // Событие возникающее при клике на кнопку список всех датчиков
        private void allSensBtn_Click(object sender, EventArgs e)
        {
            // Создаем объект класса формы все датчики
            AllSensForm allSensForm = new AllSensForm();
            // И показываем ее модально
            allSensForm.ShowDialog();
        }
    }
}
