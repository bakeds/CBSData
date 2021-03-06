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
    public partial class SelectTabel : UserControl
    {
        public SelectTabel()
        {
            InitializeComponent();
            this.SearchTheme.TextChanged += new System.EventHandler(this.SearchTheme_TextChanged);

            InitialLists();

            Globals.DataBaseUpdated += this.SearchTabel_TextChanged;
            Globals.DataBaseUpdated += this.SearchTheme_TextChanged;
        }

        private void InitialLists()
        {
            //plaats alle tabellen
            var AllTabels = Globals.LocalCatalog.Tables;
            this.TabelsList.Items.AddRange(AllTabels.ToArray());

            //plaats alle thema's
            var allThemes = Globals.LocalCatalog.Themes;
            this.ThemesList.Items.AddRange(allThemes.ToArray());

        }

        private void TabelsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var t = this.TabelsList.SelectedItem as LocalCatalogTable;

            this.Title.Text = t.Title;
            this.Summary.Text = t.Summary;

        }

        /// <summary>
        /// hier worden de zoekcriteria verscherpt of juist versoepelt dit werkt op basis van een LIKE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTabel_TextChanged(object sender, EventArgs e)
        {
            LocalCatalogTable[] newTables;
            //als er een thema geselcteerd is moet die worden gebruikt
            if(this.ThemesList.SelectedItem==null)
            {
                newTables = Globals.LocalCatalog.GetTablesWhere(this.SearchTabel.Text, this.SearchTheme.Text);
            }
            else
            {
                var theme = (LocalCatalogTheme)(this.ThemesList.SelectedItem);
                newTables = Globals.LocalCatalog.GetTablesWhere(this.SearchTabel.Text, theme.Title);
            }

            //resultaat is binnen verwijder oude items!
            this.TabelsList.Items.Clear();
            this.TabelsList.Items.AddRange(newTables);
        }

        private void SearchTheme_TextChanged(object sender, EventArgs e)
        {
            var newThemes = Globals.LocalCatalog.GetThemsWhere(this.SearchTheme.Text);

            //gooi oude resultaten weg
            this.ThemesList.Items.Clear();
            this.ThemesList.Items.AddRange(newThemes);
        }

        private void LocationTable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var t = this.TabelsList.SelectedItem as LocalCatalogTable;

            if(t!=null)
                System.Diagnostics.Process.Start("http://opendata.cbs.nl/OData3StatlineBulkService/" + t.Identifier.Replace(" ", "") );//er komenen wel eens spaties in voor die worden hier verwijdert
        }

        

        private void InfoButton_Click(object sender, EventArgs e)
        {
             var t = this.TabelsList.SelectedItem as LocalCatalogTable;

             if (t != null)
             {
                 Globals.MainWindow.SetInfoWait(new SelectTableInformationWait(t));
             }        
        }
    }
}
