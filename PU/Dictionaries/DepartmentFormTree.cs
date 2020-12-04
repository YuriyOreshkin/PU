using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Dictionaries
{
    partial class DepartmentFormTree : Telerik.WinControls.UI.RadForm
    {


        public DepartmentFormTree()
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

        private void radGridViewBaseDictionary_CreateRow(object sender, GridViewCreateRowEventArgs e)
        {
            if (e.RowType == typeof(GridDataRowElement))
            {
                e.RowType = typeof(CustomRowElement);
            }
        }

      
    }
}
