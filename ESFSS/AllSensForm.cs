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
    public partial class AllSensForm : Form
    {
        public AllSensForm()
        {
            InitializeComponent();
        }

        // Событие возникающее при клике на кнопку закрыть
        private void exitBtn_Click(object sender, EventArgs e)
        {
            // Закрываем эту форму
            this.Close();
        }

        // Событие возникающее при загрузке формы
        private void AllSensForm_Load(object sender, EventArgs e)
        {
            StatClass.showDataToDGV(String.Format("SELECT sensId, sensName, sensType, applying, cost, quality, prodName, date FROM sensors"), dataGridView1);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Записываем ид выбранного датчика
            StatClass.setSensIdFMF(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
            // Записываем имя выбранного датчика
            StatClass.setSensNameFMF(dataGridView1.CurrentRow.Cells[1].Value.ToString());
        }

        // Событие возникающее при клике на кнопку удалить датчик
        private void button1_Click(object sender, EventArgs e)
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
                    StatClass.showDataToDGV(String.Format("SELECT sensId, sensName, sensType, applying, cost, quality, prodName, date FROM sensors"), dataGridView1);
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

        // Событие возникающее при клике на кнопку просмотреть
        private void viewSensBtn_Click(object sender, EventArgs e)
        {
            ViewSensForm viewSensForm = new ViewSensForm();
            // Если пользователь выбрал хоть один проект в таблице
            if (StatClass.getSensIdFMF() > 0)
            {
                // Отображаем форму просмотра датчика
                viewSensForm.Text = "Просмотр информации о датчике " + StatClass.getSensNameFMF();
                // И показываем ее модально
                viewSensForm.ShowDialog();
            }
        }

        // Событие возникающее при активации формы
        private void AllSensForm_Activated(object sender, EventArgs e)
        {
            // Снимаем выделения с таблицы
            dataGridView1.ClearSelection();
        }
    }
}
