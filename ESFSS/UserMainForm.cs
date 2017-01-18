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
using Excel = Microsoft.Office.Interop.Excel;

namespace ESFSS
{
    public partial class UserMainForm : Form
    {
        private string sensCateg;
        private string sensType;
        private string cost;
        private string curr;
        private string quality;
        private string measRange;
        private string measRangeSym;
        private string outSignal;
        private string outSignalSym;
        private string accuracy;
        private string applying;
        private string sensClass;
        private string bodyMat;
        private string sensName;
        private string sensProd;
        private string sqlCommandForSelect;
        private Excel.Application excelApp; // Переменная приложения Excel
        private int i;

        public UserMainForm()
        {
            InitializeComponent();
        }

        // Событие возникающее при клике на кнопку просмотреть
        private void viewSensBtn_Click(object sender, EventArgs e)
        {
            ViewSensForm viewSensForm = new ViewSensForm();
            // Если пользователь выбрал хоть один проект в таблице
            if (StatClass.getSensIdFMF() > 0)
            {
                // Отображаем форму проекта
                viewSensForm.Text = "Просмотр информации о датчике " + StatClass.getSensNameFMF();
                viewSensForm.ShowDialog();
            }
        }
        
        // Событие возникающее при клике на кнопку выйти из уч. записи
        private void exitBtn_Click(object sender, EventArgs e)
        {
            // Закрываем эту форму
            this.Close();
        }

        // Событие возникающее при закрытии формы
        private void UserMainForm_FormClosing(object sender, FormClosingEventArgs e)
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
        private void UserMainForm_Load(object sender, EventArgs e)
        {
            // Выводим производителя из БД в Label
            userNameLb.Text = "Вы вошли как пользователь: " + StatClass.getUserSurname() + " " + StatClass.getUserOrProdName();
            // Снимаем выделения с таблицы
            dataGridView1.ClearSelection();

            // Создаем объект для соединения с базой, в качестве параметра вызываем функция getConnecttoDB
            MySqlConnection myConnection = new MySqlConnection(StatClass.getConnectToDb());
            // Sql скрипт для получения вышеуказанных столбцов из БД и внесения их в TextBox'ы панели изменения проекта
            MySqlCommand sqlComForSelect = new MySqlCommand(String.Format("SELECT DISTINCT prodName, applying, sensName FROM sensors"), myConnection);
            // Открываем соединение с БД
            myConnection.Open();
            // Создаем объект dataReader для выполнения Sql запроса, который возратит несколько значений
            MySqlDataReader datareader = sqlComForSelect.ExecuteReader();
            // Пока выполняется Sql запрос
            while (datareader.Read())
            {
                sensProdCB2.Items.Add(datareader["prodName"].ToString());//sensNameTB.Text = datareader["sensName"].ToString();
                applyingCB2.Items.Add(datareader["applying"].ToString());
                sensNameCB2.Items.Add(datareader["sensName"].ToString());
            }
            // Как только Sql запрос завершил выполнение
            datareader.Close();
            // Закрываем соединение с БД
            myConnection.Close();
            toolTip1.IsBalloon = true;
            toolTip1.SetToolTip(sensCatLb2, "Подсказка категории");
            toolTip1.SetToolTip(sensTypeLb2, "Подсказка типа");
            toolTip1.SetToolTip(costLb2, "Подсказка цены");
            toolTip1.SetToolTip(qualityLb2, "Подсказка качества датчика");
            toolTip1.SetToolTip(measRangeLb2, "Подсказка диапазона измерения");
            toolTip1.SetToolTip(outSignalLb2, "Подсказка вызходного сигнала");
            toolTip1.SetToolTip(accuracyLb2, "Подсказка точности впи");
            toolTip1.SetToolTip(sensNameLb2, "Подсказка кодового названия датчика");
            toolTip1.SetToolTip(applyingLb2, "Подсказка области применения датчика");
            toolTip1.SetToolTip(sensClassLb2, "Подсказка класса датчика");
            toolTip1.SetToolTip(bodyMatLb2, "Подсказка материала корпуса");
            toolTip1.SetToolTip(sensProdLb2, "Подсказка производителя");
        }

        // Событие возникающее при активации формы
        private void UserMainForm_Activated(object sender, EventArgs e)
        {
            // Снимаем выделения с таблицы
            dataGridView1.ClearSelection();
        }

        // Событие возникающее при клике на кнопку применить
        private void applyBtn_Click(object sender, EventArgs e)
        {
            if (sensCategChB.Checked == true || sensCategCB2.Text == "")
                sensCateg = "%";
            else
                sensCateg = sensCategCB2.Text;

            if (sensTypeChB.Checked == true || sensTypeCB2.Text == "")
                sensType = "%";
            else
                sensType = sensTypeCB2.Text;
            
            if (costChB.Checked == true || costTB2.Text == "" || currCB2.Text == "")
            {
                cost = "%";
                curr = "%";
            }
            else
            {
                cost = costTB2.Text;
                curr = currCB2.Text;
            }

            if (qualityChB.Checked == true || qualityCB2.Text=="")
                quality = "%";
            else quality = qualityCB2.Text;

            if (measRangeChB.Checked == true || measRangeCB2.Text == "" || measRangeSymCB2.Text == "")
            {
                measRange = "%";
                measRangeSym = "%";
            }
            else
            {
                measRange = measRangeCB2.Text;
                measRangeSym = measRangeSymCB2.Text;
            }

            if (outSignalChB.Checked == true || outSignalCB2.Text == "" || outSignalSymCB2.Text == "")
            {
                outSignal = "%";
                outSignalSym = "%";
            }
            else
            {
                outSignal = outSignalCB2.Text;
                outSignalSym = outSignalSymCB2.Text;
            }

            if (accuracyChB.Checked == true || accuracyCB2.Text == "")
                accuracy = "%";
            else accuracy = accuracyCB2.Text;

            if (sensNameChB.Checked == true || sensNameCB2.Text == "")
                sensName = "%";
            else sensName = sensNameCB2.Text;

            if (applyingChB.Checked == true || applyingCB2.Text == "")
                applying = "%";
            else applying = applyingCB2.Text;

            if (sensClassChB.Checked == true || sensClassCB2.Text == "")
                sensClass = "%";
            else sensClass = sensClassCB2.Text;

            if (bodyMatChB.Checked == true || bodyMatCB2.Text == "")
                bodyMat = "%";
            else bodyMat = bodyMatCB2.Text;

            if (sensProdChB.Checked == true || sensProdCB2.Text == "")
                sensProd = "%";
            else sensProd = sensProdCB2.Text;

            sqlCommandForSelect = String.Format("SELECT sensId, sensName, sensType, applying, cost, quality, prodName, date FROM sensors " +
                "WHERE sensName LIKE '{0}' AND sensCateg LIKE '{1}' AND " +
                        "sensType LIKE '{2}' AND measRange LIKE '{3}' AND outSignal LIKE '{4}' AND accuracy LIKE '{5}' AND bodyMat LIKE '{6}' AND " +
                        "applying LIKE '{7}' AND cost LIKE '{8}' AND quality LIKE '{9}' AND prodName LIKE '{10}' AND curr LIKE '{11}' AND measRangeSym LIKE '{12}' AND " +
                        "outSignalSym LIKE '{13}' AND sensClass LIKE '{14}'"
                    , sensName, sensCateg, sensType, measRange, outSignal,
                    accuracy, bodyMat, applying, cost, quality,
                    sensProd, curr, measRangeSym, outSignalSym, sensClass);
            StatClass.showDataToDGV(sqlCommandForSelect, dataGridView1);
            if (dataGridView1.RowCount > 0)
            {
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
                sensAvGB.Text = "Список подходящих датчиков из БД";
                // Снимаем выделения с таблицы
                dataGridView1.ClearSelection();
            }
            else sensAvGB.Text = "Увы, подходящие датчики в БД не обнаружены";
            // Очищаем текстбоксы/комбобоксы
            sensNameCB2.Text = "";
            applyingCB2.Text = "";
            sensCategCB2.Text = "";
            sensTypeCB2.Text = "";
            costTB2.Clear();
            currCB2.Text = "";
            qualityCB2.Text = "";
            measRangeCB2.Text = "";
            measRangeSymCB2.Text = "";
            outSignalCB2.Text = "";
            outSignalSymCB2.Text = "";
            accuracyCB2.Text = "";
            bodyMatCB2.Text = "";
            sensClassCB2.Text = "";
            sensProdCB2.Text = "";

            sensCategChB.Checked = false;
            costChB.Checked = false;
            qualityChB.Checked = false;
            measRangeChB.Checked = false;
            outSignalChB.Checked = false;
            accuracyChB.Checked = false;
            sensNameChB.Checked = false;
            applyingChB.Checked = false;
            sensClassChB.Checked = false;
            bodyMatChB.Checked = false;
            sensTypeChB.Checked = false;
            sensProdChB.Checked = false;

            sensTypeCB2.Enabled = false;
            sensTypeChB.Enabled = false;
            costTB2.Enabled = false;
            costChB.Enabled = false;
            currCB2.Enabled = false;
            qualityCB2.Enabled = false;
            qualityChB.Enabled = false;
            measRangeCB2.Enabled = false;
            measRangeChB.Enabled = false;
            measRangeSymCB2.Enabled = false;
            outSignalCB2.Enabled = false;
            outSignalChB.Enabled = false;
            outSignalSymCB2.Enabled = false;
            accuracyCB2.Enabled = false;
            accuracyChB.Enabled = false;
            sensNameCB2.Enabled = false;
            sensNameChB.Enabled = false;
            applyingCB2.Enabled = false;
            applyingChB.Enabled = false;
            sensClassCB2.Enabled = false;
            sensClassChB.Enabled = false;
            bodyMatCB2.Enabled = false;
            bodyMatChB.Enabled = false;
            sensProdCB2.Enabled = false;
            sensProdChB.Enabled = false;

            this.Text = "Экспертная система подбора датчиков BTS";
            selectSensPan.Visible = false;
        }

        // Событие возникающее при нажати на кнопку подобрать
        private void selectSensBtn_Click(object sender, EventArgs e)
        {
            // Делаем панель подбора видимым
            selectSensPan.Visible = true;
            titleLb2.Text = "Подбор датчика по критериям ";
            this.Text = "Подбор датчика по критериям";
        }

        // Событие возникающее при клике по записи в таблице
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Записываем ид выбранного датчика
            StatClass.setSensIdFMF(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));
            // Записываем имя выбранного датчика
            StatClass.setSensNameFMF(dataGridView1.CurrentRow.Cells[1].Value.ToString());
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

        private void sensCategChB_CheckedChanged(object sender, EventArgs e)
        {
            if (sensCategChB.Checked == true)
                sensCategCB2.Enabled = false;
            else
                sensCategCB2.Enabled = true;
            sensTypeCB2.Enabled = true;
            sensTypeChB.Enabled = true;
        }

        private void sensTypeChB_CheckedChanged(object sender, EventArgs e)
        {
            if (sensTypeChB.Checked == true)
                sensTypeCB2.Enabled = false;
            else
                sensTypeCB2.Enabled = true;
            costChB.Enabled = true;
            costTB2.Enabled = true;
            currCB2.Enabled = true;
        }

        private void costChB_CheckedChanged(object sender, EventArgs e)
        {
            if (costChB.Checked == true)
            {
                costTB2.Enabled = false;
                currCB2.Enabled = false;
            }
            else
            {
                costTB2.Enabled = true;
                currCB2.Enabled = true;
            }
            qualityChB.Enabled = true;
            qualityCB2.Enabled = true;

        }

        private void qualityChB_CheckedChanged(object sender, EventArgs e)
        {
            if (qualityChB.Checked == true)
                qualityCB2.Enabled = false;
            else
                qualityCB2.Enabled = true;
            measRangeChB.Enabled = true;
            measRangeCB2.Enabled = true;
            measRangeSymCB2.Enabled = true;
        }

        private void measRangeChB_CheckedChanged(object sender, EventArgs e)
        {
            if (measRangeChB.Checked == true)
            {
                measRangeCB2.Enabled = false;
                measRangeSymCB2.Enabled = false;
            }
            else
            {
                measRangeCB2.Enabled = true;
                measRangeSymCB2.Enabled = true;
            }
            outSignalChB.Enabled = true;
            outSignalCB2.Enabled = true;
            outSignalSymCB2.Enabled = true;
        }

        private void outSignalChB_CheckedChanged(object sender, EventArgs e)
        {
            if (outSignalChB.Checked == true)
            {
                outSignalCB2.Enabled = false;
                outSignalSymCB2.Enabled = false;
            }
            else
            {
                outSignalCB2.Enabled = true;
                outSignalSymCB2.Enabled = true;
            }
            accuracyChB.Enabled = true;
            accuracyCB2.Enabled = true;
        }

        private void accuracyChB_CheckedChanged(object sender, EventArgs e)
        {
            if (accuracyChB.Checked == true)
                accuracyCB2.Enabled = false;
            else
                accuracyCB2.Enabled = true;
            sensNameChB.Enabled = true;
            sensNameCB2.Enabled = true;
        }

        private void sensNameChB_CheckedChanged(object sender, EventArgs e)
        {
            if (sensNameChB.Checked == true)
                sensNameCB2.Enabled = false;
            else
                sensNameCB2.Enabled = true;
            applyingChB.Enabled = true;
            applyingCB2.Enabled = true;
        }

        private void sensClassChB_CheckedChanged(object sender, EventArgs e)
        {
            if (sensClassChB.Checked == true)
                sensClassCB2.Enabled = false;
            else
                sensClassCB2.Enabled = true;
            bodyMatChB.Enabled = true;
            bodyMatCB2.Enabled = true;
        }

        private void bodyMatChB_CheckedChanged(object sender, EventArgs e)
        {
            if (bodyMatChB.Checked == true)
                bodyMatCB2.Enabled = false;
            else
                bodyMatCB2.Enabled = true;
            sensProdChB.Enabled = true;
            sensProdCB2.Enabled = true;
        }

        private void sensProdChB_CheckedChanged(object sender, EventArgs e)
        {
            if (sensProdChB.Checked == true)
                sensProdCB2.Enabled = false;
            else
                sensProdCB2.Enabled = true;
        }

        private void applyingChB_CheckedChanged(object sender, EventArgs e)
        {
            if (applyingChB.Checked == true)
                applyingCB2.Enabled = false;
            else
                applyingCB2.Enabled = true;
            sensClassChB.Enabled = true;
            sensClassCB2.Enabled = true;
        }

        // Событие возникающее при клике на Label отмены
        private void cancelLb_Click(object sender, EventArgs e)
        {
            // Скрываем панель подбора датчика
            selectSensPan.Visible = false;

            // Очищаем текстбоксы/комбобоксы/чекбоксы
            sensNameCB2.Text = "";
            applyingCB2.Text = "";
            sensCategCB2.Text = "";
            sensTypeCB2.Text = "";
            costTB2.Clear();
            currCB2.Text = "";
            qualityCB2.Text = "";
            measRangeCB2.Text = "";
            measRangeSymCB2.Text = "";
            outSignalCB2.Text = "";
            outSignalSymCB2.Text = "";
            accuracyCB2.Text = "";
            bodyMatCB2.Text = "";
            sensClassCB2.Text = "";
            sensProdCB2.Text = "";

            sensCategChB.Checked = false;
            costChB.Checked = false;
            qualityChB.Checked = false;
            measRangeChB.Checked = false;
            outSignalChB.Checked = false;
            accuracyChB.Checked = false;
            sensNameChB.Checked = false;
            applyingChB.Checked = false;
            sensClassChB.Checked = false;
            bodyMatChB.Checked = false;
            sensTypeChB.Checked = false;
            sensProdChB.Checked = false;

            sensTypeCB2.Enabled = false;
            sensTypeChB.Enabled = false;
            costTB2.Enabled = false;
            costChB.Enabled = false;
            currCB2.Enabled = false;
            qualityCB2.Enabled = false;
            qualityChB.Enabled = false;
            measRangeCB2.Enabled = false;
            measRangeChB.Enabled = false;
            measRangeSymCB2.Enabled = false;
            outSignalCB2.Enabled = false;
            outSignalChB.Enabled = false;
            outSignalSymCB2.Enabled = false;
            accuracyCB2.Enabled = false;
            accuracyChB.Enabled = false;
            sensNameCB2.Enabled = false;
            sensNameChB.Enabled = false;
            applyingCB2.Enabled = false;
            applyingChB.Enabled = false;
            sensClassCB2.Enabled = false;
            sensClassChB.Enabled = false;
            bodyMatCB2.Enabled = false;
            bodyMatChB.Enabled = false;
            sensProdCB2.Enabled = false;
            sensProdChB.Enabled = false;

            this.Text = "Экспертная система подбора датчиков BTS";
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

        // Событие возникающее при клике на кнопку экспортировать
        private void exportSensBtn_Click(object sender, EventArgs e)
        {
            if (StatClass.getSensIdFMF() > 0)
            {
                // Выводим все данные о проекте из БД в таблицу
                StatClass.showDataToDGV(String.Format("SELECT * FROM sensors WHERE sensId={0}", StatClass.getSensIdFMF()), DGVFM);
                // Создаем объект приложения Excel
                excelApp = new Excel.Application();
                // Открываем приложения Excel и записываем в соответствующие ячейки данные о выбранном проекте
                excelApp.Visible = true;
                // Добавляем книгу
                excelApp.Workbooks.Add(Type.Missing);
                // Кол-во листов в книге = 1
                excelApp.SheetsInNewWorkbook = 1;
                // Лист
                Excel.Worksheet sheet = (Excel.Worksheet)excelApp.Sheets[1];
                // Столбец
                Excel.Worksheet column = (Excel.Worksheet)excelApp.Sheets[1];
                // Строка
                Excel.Worksheet rows = (Excel.Worksheet)excelApp.Sheets[1];
                // Название листа = Отчет
                sheet.Name = StatClass.getSensNameFMF();

                // Устанавливаем ширину столбцов
                column.Columns[1, Type.Missing].ColumnWidth = 40;
                column.Columns[2, Type.Missing].ColumnWidth = 40;
                column.Columns[3, Type.Missing].ColumnWidth = 3;
                column.Columns[4, Type.Missing].ColumnWidth = 40;
                column.Columns[5, Type.Missing].ColumnWidth = 40;
                // Форматирование стиля шрифта
                rows.Rows[1, Type.Missing].Font.Bold = true;
                rows.Rows[1, Type.Missing].Font.Size = 11;
                rows.Rows[2, Type.Missing].Font.Bold = true;
                rows.Rows[2, Type.Missing].Font.Size = 11;
                rows.Rows[4, Type.Missing].Font.Bold = true;
                rows.Rows[4, Type.Missing].Font.Size = 11;

                sheet.Cells[1, 1] = "Название датчика";
                sheet.Cells[2, 1] = "Поставщик датчика";

                sheet.Cells[1, 2] = DGVFM[2, 0].Value;
                sheet.Cells[2, 2] = DGVFM[12, 0].Value;

                sheet.Cells[4, 1] = "Основная информация о датчике";
                sheet.Cells[6, 1] = "Категория датчика";
                sheet.Cells[7, 1] = "Тип датчика";
                sheet.Cells[8, 1] = "Цена датчика(" + DGVFM[14, 0].Value + ")";
                sheet.Cells[9, 1] = "Качество датчика";
                sheet.Cells[6, 2].value = DGVFM[3, 0].Value;
                sheet.Cells[7, 2].value = DGVFM[4, 0].Value;
                sheet.Cells[8, 2].value = DGVFM[10, 0].Value + " " + DGVFM[14, 0].Value;
                sheet.Cells[9, 2].value = DGVFM[11, 0].Value;
                
                sheet.Cells[4, 4] = "Детальная информация о датчике";
                sheet.Cells[6, 4] = "Диапазон измерения(" + DGVFM[15, 0].Value + ")"; 
                sheet.Cells[7, 4] = "Выходной сигнал(" + DGVFM[16, 0].Value + ")";
                sheet.Cells[8, 4] = "Точность(% ВПИ)";
                sheet.Cells[9, 4] = "Кодовое название датчика";
                sheet.Cells[10, 4] = "Область применения датчика";
                sheet.Cells[11, 4] = "Класс датчика";
                sheet.Cells[12, 4] = "Материал корпуса";
                sheet.Cells[13, 4] = "Дата и время добав/измен датчика";
                

                sheet.Cells[6, 5].value = DGVFM[5, 0].Value;
                sheet.Cells[7, 5].value = DGVFM[6, 0].Value;
                sheet.Cells[8, 5].value = DGVFM[7, 0].Value;
                sheet.Cells[9, 5].value = DGVFM[2, 0].Value;
                sheet.Cells[10, 5].value = DGVFM[9, 0].Value;
                sheet.Cells[11, 5].value = DGVFM[17, 0].Value;
                sheet.Cells[12, 5].value = DGVFM[8, 0].Value;
                sheet.Cells[13, 5].value = DGVFM[19, 0].Value;
                
            }
        }

        private void aboutProgBtn_Click(object sender, EventArgs e)
        {
            AboutProgForm aboutProg = new AboutProgForm();
            aboutProg.ShowDialog();
        }

        // Событие возникающее при клике на кнопку справка
        private void helpBtn_Click(object sender, EventArgs e)
        {
            // Открываем файл Help.chm
            Help.ShowHelp(this, "Help.chm");
        }

        private void sensCategCB2_TextChanged(object sender, EventArgs e)
        {
            sensTypeCB2.Enabled = true;
            sensTypeChB.Enabled = true;
        }

        private void sensTypeCB2_TextChanged(object sender, EventArgs e)
        {
            costChB.Enabled = true;
            costTB2.Enabled = true;
            currCB2.Enabled = true;
        }

        private void costTB2_TextChanged(object sender, EventArgs e)
        {
            qualityChB.Enabled = true;
            qualityCB2.Enabled = true;
        }

        private void qualityCB2_TextChanged(object sender, EventArgs e)
        {
            measRangeChB.Enabled = true;
            measRangeCB2.Enabled = true;
            measRangeSymCB2.Enabled = true;
        }

        private void measRangeCB2_TextChanged(object sender, EventArgs e)
        {
            outSignalChB.Enabled = true;
            outSignalCB2.Enabled = true;
            outSignalSymCB2.Enabled = true;
        }

        private void outSignalCB2_TextChanged(object sender, EventArgs e)
        {
            accuracyChB.Enabled = true;
            accuracyCB2.Enabled = true;
        }

        private void accuracyCB2_TextChanged(object sender, EventArgs e)
        {
            sensNameChB.Enabled = true;
            sensNameCB2.Enabled = true;
        }

        private void sensNameCB2_TextChanged(object sender, EventArgs e)
        {
            applyingChB.Enabled = true;
            applyingCB2.Enabled = true;
        }

        private void applyingCB2_TextChanged(object sender, EventArgs e)
        {
            sensClassChB.Enabled = true;
            sensClassCB2.Enabled = true;
        }

        private void sensClassCB2_TextChanged(object sender, EventArgs e)
        {
            bodyMatChB.Enabled = true;
            bodyMatCB2.Enabled = true;
        }

        private void bodyMatCB2_TextChanged(object sender, EventArgs e)
        {
            sensProdChB.Enabled = true;
            sensProdCB2.Enabled = true;
        }

        private void costTB2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // По таблице ASCII символам от 0 до 9 эквивалентны 48-57 включительно. Для Backspace эквивалент 8.
            // Если пользоваель введет что то кроме цифр
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                // То не даем пользователю вводить в текстбокс что то крое цифр
                e.Handled = true;
        }

    }
}
