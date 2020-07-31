using PU.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PU
{
    public class CompareSqlite
    {
        public CompareParams openAndCheckBases(string txtLeftFile, string txtRightFile)
        {
            float v;
            ComparisonType ctype = ComparisonType.None;
            ctype = ComparisonType.CompareSchemaOnly;

            try
            {
                // Make sure both files are SQLite version 3 databases
                v = Utils.GetSQLiteVersion(txtLeftFile.Trim());
                if (v == -1)
                {
                    _params = new CompareParams("", "Файл эталонной базы не распознан как база данных SQLite", ctype, false);
                    return _params;
                }
                else if (v < 3)
                {
                    _params = new CompareParams("", "The left file has an older SQLite file format that is not supported by this utility.\r\n" +
                        "If you really want to compare this file then you'll have to convert it to the newer file\r\n" +
                        "format by following the instructions at http://www.sqlite.org/formatchng.html", ctype, false);
                    return _params;
                }
            }
            catch (Exception ex)
            {
                _params = new CompareParams("", ex.Message, ctype, false);
                return _params;
            } // catch

            try
            {
                v = Utils.GetSQLiteVersion(txtRightFile.Trim());
                if (v == -1)
                {
                    _params = new CompareParams("", "Файл проверяемой базы не распознан как база данных SQLite", ctype, false);

                }
                else if (v < 3)
                {
                    _params = new CompareParams("", "The right file has an older SQLite file format that is not supported by this utility.\r\n" +
                        "If you really want to compare this file then you'll have to convert it to the newer file\r\n" +
                        "format by following the instructions at http://www.sqlite.org/formatchng.html", ctype, false);
                    return _params;

                }
            }
            catch (Exception ex)
            {
                _params = new CompareParams("", ex.Message, ctype, false);

            } // catch


            // Prepare the comparison parameters object
            _params = new CompareParams(txtLeftFile.Trim(), txtRightFile.Trim(), ctype, false);
            return _params;
        }

        #region Private Variables
        private CompareParams _params;
        #endregion

    }
}
