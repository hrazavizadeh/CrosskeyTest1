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

        // fill all records available  in an arraylist to the datagridview
        private void FillGridView(ArrayList customerlist,DataGridView dgv)
        {
            DataGridViewRow dgvRow;
            foreach (customerLoan customer in customerlist)
            {
                dgvRow = new DataGridViewRow();
                dgvRow.CreateCells(dgv);
                dgvRow.Cells[0].Value = customer.Name;
                dgvRow.Cells[1].Value = customer.TotalLoan.ToString("###,###.##");
                dgvRow.Cells[2].Value = customer.Interest;
                dgvRow.Cells[3].Value = customer.Years;
                dgvRow.Cells[4].Value = customer.MonthlyPay.ToString("###,###.##");
                dgv.Rows.Add(dgvRow);
            }            
            
        }



        // Set DataGridView Column's Header from first Line of Imported file
        private void GridSettings(String colimnsHeaderText)
        {
            string[] title;
            int counter = 0;

            title = colimnsHeaderText.Split(',');            
            dgvCustomer.ColumnCount = title.Length+1;
            
            foreach (string t in title)
            {
                dgvCustomer.Columns[counter].HeaderText = title[counter];               
                counter++;
            }
            
            dgvCustomer.Columns[dgvCustomer.ColumnCount - 1].HeaderText = "Monthly Payment";
                        
        }




        private void mnuImport_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList customerList = new ArrayList();
                string filePath = "";
                string readline = "";
                bool isquotation = false;
                int pointer = 0;
                int startIndex = 0;
                int fieldIndex = 0;
                string[] recordFields;
                FileStream fileStream;
                StreamReader streamReader;

                dgvCustomer.Rows.Clear();
                openFileDialog1.ShowDialog();
                filePath = openFileDialog1.FileName;
                
                //Validating file existence  and file type of selected file
                if (File.Exists(filePath) && Path.GetExtension(filePath) == ".txt")
                {
                    //Open file as Read
                    fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    streamReader = new StreamReader(fileStream);
                    //Set pointer to read from start of file
                    streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

                    //read the first line and check the file contain valid data
                    readline = streamReader.ReadLine();
                    if (!readline.Substring(0, 9).Equals("Customer,"))
                        throw new System.ArgumentException("Wrong File Selected!");
                    
                    //if selected file is valis, set the gridview headers
                    GridSettings(readline);
                    
                    //start reading records from file till Last Record defined by "."
                    readline = streamReader.ReadLine();
                    while (readline != "." )
                    {
                        //if record is empty, escaped to next record
                        if (readline.Length == 0)
                        {
                            readline = streamReader.ReadLine();
                            continue;
                        }

                        startIndex = 0;
                        fieldIndex = 0;
                        recordFields = new string[4];
                        isquotation = false;
                        pointer = 0;
                        //serch till end of record
                        while (pointer < readline.Length)
                        {
                            //check every character in record until finding ','
                            //if before finding ',' it was a '"' in record, ignore the ',' until you find another '"'
                            while (pointer < readline.Length && (isquotation || readline[pointer] != ','))
                            {
                                //if current character is '"', change value of isquotation
                                if (readline[pointer] == '"') isquotation = !isquotation;
                                pointer++;

                            }
                            //extratct the found field by substring from record
                            recordFields[fieldIndex] = readline.Substring(startIndex, pointer - startIndex);
                            pointer++;
                            fieldIndex++;
                            //Set start Index to position of first character of next field
                            startIndex = pointer;
                        }
                        readline = streamReader.ReadLine();
                        //create a new object of CustomerLoan from extracted fields and add to an arraylist
                        customerList.Add(new customerLoan(recordFields[0].Replace("\"", ""), double.Parse(recordFields[1]), double.Parse(recordFields[2]), int.Parse(recordFields[3])));
                    }

                    streamReader.Close();
                    fileStream.Close();

                    //fill all records in arraylist to datagrid
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




        //Print all data in datagrid by using DGVPrinter Class
        private void mnuPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //check data available for print
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


        private void mnuClearGrid_Click(object sender, EventArgs e)
        {
            dgvCustomer.Rows.Clear();
        }


        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
