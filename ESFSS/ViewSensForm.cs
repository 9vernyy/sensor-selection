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
using System.Drawing.Imaging;

namespace ESFSS
{
    public partial class ViewSensForm : Form
    {
        public ViewSensForm()
        {
            InitializeComponent();
        }

        private void ViewSensForm_Load(object sender, EventArgs e)
        {
                //Stream ms = null;
                // Создаем объект для соединения с базой, в качестве параметра вызываем функция getConnecttoDB
                MySqlConnection myConnection = new MySqlConnection(StatClass.getConnectToDb());
                // Sql скрипт для получения вышеуказанных столбцов из БД и внесения их в TextBox'ы панели изменения проекта
                MySqlCommand sqlComForSelect = new MySqlCommand(String.Format("SELECT s.sensName, s.sensCateg, s.sensType, " +
                    "s.measRange, s.outSignal, s.accuracy, s.bodyMat, s.applying, s.cost, s.quality, s.additInf, s.curr, " +
                    "s.measRangeSym, s.outSignalSym, s.sensClass, s.date, s.image, p.prodName, p.prodLocation, p.prodEmail" +
                    " FROM sensors s, producers p WHERE s.sensId={0} AND s.prodId=p.prodId",
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
                    sensCategTB.Text = datareader["sensCateg"].ToString();
                    sensTypeTB.Text = datareader["sensType"].ToString();
                    measRangeTB.Text = datareader["measRange"].ToString();
                    outSignalTB.Text = datareader["outSignal"].ToString();
                    accuracyTB.Text = datareader["accuracy"].ToString();
                    bodyMatTB.Text = datareader["bodyMat"].ToString();
                    applyingTB.Text = datareader["applying"].ToString();
                    costTB.Text = datareader["cost"].ToString();
                    qualityTB.Text = datareader["quality"].ToString();
                    additInfTB.Text = datareader["additInf"].ToString();
                    currTB.Text = datareader["curr"].ToString();
                    measRangeSymTB.Text = datareader["measRangeSym"].ToString();
                    outSignalSymTB.Text = datareader["outSignalSym"].ToString();
                    sensClassTB.Text = datareader["sensClass"].ToString();
                    dateTB.Text = datareader["date"].ToString();
                    prodNameTB.Text = datareader["prodName"].ToString();
                    prodLocTB.Text = datareader["prodLocation"].ToString();
                    prodContTB.Text = datareader["prodEmail"].ToString();
                    MemoryStream stream = new MemoryStream((byte[])datareader["image"]);
                    pictureBox1.Image = Image.FromStream(stream);
                }
                // Как только Sql запрос завершил выполнение
                datareader.Close();
                // Закрываем соединение с БД
                myConnection.Close();
                titleLb.Text = "Просмотр информации о датчике " + StatClass.getSensNameFMF();
                
        }

        // Событие возникающее при закрытии формы
        private void ViewSensForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Обнуляем ид проекта
            StatClass.setSensIdFMF(0);
            StatClass.setSensNameFMF("");
        }

        // Событие возникающее при клике на кнопку Ок
        private void okBtn_Click(object sender, EventArgs e)
        {
            // Закрываем окно
            this.Close();
        }
    }
}
