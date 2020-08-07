using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PU.Models;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.IO;
using PU.Classes;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using PU.Models.ModelViews;
using PU.Models.Mapping;

namespace PU.Dictionaries
{
    partial class BaseDictionaryFormList : Telerik.WinControls.UI.RadForm
    {


        public BaseDictionaryFormList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Перехват нажатия на ESC для закрытия формы
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
