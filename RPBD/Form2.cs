using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RPBD
{
    public partial class Form2 : Form
    {
        private DataSet dataset = new DataSet();

        public Form2()
        {
            InitializeComponent();
        }

        public DataSet Dataset
        {
            get { return dataset; }
        }

        public void LoadData(DataSet ds)
        {
            dataset.Tables.Add(ds.Tables["Здание"].Copy());
            dataset.Tables.Add(ds.Tables["Помещение"].Copy());
            dataset.Tables.Add(ds.Tables["Арендатор"].Copy());
            dataset.Tables.Add(ds.Tables["Аренда"].Copy());



            DataTable roomTable = new DataTable();
            var query = from building in dataset.Tables["Здание"].AsEnumerable()
                        join room in dataset.Tables["Помещение"].AsEnumerable()
                        on building.Field<int>("Код Здания") equals room.Field<int>("Код Здания")
                        select new
                        {
                            buildingCode = building.Field<int>("Код Здания"),
                            roomCode = room.Field<int>("Код Помещения"),
                            buildingName = building.Field<string>("Имя"),
                            buildingAddress = building.Field<string>("Адрес"),
                            roomName = room.Field<string>("Имя"),
                            roomSquare = room.Field<double>("Площадь")
                        };
            roomTable.Columns.Add("Код Здания", typeof(int));
            roomTable.Columns.Add("Код Помещения", typeof(int));
            roomTable.Columns.Add("Название здания", typeof(string));
            roomTable.Columns.Add("Адрес здания", typeof(string));
            roomTable.Columns.Add("Имя помещения", typeof(string));
            roomTable.Columns.Add("Площадь помещения", typeof(double));

            foreach (var item in query)
            {
                roomTable.Rows.Add(item.buildingCode, item.roomCode, item.buildingName, item.buildingAddress, item.roomName, item.roomSquare);
            }


            DataTable rentTable = new DataTable();
            var query1 = from room in roomTable.AsEnumerable()
                         join rent in dataset.Tables["Аренда"].AsEnumerable()
                         on room.Field<int>("Код Помещения") equals rent.Field<int>("Код Помещения")
                         join renter in dataset.Tables["Арендатор"].AsEnumerable()
                         on rent.Field<int>("Код арендатора") equals renter.Field<int>("Код арендатора")
                         select new
                         {
                             buildingName = room.Field<string>("Название здания"),
                             buildingAddress = room.Field<string>("Адрес здания"),
                             roomName = room.Field<string>("Имя помещения"),
                             roomSquare = room.Field<double>("Площадь помещения"),
                             renterName = renter.Field<string>("Название Фирмы"),
                             renterAddress = renter.Field<string>("Юридический Адрес"),
                             renterFIO = renter.Field<string>("ФИО"),
                             renterPhone = renter.Field<string>("Контактный телефон"),
                             rentNumber = rent.Field<string>("Номер Договора"),
                             rentNumberData = rent.Field<DateTime>("Дата оформления договора"),
                             rentStart = rent.Field<DateTime>("Дата начала аренды"),
                             rentEnd = rent.Field<DateTime>("Дата конца аренды")
                         };
            rentTable.Columns.Add("Название здания", typeof(string));
            rentTable.Columns.Add("Адрес здания", typeof(string));
            rentTable.Columns.Add("Имя помещения", typeof(string));
            rentTable.Columns.Add("Площадь помещения", typeof(double));
            rentTable.Columns.Add("Название фирмы", typeof(string));
            rentTable.Columns.Add("Юридический адрес фирмы", typeof(string));
            rentTable.Columns.Add("Фио руководителя", typeof(string));
            rentTable.Columns.Add("Контактный телефон", typeof(string));
            rentTable.Columns.Add("Номер Договора", typeof(string));
            rentTable.Columns.Add("Дата оформления договора", typeof(DateTime));
            rentTable.Columns.Add("Дата начала аренды", typeof(DateTime));
            rentTable.Columns.Add("Дата конца аренды", typeof(DateTime));
            foreach (var item in query1)
            {
                rentTable.Rows.Add(item.buildingName, item.buildingAddress, item.roomName, item.roomSquare, item.renterName, item.renterAddress, item.renterFIO, item.renterPhone, item.rentNumber, item.rentNumberData, item.rentStart, item.rentEnd);
            }
// Допиши тут еще лоад дату, а то не ебу как ее писать 
            switch (this.Text)
            {
                case "Здание":
                    {
                        this.dataGridView1.DataSource = dataset.Tables["Здание"];
                        this.dataGridView1.Columns["Код Здания"].Visible = false;
                    }
                    break;
                case "Помещение":
                    {
                        this.dataGridView1.DataSource = roomTable;
                        this.dataGridView1.Columns["Код Помещения"].Visible = false;
                        this.dataGridView1.Columns["Код Здания"].Visible = false;
                    }
                    break;
                case "Арендатор":
                    {
                        this.dataGridView1.DataSource = dataset.Tables["Арендатор"];
                        this.dataGridView1.Columns["Код арендатора"].Visible = false;
                    }
                    break;
                case "Аренда":
                    {
                        this.dataGridView1.DataSource = rentTable;
                        
                    }
                    break;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            switch (this.Text)
            {
                case "Здание":
                    {
                        BuildingInsert bi = new BuildingInsert();
                        
                        bi.ShowDialog();
                        if(bi.DialogResult == DialogResult.OK && bi.Imya!=String.Empty && bi.Address!= String.Empty)
                        {
                            DataRow newRow = dataset.Tables["Здание"].NewRow();
                            newRow["Имя"] = bi.Imya;
                            newRow["Адрес"] = bi.Address;
                            dataset.Tables["Здание"].Rows.Add(newRow);
                            dataGridView1.Refresh();
                        }
                    }break;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            switch (this.Text)
            {
                case "Здание":
                    {
                        BuildingEdit bi = new BuildingEdit();
                        DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                        int rowIndex = dataGridView1.Rows.IndexOf(selectedRow);
                        bi.SetFields(selectedRow.Cells["Имя"].Value.ToString(), selectedRow.Cells["Адрес"].Value.ToString());
                        if(dataGridView1.CurrentCell.OwningColumn.Name == "Адрес")
                        {
                            bi.SetFocus(dataGridView1.CurrentCell.OwningColumn.Name);
                        }
                        bi.ShowDialog();
                        if(bi.DialogResult == DialogResult.OK && bi.TextBox1Value!= String.Empty && bi.TextBox2Value!=String.Empty)
                        {
                            
                            dataset.Tables["Здание"].Rows[rowIndex]["Имя"] = bi.TextBox1Value;
                            dataset.Tables["Здание"].Rows[rowIndex]["Адрес"] = bi.TextBox2Value;
                            
                            

                            dataset.AcceptChanges();

                            

                            dataGridView1.Refresh();
                        }
                        else
                            if (bi.DialogResult == DialogResult.OK)
                            {
                                MessageBox.Show("Неверно введены данные!");
                            }
                    }
                    break;
                case "Помещение":
                    {
                        RoomEdit bi = new RoomEdit();
                        DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                        int rowIndex = dataGridView1.Rows.IndexOf(selectedRow);
                        bi.SetFields(selectedRow.Cells["Имя помещения"].Value.ToString(), selectedRow.Cells["Площадь помещения"].Value.ToString(), dataset.Tables["Здание"], selectedRow.Cells["Название здания"].Value.ToString(), selectedRow.Cells["Адрес здания"].Value.ToString());
                        bi.SetFocus(dataGridView1.CurrentCell.OwningColumn.Name);
                        bi.ShowDialog();
                    }
                    break;
                case "Арендатор":
                    {
                        RentorEdit RE = new RentorEdit();
                        DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                        int rowIndex = dataGridView1.Rows.IndexOf(selectedRow);
                        RE.SetFields(selectedRow.Cells["Название Фирмы"].Value.ToString(), selectedRow.Cells["Юридический Адрес"].Value.ToString(), selectedRow.Cells["ФИО"].Value.ToString(), selectedRow.Cells["Контактный телефон"].Value.ToString());
                        RE.SetFocus(dataGridView1.CurrentCell.OwningColumn.Name);

                        RE.ShowDialog();

                        if (RE.DialogResult == DialogResult.OK && RE.TextBox1Value != String.Empty && RE.TextBox2Value != String.Empty && RE.TextBox3Value != String.Empty && RE.TextBox4Value != String.Empty)
                        {

                            dataset.Tables["Арендатор"].Rows[rowIndex]["Название Фирмы"] = RE.TextBox1Value;
                            dataset.Tables["Арендатор"].Rows[rowIndex]["Юридический Адрес"] = RE.TextBox2Value;
                            dataset.Tables["Арендатор"].Rows[rowIndex]["ФИО"] = RE.TextBox3Value;
                            dataset.Tables["Арендатор"].Rows[rowIndex]["Контактный телефон"] = RE.TextBox4Value;



                            dataset.AcceptChanges();



                            dataGridView1.Refresh();
                        }
                        else
                            if (RE.DialogResult == DialogResult.OK)
                        {
                            MessageBox.Show("Неверно введены данные!");
                        }
                    }
                    break;
            }
        }
    }
}
