﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CBSData;

namespace GUICBSData.MainScreen
{
    public partial class SelectTabelInformation : UserControl
    {
        private LocalCatalogTable _table;

        public SelectTabelInformation(DataCriteria data, LocalCatalogTable table )
        {
            InitializeComponent();


            this._table = table;
            this.CatogoriesList.Items.AddRange(data.Select
                .Where(x=>!string.IsNullOrEmpty(x))//belangerijk dit verwijdert lege items
                .Select(x=> new SelectValue(x) )
                .ToArray()
                );

            //trackbar
            this.LimitTo.Text = data.Limit.ToString();
            this.trackBar1.Maximum = data.Limit;
            this.trackBar1.Minimum = 1;

            if (data.Limit > 500)
            {
                this.trackBar1.Value = 500;
                this.limit.Text = "500";
            }
            if(data.Limit>10000)
            {
                this.LimitTo.Text = "10000";
                this.trackBar1.Maximum = 10000;
            }
        }

        private SelectValue deleteNumber(string x)
        {
            return new SelectValue(x);
        }

        private void GetData_Click(object sender, EventArgs e)
        {

            var t = this._table;

            if(t!=null && !this.DataTransfer.IsBusy)
            {
                var AllData = new { T = t, limit = trackBar1.Value, select = this.CatogoriesList };
                this.DataTransfer.RunWorkerAsync(AllData);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.limit.Text = this.trackBar1.Value.ToString();
        }

        private void DataTransfer_DoWork(object sender, DoWorkEventArgs e)
        {
            MessageBox.Show("De data wordt opgehaald en naar Excel geschreven dit kan even duren afhankelijk van de hoeveelheid data");
            dynamic argument = e.Argument;

            TableManager maneger = argument.T.TableManeger as TableManager;
            //zorg eerst dat de data beschikbaar is.

            DataCriteria criteria = new DataCriteria();
            criteria.Limit = argument.limit;
            List<string> select = new List<string>();
            if (argument.select.CheckedItems.Count > 0)
            {

                foreach (var val in argument.select.CheckedItems)
                {
                    select.Add(val.Origenal);
                }

                
            }
            else
            {
                foreach (var val in argument.select.Items)
                {
                    select.Add(val.Origenal);
                }
            }

            criteria.Select = select;

            maneger.GetAllData(criteria);

            //start Excelapplicatie
            WorkbookCreator.Workbook wb = new WorkbookCreator.Workbook();

            try
            {
                var infoSheet = wb.GetSheet(
                    "info",
                    new List<string> { "dscription" }, new List<List<object>> { new List<object> { maneger.GetInfo() } });
                var sheet = wb.GetSheet("getallen", maneger.TableData.HeaderData, maneger.TableData.RowData);

                int i = 0;
                foreach (TableData tab in maneger.GetExtraData(criteria))
                {
                    wb.GetSheet(criteria.Select[i], tab.HeaderData, tab.RowData);
                    i++;
                }
            }
            catch
            {
                MessageBox.Show("er is iets misgegaan tijdens het ophalen van de data");
            }

            wb.Vissable = true;
        }

        private void back_Click(object sender, EventArgs e)
        {
            Globals.MainWindow.ShowSelectTabel();
        }        
    }
}
