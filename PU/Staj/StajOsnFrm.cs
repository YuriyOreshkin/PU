using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using PU.Models;
using PU.Classes;
using Telerik.WinControls.UI;
using System.Reflection;
using PU.FormsSPW2_2014;
using PU.FormsRSW2014;



namespace PU.Staj
{
    public partial class StajOsnFrm : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public string ParentFormName { get; set; }
        public RaschetPeriodContainer period { get; set; }
        public StajOsn formData { get; set; }
        public int rowindex { get; set; }
        private bool setNull = true;
        public bool dateControl = true;


        public StajOsnFrm()
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
                formData = null;
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {

            List<ErrList> validation = StajValidation();

            if (validation.Count != 0)
            {
                foreach (var item in validation)
                {
                    RadMessageBox.Show(item.name);
                }
            }
            else
            {
                if (ParentFormName == "SPW2")  // если заводим стаж для СПВ-2
                {
                    bool flag_ok = false;
                    try
                    {
                        getValues();
                        flag_ok = true;
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы. Код ошибки: " + ex.Message);
                    }

                    if (flag_ok)
                    {
                        switch (action)
                        {
                            case "add":

                                // нет дублирующих записей, уникальность (просто добавляем)
                                if (!db.StajOsn.Any(x => x.Number == formData.Number && x.FormsSPW2_ID == formData.FormsSPW2_ID))
                                {
                                    try
                                    {
                                        db.AddToStajOsn(formData);
                                        db.SaveChanges();
                                        setNull = false;
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("При сохранении данных об основном стаже для Формы СПВ-2 произошла ошибка. Код ошибки: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    RadMessageBox.Show("Ошибка! Нарушение уникальности записей по ключу. ");
                                }

                                break;
                            case "edit":
                                // нет дублирующих записей, уникальность (изменяем)
                                if (!db.StajOsn.Any(x => x.Number == formData.Number && x.FormsSPW2_ID == formData.FormsSPW2_ID && x.ID != formData.ID))
                                {
                                    // выбираем из базы исходную запись по идешнику
                                    StajOsn r1 = db.StajOsn.FirstOrDefault(x => x.ID == formData.ID);
                                    try
                                    {
                                        var fields = typeof(StajOsn).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                        var names = Array.ConvertAll(fields, field => field.Name);

                                        foreach (var itemName_ in names)
                                        {
                                            string itemName = itemName_.TrimStart('_');
                                            var properties = formData.GetType().GetProperty(itemName);
                                            if (properties != null)
                                            {
                                                object value = properties.GetValue(formData, null);
                                                var data = value;

                                                r1.GetType().GetProperty(itemName).SetValue(r1, data, null);
                                            }

                                        }


                                        // сохраняем модифицированную запись обратно в бд
                                        db.ObjectStateManager.ChangeObjectState(r1, System.Data.EntityState.Modified);
                                        db.SaveChanges();
                                        setNull = false;
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("При сохранении данных об основном стаже для Формы СПВ-2 произошла ошибка. Код ошибки: " + ex.Message);
                                    }

                                }
                                else
                                {
                                    RadMessageBox.Show("Ошибка! Нарушение уникальности записей по ключу. ");
                                }
                                break;
                        }
                    }


                }
                else if (ParentFormName == "SZV_6_4")  // если заводим стаж для СЗВ-6-4
                {
                    bool flag_ok = false;
                    try
                    {
                        getValues();
                        flag_ok = true;
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы. Код ошибки: " + ex.Message);
                    }

                    if (flag_ok)
                    {
                        switch (action)
                        {
                            case "add":

                                // нет дублирующих записей, уникальность (просто добавляем)
                                if (!db.StajOsn.Any(x => x.Number == formData.Number && x.FormsSZV_6_4_ID == formData.FormsSZV_6_4_ID))
                                {
                                    try
                                    {
                                        db.AddToStajOsn(formData);
                                        db.SaveChanges();
                                        setNull = false;
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("При сохранении данных об основном стаже для Формы СЗВ-6-4 произошла ошибка. Код ошибки: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    RadMessageBox.Show("Ошибка! Нарушение уникальности записей по ключу. ");
                                }

                                break;
                            case "edit":
                                // нет дублирующих записей, уникальность (изменяем)
                                if (!db.StajOsn.Any(x => x.Number == formData.Number && x.FormsSZV_6_4_ID == formData.FormsSZV_6_4_ID && x.ID != formData.ID))
                                {
                                    // выбираем из базы исходную запись по идешнику
                                    StajOsn r1 = db.StajOsn.FirstOrDefault(x => x.ID == formData.ID);
                                    try
                                    {
                                        var fields = typeof(StajOsn).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                        var names = Array.ConvertAll(fields, field => field.Name);

                                        foreach (var itemName_ in names)
                                        {
                                            string itemName = itemName_.TrimStart('_');
                                            var properties = formData.GetType().GetProperty(itemName);
                                            if (properties != null)
                                            {
                                                object value = properties.GetValue(formData, null);
                                                var data = value;

                                                r1.GetType().GetProperty(itemName).SetValue(r1, data, null);
                                            }

                                        }


                                        // сохраняем модифицированную запись обратно в бд
                                        db.ObjectStateManager.ChangeObjectState(r1, System.Data.EntityState.Modified);
                                        db.SaveChanges();
                                        setNull = false;
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("При сохранении данных об основном стаже для Формы СЗВ-6-4 произошла ошибка. Код ошибки: " + ex.Message);
                                    }

                                }
                                else
                                {
                                    RadMessageBox.Show("Ошибка! Нарушение уникальности записей по ключу. ");
                                }
                                break;
                        }
                    }


                }
                else if (ParentFormName == "SZV_6")  // если заводим стаж для СЗВ-6
                {
                    bool flag_ok = false;
                    try
                    {
                        getValues();
                        flag_ok = true;
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы. Код ошибки: " + ex.Message);
                    }

                    if (flag_ok)
                    {
                        switch (action)
                        {
                            case "add":

                                // нет дублирующих записей, уникальность (просто добавляем)
                                if (!db.StajOsn.Any(x => x.Number == formData.Number && x.FormsSZV_6_ID == formData.FormsSZV_6_ID))
                                {
                                    try
                                    {
                                        db.AddToStajOsn(formData);
                                        db.SaveChanges();
                                        setNull = false;
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("При сохранении данных об основном стаже для Формы СЗВ-6 произошла ошибка. Код ошибки: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    RadMessageBox.Show("Ошибка! Нарушение уникальности записей по ключу. ");
                                }

                                break;
                            case "edit":
                                // нет дублирующих записей, уникальность (изменяем)
                                if (!db.StajOsn.Any(x => x.Number == formData.Number && x.FormsSZV_6_ID == formData.FormsSZV_6_ID && x.ID != formData.ID))
                                {
                                    // выбираем из базы исходную запись по идешнику
                                    StajOsn r1 = db.StajOsn.FirstOrDefault(x => x.ID == formData.ID);
                                    try
                                    {
                                        var fields = typeof(StajOsn).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                        var names = Array.ConvertAll(fields, field => field.Name);

                                        foreach (var itemName_ in names)
                                        {
                                            string itemName = itemName_.TrimStart('_');
                                            var properties = formData.GetType().GetProperty(itemName);
                                            if (properties != null)
                                            {
                                                object value = properties.GetValue(formData, null);
                                                var data = value;

                                                r1.GetType().GetProperty(itemName).SetValue(r1, data, null);
                                            }

                                        }


                                        // сохраняем модифицированную запись обратно в бд
                                        db.ObjectStateManager.ChangeObjectState(r1, System.Data.EntityState.Modified);
                                        db.SaveChanges();
                                        setNull = false;
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("При сохранении данных об основном стаже для Формы СЗВ-6 произошла ошибка. Код ошибки: " + ex.Message);
                                    }

                                }
                                else
                                {
                                    RadMessageBox.Show("Ошибка! Нарушение уникальности записей по ключу. ");
                                }
                                break;
                        }
                    }


                }
                else
                {
                    getValues();
                    setNull = false;
                    this.Close();
                }
            }

        }

        private void getValues()
        {
            List<StajLgot> StajLgotList = formData.StajLgot.ToList();

            formData.Number = long.Parse(NumberSpin.Value.ToString());
            if (StajBeginDate.Text != "")
                formData.DateBegin = StajBeginDate.Value;

            if (StajEndDate.Text != "")
                formData.DateEnd = StajEndDate.Value;

            if (ParentFormName == "SZV_STAJ_Edit")
            {
                formData.CodeBEZR = codeBEZRCheckBox.Checked;
            }

        }


        private void setValues()
        {
            NumberSpin.Value = formData.Number.Value;
            StajBeginDate.Value = formData.DateBegin.Value;

            StajEndDate.Value = formData.DateEnd.Value;

            if (ParentFormName == "SZV_STAJ_Edit")
            {
                codeBEZRCheckBox.Checked = formData.CodeBEZR.HasValue ? formData.CodeBEZR.Value : false;
            }

        }

        /// <summary>
        /// Проверка на правильность запонения формы
        /// </summary>
        /// <returns></returns>
        private List<ErrList> StajValidation()
        {
            List<ErrList> ErrorList = new List<ErrList> { };  // список для занесения выВленных ошибок
            if (StajBeginDate.Value == null || StajBeginDate.Value == StajBeginDate.NullDate)
            {
                ErrorList.Add(new ErrList { name = "Не заполнена дата начала стажа", control = "StajBeginDate" });
            }
            if (StajEndDate.Value == null || StajEndDate.Value == StajEndDate.NullDate)
            {
                ErrorList.Add(new ErrList { name = "Не заполнена дата окончания стажа", control = "StajEndDate" });
            }
            if (StajBeginDate.Value != null && StajEndDate.Value != null && StajBeginDate.Value != StajBeginDate.NullDate && StajEndDate.Value != StajEndDate.NullDate && period != null)
            {
                if (StajBeginDate.Value > StajEndDate.Value)
                    ErrorList.Add(new ErrList { name = "Дата начала не может быть больше даты окончания стажа", control = "StajBeginDate" });

                //if (StajBeginDate.Value.Date < period.DateBegin || StajBeginDate.Value.Date > period.DateEnd)
                //    ErrorList.Add(new ErrList { name = "Ошибка! Дата начала периода должна быть в пределах выбранного отчетного периода", control = "StajBeginDate" });
                //if (StajEndDate.Value.Date < period.DateBegin || StajEndDate.Value.Date > period.DateEnd)
                //    ErrorList.Add(new ErrList { name = "Ошибка! Дата окончания периода должна быть в пределах выбранного отчетного периода", control = "StajEndDate" });
            }
            if (ErrorList.Count == 0)
            {
                switch (ParentFormName)
                {
                    case "RSW2014_6":
                        RSW2014_6 main = this.Owner as RSW2014_6;
                        if (main != null)
                        {
                            if (main.StajOsn_List.Count != 0)
                            {
                                List<StajOsn> StajOsn_List_ = new List<StajOsn> { };
                                foreach (var item in main.StajOsn_List)
                                {
                                    StajOsn_List_.Add(item);
                                }

                                if (rowindex >= 0)
                                    StajOsn_List_.RemoveAt(rowindex);

                                if (StajOsn_List_.Any(x => x.Number == (long)NumberSpin.Value))
                                {
                                    ErrorList.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumberSpin" });
                                }

                                if (dateControl)
                                {
                                    foreach (var item in StajOsn_List_)
                                    {
                                        if (!((StajBeginDate.Value > item.DateEnd.Value) || (StajEndDate.Value < item.DateBegin.Value)))
                                        {
                                            ErrorList.Add(new ErrList { name = "Ошибка! Диапазон дат записи о стаже, не может пересекаться с периодами предыдуших записей о стаже. Необходимо исправить!", control = "StajBeginDate" });
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "SPW2":
                        StajList main_ = this.Owner as StajList;
                        if (main_ != null)
                        {
                            if (main_.stajOsnGrid.RowCount != 0)
                            {
                                List<StajOsn> StajOsn_List_ = new List<StajOsn> { };
                                foreach (var item in main_.stajOsnGrid.Rows)
                                {
                                    StajOsn_List_.Add(new StajOsn { DateBegin = DateTime.Parse(item.Cells["DateBegin"].Value.ToString()), DateEnd = DateTime.Parse(item.Cells["DateEnd"].Value.ToString()) });
                                }

                                if (rowindex >= 0)
                                    StajOsn_List_.RemoveAt(rowindex);

                                if (StajOsn_List_.Any(x => x.Number == (long)NumberSpin.Value))
                                {
                                    ErrorList.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumberSpin" });
                                }

                                if (dateControl)
                                {
                                    foreach (var item in StajOsn_List_)
                                    {
                                        if (!((StajBeginDate.Value > item.DateEnd.Value) || (StajEndDate.Value < item.DateBegin.Value)))
                                        {
                                            ErrorList.Add(new ErrList { name = "Ошибка! Диапазон дат записи о стаже, не может пересекаться с периодами предыдуших записей о стаже. Необходимо исправить!", control = "StajBeginDate" });
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "SZV_6_4":
                        StajList main_2 = this.Owner as StajList;
                        if (main_2 != null)
                        {
                            if (main_2.stajOsnGrid.RowCount != 0)
                            {
                                List<StajOsn> StajOsn_List_ = new List<StajOsn> { };
                                foreach (var item in main_2.stajOsnGrid.Rows)
                                {
                                    StajOsn_List_.Add(new StajOsn { DateBegin = DateTime.Parse(item.Cells["DateBegin"].Value.ToString()), DateEnd = DateTime.Parse(item.Cells["DateEnd"].Value.ToString()) });
                                }

                                if (rowindex >= 0)
                                    StajOsn_List_.RemoveAt(rowindex);

                                if (StajOsn_List_.Any(x => x.Number == (long)NumberSpin.Value))
                                {
                                    ErrorList.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumberSpin" });
                                }

                                if (dateControl)
                                {
                                    foreach (var item in StajOsn_List_)
                                    {
                                        if (!((StajBeginDate.Value > item.DateEnd.Value) || (StajEndDate.Value < item.DateBegin.Value)))
                                        {
                                            ErrorList.Add(new ErrList { name = "Ошибка! Диапазон дат записи о стаже, не может пересекаться с периодами предыдуших записей о стаже. Необходимо исправить!", control = "StajBeginDate" });
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "SZV_6":
                        StajList main_3 = this.Owner as StajList;
                        if (main_3 != null)
                        {
                            if (main_3.stajOsnGrid.RowCount != 0)
                            {
                                List<StajOsn> StajOsn_List_ = new List<StajOsn> { };
                                foreach (var item in main_3.stajOsnGrid.Rows)
                                {
                                    StajOsn_List_.Add(new StajOsn { DateBegin = DateTime.Parse(item.Cells["DateBegin"].Value.ToString()), DateEnd = DateTime.Parse(item.Cells["DateEnd"].Value.ToString()) });
                                }

                                if (rowindex >= 0)
                                    StajOsn_List_.RemoveAt(rowindex);

                                if (StajOsn_List_.Any(x => x.Number == (long)NumberSpin.Value))
                                {
                                    ErrorList.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumberSpin" });
                                }

                                if (dateControl)
                                {
                                    foreach (var item in StajOsn_List_)
                                    {
                                        if (!((StajBeginDate.Value > item.DateEnd.Value) || (StajEndDate.Value < item.DateBegin.Value)))
                                        {
                                            ErrorList.Add(new ErrList { name = "Ошибка! Диапазон дат записи о стаже, не может пересекаться с периодами предыдуших записей о стаже. Необходимо исправить!", control = "StajBeginDate" });
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "SZV_STAJ_Edit":
                        PU.FormsSZV_STAJ.SZV_STAJ_Edit main_4 = this.Owner as PU.FormsSZV_STAJ.SZV_STAJ_Edit;
                        if (main_4 != null)
                        {
                            if (main_4.StajOsn_List.Count != 0)
                            {
                                List<StajOsn> StajOsn_List_ = new List<StajOsn> { };
                                foreach (var item in main_4.StajOsn_List)
                                {
                                    StajOsn_List_.Add(item);
                                }

                                if (rowindex >= 0)
                                    StajOsn_List_.RemoveAt(rowindex);

                                if (StajOsn_List_.Any(x => x.Number == (long)NumberSpin.Value))
                                {
                                    ErrorList.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumberSpin" });
                                }

                                if (dateControl)
                                {
                                    foreach (var item in StajOsn_List_)
                                    {
                                        if (!((StajBeginDate.Value > item.DateEnd.Value) || (StajEndDate.Value < item.DateBegin.Value)))
                                        {
                                            ErrorList.Add(new ErrList { name = "Ошибка! Диапазон дат записи о стаже, не может пересекаться с периодами предыдуших записей о стаже. Необходимо исправить!", control = "StajBeginDate" });
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }




            return ErrorList;
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    radButton1.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    break;
            }
        }


        private void StajOsnFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (setNull)
                formData = null;
        }

        private void StajOsnFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            checkAccessLevel();

            if (ParentFormName == "SZV_STAJ_Edit")
            {
                codeBEZRCheckBox.Visible = true;
            }

            switch (action)
            {
                case "add":

                    break;
                case "edit":
                    setValues();
                    break;
            }
        }


        private void radButton2_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
        }

        private void StajBeginDate_ValueChanged(object sender, EventArgs e)
        {
            if (StajBeginDate.Value != StajBeginDate.NullDate)
                beginDateMaskedEditBox.Text = StajBeginDate.Value.ToShortDateString();
            else
                beginDateMaskedEditBox.Text = beginDateMaskedEditBox.NullText;
        }

        private void radMaskedEditBox1_Leave(object sender, EventArgs e)
        {
            if (beginDateMaskedEditBox.Text != beginDateMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(beginDateMaskedEditBox.Text, out date))
                {
                    StajBeginDate.Value = date;
                }
                else
                {
                    StajBeginDate.Value = StajBeginDate.NullDate;
                }
            }
            else
            {
                StajBeginDate.Value = StajBeginDate.NullDate;
            }

        }

        private void StajEndDate_ValueChanged(object sender, EventArgs e)
        {
            if (StajEndDate.Value != StajEndDate.NullDate)
                endDateMaskedEditBox.Text = StajEndDate.Value.ToShortDateString();
            else
                endDateMaskedEditBox.Text = endDateMaskedEditBox.NullText;
        }

        private void endDateMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (endDateMaskedEditBox.Text != endDateMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(endDateMaskedEditBox.Text, out date))
                {
                    StajEndDate.Value = date;
                }
                else
                {
                    StajEndDate.Value = StajEndDate.NullDate;
                }
            }
            else
            {
                StajEndDate.Value = StajEndDate.NullDate;
            }
        }



    }
}
