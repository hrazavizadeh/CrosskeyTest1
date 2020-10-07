using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using DGVPrinterHelper;

namespace CrosskeyExam
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }


        private void FillGridView(ArrayList alist,DataGridView dgv)
        {
            DataGridViewRow row;
            foreach (customerLoan cl in alist)
            {
                row = new DataGridViewRow();
                row.CreateCells(dgv);
                row.Cells[0].Value = cl.Name;
                row.Cells[1].Value = cl.TotalLoan.ToString("###,###.##");
                row.Cells[2].Value = cl.Interest;
                row.Cells[3].Value = cl.Years;
                row.Cells[4].Value = cl.MonthlyPay.ToString("###,###.##");
                dgv.Rows.Add(row);
            }            
            
        }


        private void GridSettings(String colimnsHeaderText)
        {
            string[] title;
            title = colimnsHeaderText.Split(',');
            dgvCustomer.ColumnCount = title.Length+1;
            int i = 0;
            foreach (string t in title)
            {
                dgvCustomer.Columns[i].HeaderText = title[i];
                //dgvCustomer.Columns[i].Width = 125;
                //dgvCustomer.Columns[i].ReadOnly = true;                
                i++;
            }

            dgvCustomer.Columns[dgvCustomer.ColumnCount - 1].HeaderText = "Monthly Payment";
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList customerList = new ArrayList();
                string fpath = "";
                string rline = "";
                bool kot = false;
                int loc = 0;
                int sindex = 0;
                int ifield = 0;
                string[] field;
                FileStream fs;
                StreamReader sr;
                dgvCustomer.Rows.Clear();
                openFileDialog1.ShowDialog();
                fpath = openFileDialog1.FileName;
                if (File.Exists(fpath) && Path.GetExtension(fpath) == ".txt")
                {
                    fs = new FileStream(fpath, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    rline = sr.ReadLine();
                    if (!rline.Substring(0, 9).Equals("Customer,"))
                        throw new System.ArgumentException("Wrong File Selected!");
                    GridSettings(rline);
                    rline = sr.ReadLine();
                    while (rline != "." && rline.Length > 0)
                    {

                        sindex = 0;
                        ifield = 0;
                        field = new string[4];
                        kot = false;
                        loc = 0;
                        while (loc < rline.Length)
                        {

                            while (loc < rline.Length && (kot || rline[loc] != ','))
                            {
                                if (rline[loc] == '"') kot = !kot;
                                loc++;

                            }
                            field[ifield] = rline.Substring(sindex, loc - sindex);
                            loc++;
                            ifield++;
                            sindex = loc;
                        }
                        rline = sr.ReadLine();
                        customerList.Add(new customerLoan(field[0].Replace("\"", ""), double.Parse(field[1]), double.Parse(field[2]), int.Parse(field[3])));
                    }

                    sr.Close();
                    fs.Close();

                    FillGridView(customerList, dgvCustomer);

                }
                else
                    throw new System.ArgumentException("Selected wrong File Type!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Crosskey",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCustomer.Rows.Count == 0)
                    MessageBox.Show("No data to print!");
                else
                {
                    DGVPrinter printer = new DGVPrinter();
                    printer.Title = "Crosskey Customer Report";
                    printer.TitleColor = Color.DarkGreen;
                    printer.SubTitle = string.Format("Date: {0}", DateTime.Now);
                    printer.SubTitleSpacing = 20;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Center;
                    printer.PrintDataGridView(dgvCustomer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





        private void clearGridToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dgvCustomer.Rows.Clear();
        }




    }
}
