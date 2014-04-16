﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUICBSData.MainScreen;

namespace GUICBSData
{
    public partial class MainWindow : Form
    {
        #region private fields
        private About _aboutBox = new About();
        
        #region Screens
        private Home _home = new Home();
        private SelectTabel _selectTabel;
        #endregion

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            this.Controls.Add(this._home);
            this.Controls[this.Controls.Count - 1].Dock = DockStyle.Fill;
        }

        /// <summary>
        /// laat de auboutbox zien
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._aboutBox.Show();
        }

        private void synchroniseerCatalogesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Globals.SychronisserCataloges();
        }

        public void ShowSelectTabel()
        {
            this._selectTabel = new SelectTabel();
            this._selectTabel.Dock = DockStyle.Fill;
            this.Controls.Remove(this._home);
            this.Controls.Add(this._selectTabel);            
        }
    }
}