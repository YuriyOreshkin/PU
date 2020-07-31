using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using SQLiteParser;
using System.Data.SQLite;
using System.Data;
using Microsoft.Win32;

namespace PU.Classes
{

    public static class Utils
    {
        #region Private Types
        private delegate object ValueConverter(object value);
        #endregion


        private static Dictionary<DbType, Dictionary<Type, ValueConverter>> _converters = new Dictionary<DbType, Dictionary<Type, ValueConverter>>();
        private static Regex _comment = new Regex(@"(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)");
        private static Regex _comment2 = new Regex(@"(\-\-[^\n]*)\n");
        private static int _seed = 1;



        /// <summary>
        /// Return the version number for the specified database file.
        /// </summary>
        /// <param name="fpath">The path to the database file</param>
        /// <returns>-1 if the file was not recognized as a valid SQLite file, or the version of the
        /// SQLite file</returns>
        public static float GetSQLiteVersion(string fpath)
        {
            int index = 0;
            byte[] buffer = new byte[1024];
            using (FileStream fs = File.Open(fpath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int res = fs.ReadByte();
                while (res != -1 && res != 0 && index < buffer.Length)
                {
                    buffer[index] = (byte)res;
                    res = fs.ReadByte();
                    index++;
                } // while

                fs.Close();
            } // using

            // A valid SQLite file will have a much lower index value
            if (index == buffer.Length)
                return -1F;

            // Convert the bytes array that was read to an ASCII string
            string vstr = Encoding.ASCII.GetString(buffer, 0, index);

            // Check for version 3 and above file format
            Regex rx3 = new Regex(@"SQLite format (\d+(\.\d+)?)");
            Match m = rx3.Match(vstr);
            if (m.Success)
            {
                float res;
                if (float.TryParse(m.Groups[1].Value, out res))
                    return res;
                else
                    return -1F;
            }

            // Check for version 2.x format
            Regex rx2 = new Regex(@"\*\* This file contains an SQLite (\d+(\.\d+)?) database \*\*");
            m = rx2.Match(vstr);
            if (m.Success)
            {
                float res;
                if (float.TryParse(m.Groups[1].Value, out res))
                    return res;
            }

            return -1F;
        }

        /// <summary>
        /// Sometimes, the table names that appear in the database have non-letter/digit characters.
        /// In order to create valid letters that will be used when creating the temporary file name,
        /// we need to normalize these letters to '_'.
        /// </summary>
        /// <param name="name">The original table/db object name</param>
        /// <returns>The normalized version of that name</returns>
        public static string NormalizeName(string name)
        {
            // Remove quotes
            if (name.Length > 1 && name[0] == '[' && name[name.Length - 1] == ']')
                name = name.Substring(1, name.Length - 2);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                if (Char.IsLetterOrDigit(name[i]))
                    sb.Append(name[i]);
                else
                    sb.Append("_");
            } // for
            if (Char.IsDigit(sb[0]))
                sb.Insert(0, "_");
            return sb.ToString();
        }

        /// <summary>
        /// Check if the specified column acts as an alias to the hidden RowID column.
        /// The (somewhat weird) rules for this can be found at http://www.sqlite.org/lang_createtable.html
        /// </summary>
        /// <param name="table">The table schema object</param>
        /// <param name="name">The column name</param>
        /// <returns>TRUE if the column is acting as a rowid alias, FALSE otherwise</returns>
        public static bool IsColumnActingAsRowIdAlias(SQLiteCreateTableStatement table, string name)
        {
            SQLiteColumnStatement column = Classes.Utils.FindColumn(table.Columns, name);
            if (column.ColumnType.TypeName.ToLower() == "integer")
            {
                if (column.ColumnConstraints != null)
                {
                    foreach (SQLiteColumnConstraint cons in column.ColumnConstraints)
                    {
                        SQLitePrimaryKeyColumnConstraint pcon = cons as SQLitePrimaryKeyColumnConstraint;
                        if (pcon != null)
                        {
                            // See http://www.sqlite.org/lang_createtable.html (bottom of the page) for more details
                            if (pcon.SortOrder == SQLiteSortOrder.Ascending || pcon.SortOrder == SQLiteSortOrder.None)
                                return true;
                            else
                                return false;
                        } // if
                    } // foreach
                }

                if (table.Constraints != null)
                {
                    foreach (SQLiteTableConstraint tcon in table.Constraints)
                    {
                        SQLitePrimaryKeyTableConstraint ptcon = tcon as SQLitePrimaryKeyTableConstraint;
                        if (ptcon != null)
                        {
                            if (ptcon.Columns.Count == 1)
                            {
                                if (SQLiteParser.Utils.Chop(ptcon.Columns[0].ColumnName).ToLower() ==
                                    SQLiteParser.Utils.Chop(name).ToLower())
                                {
                                    return true;
                                }
                            }
                            break;
                        } // if
                    } // foreach
                } // if
            }

            return false;
        }

        /// <summary>
        /// Finds a column with the specified name within the specified columns list.
        /// </summary>
        /// <param name="list">The list to search</param>
        /// <param name="colname">The name of the column to find</param>
        /// <returns>The column object with the specified name</returns>
        public static SQLiteColumnStatement FindColumn(List<SQLiteColumnStatement> list, string colname)
        {
            foreach (SQLiteColumnStatement stmt in list)
            {
                if (SQLiteParser.Utils.Chop(stmt.ObjectName.ToString()).ToLower() ==
                    SQLiteParser.Utils.Chop(colname).ToLower())
                    return stmt;
            } // foreach

            return null;
        }

        /// <summary>
        /// Searches the columns of the specified table statement for a column with the specified name
        /// </summary>
        /// <param name="stmt">The CREATE TABLE statement to search</param>
        /// <param name="colname">The name of the column to search</param>
        /// <returns>The column statement or NULL if not found</returns>
        public static SQLiteColumnStatement GetColumnByName(SQLiteCreateTableStatement stmt, string colname)
        {
            return GetColumnByName(stmt.Columns, colname);
        }

        /// <summary>
        /// Searches the columns of the specified columns list for a column with the specified name
        /// </summary>
        /// <param name="stmt">The CREATE TABLE statement to search</param>
        /// <param name="colname">The name of the column to search</param>
        /// <returns>The column statement or NULL if not found</returns>
        public static SQLiteColumnStatement GetColumnByName(List<SQLiteColumnStatement> clist, string colname)
        {
            foreach (SQLiteColumnStatement col in clist)
            {
                if (SQLiteParser.Utils.Chop(col.ObjectName.ToString().ToLower()) ==
                    SQLiteParser.Utils.Chop(colname.ToLower()))
                    return col;
            } // foreach

            return null;
        }

        /// <summary>
        /// Create a new SQLite connection to the specified database file
        /// </summary>
        /// <param name="fpath">The path to teh DB file</param>
        /// <param name="writable">TRUE for having read/write connection, FALSE for having
        /// readonly connection</param>
        /// <returns>The SQLite connection object</returns>
        public static SQLiteConnection MakeDbConnection(string fpath, bool writable)
        {
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = fpath;
            sb.ReadOnly = !writable;
            return new SQLiteConnection(sb.ConnectionString, true);
        }

        /// <summary>
        /// Computes the set of common columns to both table schema objects.
        /// </summary>
        /// <param name="fromTable">The table schema object that acts as the basis from which the common columns will be taken</param>
        /// <param name="rightTable">The table schema object that is used for comparison</param>
        /// <returns>The list of common column statements</returns>
        public static List<SQLiteColumnStatement> GetCommonColumns(
            SQLiteCreateTableStatement fromTable,
            SQLiteCreateTableStatement toTable,
            bool ignoreColumnType,
            out List<SQLiteColumnStatement> diffColumns)
        {
            diffColumns = null;
            List<SQLiteColumnStatement> common = new List<SQLiteColumnStatement>();
            for (int i = 0; i < fromTable.Columns.Count; i++)
            {
                SQLiteColumnStatement lcol = fromTable.Columns[i];
                for (int k = 0; k < toTable.Columns.Count; k++)
                {
                    SQLiteColumnStatement rcol = toTable.Columns[k];
                    if (lcol.ObjectName.Equals(rcol.ObjectName))
                    {
                        common.Add(lcol);

                        // This column is common to both the left table and to the right table
                        // so we need to make sure that it has the same type in both tables.
                        if (!ignoreColumnType && !Utils.IsColumnTypesMatching(lcol, rcol))
                        {
                            // Add this column to the list of columns that have different types in
                            // the two databases.
                            if (diffColumns == null)
                                diffColumns = new List<SQLiteColumnStatement>();
                            diffColumns.Add(rcol);
                        } // else
                    } // if
                } // for
            } // for

            return common;
        }

        /// <summary>
        /// Computes the set of common columns to both table schema objects.
        /// </summary>
        /// <param name="leftTable">The left table schema object</param>
        /// <param name="rightTable">The right table schema object</param>
        /// <returns>The list of common column statements</returns>
        public static List<SQLiteColumnStatement> GetCommonColumns(
            SQLiteCreateTableStatement leftTable,
            SQLiteCreateTableStatement rightTable)
        {
            List<SQLiteColumnStatement> dcols = null;
            return GetCommonColumns(leftTable, rightTable, false, out dcols);
        }

        /// <summary>
        /// Checks if the specified columns have a matching underlying type
        /// </summary>
        /// <remarks>As an example - nvarchar(50) and cahr(500) have a matching type</remarks>
        /// <param name="leftCol">The left column to compare</param>
        /// <param name="rightCol">The right column to compare</param>
        /// <returns>TRUE if the two columns have a matching type, FALSE otherwise</returns>
        public static bool IsColumnTypesMatching(SQLiteColumnStatement leftCol, SQLiteColumnStatement rightCol)
        {
            DbType ltype = Utils.GetDbType(leftCol.ColumnType);
            DbType rtype = Utils.GetDbType(rightCol.ColumnType);

            if (ltype == DbType.Single && rtype == DbType.Double ||
                ltype == DbType.Double && ltype == DbType.Single)
                return true;
            else
                return ltype == rtype;
        }

        /// <summary>
        /// For every column type - return the DbType value that is most appropriate for it.
        /// </summary>
        /// <param name="colType">The column type object</param>
        /// <returns>A DbType enumeration value</returns>
        public static DbType GetDbType(SQLiteColumnType colType)
        {
            string tname = colType.TypeName;
            if (tname == null || tname == string.Empty)
                return DbType.Binary;
            tname = tname.ToLower();

            if (tname.Contains("int"))
                return DbType.Int64;
            if (tname.Contains("float") || tname.Contains("double") || tname.Contains("numeric"))
                return DbType.Double;
            if (tname.Contains("real"))
                return DbType.Single;

            if (tname.Contains("boolean") || tname.Contains("bit"))
                return DbType.Boolean;

            if (colType.TypeName == "datetime" || colType.TypeName == "timestamp")
                return DbType.DateTime;

            if (tname.Contains("char") || tname.Contains("text") || tname.Contains("clob"))
                return DbType.String;

            if (tname.Contains("blob"))
                return DbType.Binary;

            if (tname.Contains("uniqueidentifier") || tname.Contains("guid"))
                return DbType.Guid;

            return DbType.String;
        }

        /// <summary>
        /// Extract the list of primary column names from the table statement
        /// </summary>
        /// <param name="table">The table statement object</param>
        /// <returns>The list of primary columns</returns>
        public static List<SQLiteColumnStatement> GetPrimaryColumns(SQLiteCreateTableStatement table)
        {
            List<SQLiteColumnStatement> res = new List<SQLiteColumnStatement>();

            // First - search for the primary column in the columns list part. If found - there can
            // be exactly one column so break as soon as possible from the loop.
            foreach (SQLiteColumnStatement col in table.Columns)
            {
                if (col.ColumnConstraints != null)
                {
                    foreach (SQLiteColumnConstraint ccon in col.ColumnConstraints)
                    {
                        if (ccon is SQLitePrimaryKeyColumnConstraint)
                        {
                            res.Add(col);
                            break;
                        }
                    } // foreach
                    if (res.Count > 0)
                        break;
                } // if
            } // foreach

            // Otherwise - the primary key appears in the table constraints list or not at all.
            if (res.Count == 0)
            {
                if (table.Constraints != null)
                {
                    // Look for the primary key(s) in the table constraints section
                    foreach (SQLiteTableConstraint tcon in table.Constraints)
                    {
                        if (tcon is SQLitePrimaryKeyTableConstraint)
                        {
                            SQLitePrimaryKeyTableConstraint pcon = (SQLitePrimaryKeyTableConstraint)tcon;
                            for (int i = 0; i < pcon.Columns.Count; i++)
                            {
                                SQLiteColumnStatement stmt = FindColumn(table.Columns, pcon.Columns[i].ColumnName);
                                if (stmt == null)
                                    throw new IndexOutOfRangeException("can't find column with name " + pcon.Columns[i].ColumnName);
                                res.Add(stmt);
                            }
                        } // if
                    } // foreach
                } // if
            } // if

            return res;
        }

        /// <summary>
        /// Searches the specified columns list for the specified column
        /// </summary>
        /// <param name="clist">The list of columns to search in</param>
        /// <param name="col">The column to search for</param>
        /// <returns>TRUE if the column was found, FALSE if not.</returns>
        public static bool ColumnListContains(List<SQLiteColumnStatement> clist, SQLiteColumnStatement col)
        {
            foreach (SQLiteColumnStatement c in clist)
            {
                if (c.ObjectName.Equals(col.ObjectName))
                    return true;
            } // foreach
            return false;
        }


        /// <summary>
        /// Builds a string that contains the names of all specified columns.
        /// This string can than be used e.g., in SELECT statements.
        /// </summary>
        /// <param name="clist">List of column objects</param>
        /// <returns>A string that contains a comma delimited list of all column names</returns>
        public static string BuildColumnsString(List<SQLiteColumnStatement> clist, bool replaceBlobs)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < clist.Count; i++)
            {
                if (replaceBlobs && Utils.GetDbType(clist[i].ColumnType) == DbType.Binary)
                {
                    // Note: we have to replace a BLOB column name by <NAME> IS NOT NULL expression in order
                    // to avoid a SELECT command from loading the entire BLOB field into memory.
                    sb.Append("(" + clist[i].ObjectName.ToString() + " IS NOT NULL) AS " + clist[i].ObjectName.ToString());
                }
                else
                    sb.Append(clist[i].ObjectName.ToString());
                if (i < clist.Count - 1)
                    sb.Append(",");
            } // for
            return sb.ToString();
        }

        /// <summary>
        /// Builds a list of column equality checks that can be used in WHERE clause
        /// </summary>
        /// <example>[objID] = @objId AND [FirstName] = @FirstName</example>
        /// <param name="clist">The columns list to use</param>
        /// <returns>The string that can be used in a WHERE clause</returns>
        public static string BuildColumnParametersEqualityCheckString(List<SQLiteColumnStatement> clist)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < clist.Count; i++)
            {
                string pname = Utils.GetColumnParameterName(clist[i].ObjectName.ToString());
                sb.Append(clist[i].ObjectName.ToString() + " = " + pname);
                if (i < clist.Count - 1)
                    sb.Append(" AND ");
            } // for
            return sb.ToString();
        }

        /// <summary>
        /// Build a list of parameter names from the specified columns list.
        /// </summary>
        /// <param name="clist">The columns list</param>
        /// <returns>A string that contains a list of parameter names</returns>
        public static string BuildColumnParametersString(List<SQLiteColumnStatement> clist)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < clist.Count; i++)
            {
                string pname = Utils.GetColumnParameterName(clist[i].ObjectName.ToString());
                sb.Append(pname);
                if (i < clist.Count - 1)
                    sb.Append(", ");
            } // for
            return sb.ToString();
        }

        /// <summary>
        /// Adds parameters to the specified command based on the columns that are specified
        /// int <paramref name="clist"/> parameter.
        /// </summary>
        /// <param name="cmd">The command we need to add parameters to</param>
        /// <param name="clist">The columns list</param>
        public static void AddCommandColumnParameters(SQLiteCommand cmd, List<SQLiteColumnStatement> clist)
        {
            for (int i = 0; i < clist.Count; i++)
            {
                string pname = Utils.GetColumnParameterName(clist[i].ObjectName.ToString());
                DbType ctype = Utils.GetDbType(clist[i].ColumnType);
                cmd.Parameters.Add(pname, ctype);
            } // for
        }

        /// <summary>
        /// Creates the corresponding column parameter name from a column name.
        /// </summary>
        /// <param name="colname">The name of the column</param>
        /// <returns>The corresponding parameter name</returns>
        public static string GetColumnParameterName(string colname)
        {
            return "@" + Utils.NormalizeName(colname);
        }

        /// <summary>
        /// Selects the appropriate converter method to perform the converstion from the
        /// value object to the required format (specified in the command parameter).
        /// </summary>
        /// <param name="prm">The command parameter</param>
        /// <param name="value">The value to convert</param>
        public static void AssignParameterValue(SQLiteParameter prm, object value)
        {
            Type srctype = value.GetType();
            DbType dsttype = prm.DbType;

            if (_converters.ContainsKey(dsttype) && _converters[dsttype].ContainsKey(srctype))
                prm.Value = _converters[dsttype][srctype](value);
            else
                prm.Value = value;
        }


        public static DbType GetDbTypeFromClrType(SQLiteColumnStatement column, object updatedValue)
        {
            if (updatedValue == DBNull.Value)
                return Utils.GetDbType(column.ColumnType);
            else
                return Utils.GetDbTypeFromClrType(updatedValue);
        }

        public static DbType GetDbTypeFromClrType(object value)
        {
            if (value is string || value is char)
                return DbType.String;
            if (value is byte || value is sbyte || value is short || value is ushort || value is int ||
                value is uint || value is long || value is ulong)
                return DbType.Int64;
            if (value is double || value is decimal)
                return DbType.Double;
            if (value is float)
                return DbType.Single;
            if (value is byte[])
                return DbType.Binary;
            if (value is bool)
                return DbType.Boolean;
            if (value is DateTime)
                return DbType.DateTime;
            if (value is Guid)
                return DbType.Guid;
            throw new ArgumentException("illegal value type [" + value.GetType().FullName + "]");
        }

        /// <summary>
        /// This method is used to check if table comparison is allowed fo the two tables
        /// whose schema objects are provided. In case the comparison is allowed - the method
        /// will provide the list of columns that can be compared.
        /// </summary>
        /// <param name="leftTable">The schema of the left database table</param>
        /// <param name="rightTable">The schema of the right database table</param>
        /// <param name="errmsg">In case the comparison is not allowed - the error message to display</param>
        /// <param name="allowBlobComparison">If TRUE - it assumes that BLOB comparison will take place,
        /// otherwise - it assumes that no BLOB fields will be compared</param>
        /// <returns>
        /// TRUE if table comparison is allowed, FALSE if not.
        /// </returns>
        public static bool IsTableComparisonAllowed(
            SQLiteCreateTableStatement leftTable, SQLiteCreateTableStatement rightTable,
            out string errmsg, bool allowBlobComparison)
        {
            errmsg = null;

            // Extract the two sets of primary keys for the left table and for the right table
            List<SQLiteColumnStatement> leftpkeys = Utils.GetPrimaryColumns(leftTable);
            List<SQLiteColumnStatement> rightpkeys = Utils.GetPrimaryColumns(rightTable);

            // Make sure that the set of primary keys is identical in both databases.
            if (leftpkeys.Count != rightpkeys.Count)
            {
                errmsg = "The set of primary keys need to be identical in order to perform the comparison";
                return false;
            }
            for (int i = 0; i < leftpkeys.Count; i++)
            {
                SQLiteColumnStatement lpcol = leftpkeys[i];
                SQLiteColumnStatement rpcol = rightpkeys[i];
                if (!Utils.IsColumnTypesMatching(lpcol, rpcol))
                {
                    errmsg = "Primary key column " + lpcol.ObjectName.ToString() + " in the left database table\r\n" +
                        "has a different type than the one in the right database table.";
                    return false;
                }
                if (!lpcol.ObjectName.Equals(rpcol.ObjectName))
                {
                    errmsg = "Primary key columns at index " + i + " have different names.";
                    return false;
                }
            } // for

            // Make sure that there is at least a single common column to both tables
            List<SQLiteColumnStatement> common = Utils.GetCommonColumns(leftTable, rightTable);
            if (common.Count == 0)
            {
                errmsg = "There are no common columns to the two tables.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the columns list contains a BLOB column.
        /// </summary>
        /// <param name="clist">The list to search</param>
        /// <returns>TRUE if the list contains a BLOB column, FALSE otherwise</returns>
        public static bool ContainsBlobColumn(List<SQLiteColumnStatement> clist)
        {
            foreach (SQLiteColumnStatement col in clist)
            {
                if (Utils.GetDbType(col.ColumnType) == DbType.Binary)
                    return true;
            } // foreach
            return false;
        }

        /// <summary>
        /// Convenience method for comparing two objects, with support for comparing byte arrays
        /// </summary>
        /// <param name="lval">The left value to compare</param>
        /// <param name="rval">The right value to compare</param>
        /// <returns>TRUE if the two values are equal, FALSE otherwise</returns>
        public static bool ObjectsAreEqual(object lval, object rval)
        {
            if (lval.GetType() != rval.GetType())
                return false;
            if (lval is byte[])
                return Array.Equals(lval, rval);
            return lval.Equals(rval);
        }

        /// <summary>
        /// Returns a list that contains columns with the same names as those specified in the <paramref name="names"/> list
        /// but taken from the columns list specified in <paramref name="basis"/> list.
        /// </summary>
        /// <param name="basis">The basis list from which column objects will be returned</param>
        /// <param name="names">A list of columns whose names are used to pick the relevant columns from the basis list.</param>
        /// <returns></returns>
        public static List<SQLiteColumnStatement> GetMatchingColumns(List<SQLiteColumnStatement> basis, List<SQLiteColumnStatement> names)
        {
            List<SQLiteColumnStatement> res = new List<SQLiteColumnStatement>();
            foreach (SQLiteColumnStatement col in names)
            {
                foreach (SQLiteColumnStatement icol in basis)
                {
                    if (col.ObjectName.Equals(icol.ObjectName))
                    {
                        res.Add(icol);
                        break;
                    }
                } // foreach
            } // foreach

            return res;
        }

        /// <summary>
        /// Used to strip the input string from any SQL comments that may cause parsing problems.
        /// </summary>
        /// <param name="sql">The input SQL string to strip</param>
        /// <returns>The stripped version of the SQL string without any SQL comments</returns>
        public static string StripComments(string sql)
        {
            sql = _comment.Replace(sql, string.Empty);
            sql = _comment2.Replace(sql, string.Empty);
            return sql;
        }

        /// <summary>
        /// This method compares the original table schema and the updated table schema and checks
        /// if a simple ALTER TABLE can be performed in order to perform the update. If so it returns
        /// the list of columns that need to be added and return TRUE.
        /// </summary>
        /// <param name="orig">The original table schema</param>
        /// <param name="updated">The updated table schema</param>
        /// <param name="added">In case a simple ALTER TABLE can be performed it will be set to the list
        /// of column statements for all columns that need to be added.</param>
        /// <returns>TRUE if we can use the ALTER TABLE command in order to modify the table schema</returns>
        public static bool AlterTableIsPossible(SQLiteDdlStatement orig, SQLiteDdlStatement updated,
            ref List<SQLiteColumnStatement> added)
        {
            SQLiteCreateTableStatement otable = (SQLiteCreateTableStatement)orig;
            SQLiteCreateTableStatement utable = (SQLiteCreateTableStatement)updated;

            // If there is any difference in the table constraints - we can't do a simple ALTER TABLE
            // in order to update the table schema.
            if (otable.Constraints == null && utable.Constraints != null ||
                otable.Constraints != null && utable.Constraints == null)
                return false;
            if (otable.Constraints != null && utable.Constraints != null)
            {
                if (otable.Constraints.Count != utable.Constraints.Count)
                    return false;
                for (int i = 0; i < utable.Constraints.Count; i++)
                {
                    if (!otable.Constraints[i].Equals(utable.Constraints[i]))
                        return false;
                } // for
            } // if

            // If any columns were deleted in the updated table or if a column
            // in the original table does not match the column with the same index
            // in the updated table exactly then we'll have to re-create the table
            // from scratch.
            if (utable.Columns.Count < otable.Columns.Count)
                return false;
            for (int i = 0; i < otable.Columns.Count; i++)
            {
                SQLiteColumnStatement ocol = otable.Columns[i];
                SQLiteColumnStatement ucol = utable.Columns[i];
                if (!ocol.Equals(ucol))
                {
                    // Note: In order to be able to perform ALTER TABLE, all columns that
                    //       exist in the original table must exist in the updated table
                    //       in the same location (index) that they had in the original
                    //       table!
                    return false;
                }
            } // for

            // At this point it is certain that the only kind of difference that can exist
            // between the original table and the updated table is that the updated table
            // contains some added columns.
            if (utable.Columns.Count == otable.Columns.Count)
                throw new ArgumentException("The original table and the updated table are the same!");

            // One last chance to change my mind - ALTER TABLE does not support adding
            // columns where the following conditions are true:
            // 1. The column has a PRIMARY KEY or UNIQUE constraint
            // 2. The column may not have a default value of CURRENT_TIME, CURRENT_DATE or CURRENT_TIMESTAMP.
            // 3. If a NOT NULL constraint is specified, then the column must have a default value other than NULL.
            for (int i = otable.Columns.Count; i < utable.Columns.Count; i++)
            {
                SQLiteColumnStatement col = utable.Columns[i];

                // If the column is NOT NULL without a suitable (not null) default value we can't use
                // ALTER TABLE command to change the table schema.
                if (Utils.IsNotNullColumnWithoutDefault(col))
                    return false;

                if (col.ColumnConstraints != null)
                {
                    foreach (SQLiteColumnConstraint ccon in col.ColumnConstraints)
                    {
                        // Don't allow PRIMARY KEY or UNIQUE constraints
                        if (ccon is SQLitePrimaryKeyColumnConstraint || ccon is SQLiteUniqueColumnConstraint)
                            return false;

                        // Check DEFAULT clause
                        SQLiteDefaultColumnConstraint defcon = ccon as SQLiteDefaultColumnConstraint;
                        if (defcon != null)
                        {
                            // We don't allow default value of CURRENT_TIME, CURRENT_DATE or CURRENT_TIMESTAMP
                            if (defcon.Term != null && defcon.Term.AsTimeFunction.HasValue)
                                return false;
                            if (defcon.Expression != null)
                            {
                                // ALTER TABLE won't accept ADD COLUMN when the column has a DEFAULT clause
                                // with non-constant or constant-but-null value.
                                if (!defcon.Expression.IsConstant(false))
                                    return false;
                            }
                        } // if
                    } // foreach
                } // if
            } // for

            // Prepare the list of columns that need to be added to the table schema.
            added = new List<SQLiteColumnStatement>();
            for (int i = otable.Columns.Count; i < utable.Columns.Count; i++)
                added.Add(utable.Columns[i]);

            return true;
        }

        /// <summary>
        /// Used to check if the specified column has a NOT NULL constraint but no
        /// suitable DEFAULT clause. Such columns may cause problems when re-creating
        /// a table from scratch because we cannot fill them with NULL values if they
        /// don't matching column data in the original table.
        /// </summary>
        /// <param name="col">The column statement to check</param>
        /// <returns>TRUE if the specified column is NOT NULL without a proper DEFAULT
        /// clause.</returns>
        public static bool IsNotNullColumnWithoutDefault(SQLiteColumnStatement col)
        {
            return !col.IsNullable && !col.HasNonNullConstDefault;
        }

        /// <summary>
        /// Convenience method for creating temporary DB object names
        /// </summary>
        /// <param name="root">The root name</param>
        /// <returns>A temporary name based on the root name</returns>
        public static string GetTempName(string root)
        {
            if (root == null)
                return "temp_" + (new Random(_seed++).Next());
            else
                return "temp_" + Utils.NormalizeName(root) + "_" + (new Random(_seed++).Next());
        }

        /// <summary>
        /// Used to create a SELECT list of columns whose values are either NULL (if the column
        /// is NULLABLE and has no NON-NULL-DEFAULT value) or a DEFAULT non-null value. This
        /// list is can be used to construct INSERT statements.
        /// Example: INSERT INTO table1 SELECT NULL as extra, (1) AS id
        /// </summary>
        /// <param name="ncols">The list of columns that either have non-null DEFAULT constraint or
        /// are NULLable</param>
        /// <returns>A string that contains the list of columns with their value assignment (either NULL
        /// or a DEFAULT non-null value)</returns>
        public static string BuildNullableOrNonNullConstantDefaultSelectList(List<SQLiteColumnStatement> ncols)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ncols.Count; i++)
            {
                SQLiteColumnStatement c = ncols[i];
                if (c.HasNonNullConstDefault)
                {
                    SQLiteDefaultColumnConstraint condef = c.GetDefaultConstraint();
                    sb.Append("(" + condef.ValueString + ") AS " + c.ObjectName.ToString());
                }
                else if (c.IsNullable)
                {
                    sb.Append("NULL AS " + c.ObjectName.ToString());
                }
                else
                    continue;
                if (i < ncols.Count - 1)
                    sb.Append(",");
            } // for
            return sb.ToString();
        }

        public static string BuildCastedSelectList(List<SQLiteColumnStatement> common, List<SQLiteColumnStatement> diffcols)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < common.Count; i++)
            {
                bool found = false;
                SQLiteColumnStatement col = common[i];
                if (diffcols != null)
                {
                    foreach (SQLiteColumnStatement dcol in diffcols)
                    {
                        if (col.ObjectName.Equals(dcol.ObjectName))
                        {
                            found = true;

                            // The column changed its type so we need to add an appropriate CAST expression
                            sb.Append("CAST(" + col.ObjectName.ToString() + " as " + GetGeneralColumnType(dcol.ColumnType) + ") as " + col.ObjectName.ToString());
                            break;
                        }
                    } // foreach
                }
                if (!found)
                    sb.Append(col.ObjectName.ToString());
                if (i < common.Count - 1)
                    sb.Append(", ");
            } // for
            return sb.ToString();
        }

        private static string GetGeneralColumnType(SQLiteColumnType ctype)
        {
            string tp = ctype.TypeName;
            if (tp == null)
                return "BLOB";
            else if (tp.Contains("int"))
                return "INTEGER";
            else if (tp.Contains("real") || tp.Contains("doub") || tp.Contains("floa"))
                return "FLOAT";
            else if (tp.Contains("char") || tp.Contains("clob") || tp.Contains("text"))
                return "TEXT";
            else
                return "TEXT";
        }

        /// <summary>
        /// Checks if any of the table indexes has changed/added/removed between the original
        /// version and the updated version.
        /// </summary>
        /// <param name="tableName">The table name to check</param>
        /// <param name="origIndexes">The dictionary that includes all the original table indexes</param>
        /// <param name="updIndexes">The dictionary that includes all the updated table indexes</param>
        /// <param name="changedIndexes">In case changes were found - it will contain the list of 
        /// all indexes that were </param>
        /// <returns></returns>
        public static bool TableIndexesWereChanged(string tableName,
            Dictionary<string, SQLiteDdlStatement> origIndexes,
            Dictionary<string, SQLiteDdlStatement> updIndexes,
            out List<SQLiteCreateIndexStatement> changedIndexes,
            out List<SQLiteObjectName> removedIndexes)
        {
            changedIndexes = null;
            removedIndexes = null;
            foreach (SQLiteCreateIndexStatement idx in origIndexes.Values)
            {
                if (SQLiteParser.Utils.Chop(idx.OnTable).ToLower() ==
                    SQLiteParser.Utils.Chop(tableName).ToLower())
                {
                    // Go look for the same index in the updated schema. If not found - 
                    // add its naem to the 'removedIndexes' list.
                    bool found = false;
                    foreach (SQLiteCreateIndexStatement idx2 in updIndexes.Values)
                    {
                        if (idx2.ObjectName.Equals(idx.ObjectName))
                        {
                            found = true;

                            // Object was found in the updated schema - check if the two indexes
                            // are equal
                            if (!idx2.Equals(idx))
                            {
                                if (changedIndexes == null)
                                    changedIndexes = new List<SQLiteCreateIndexStatement>();
                                changedIndexes.Add(idx2);
                            }

                            break;
                        }
                    } // foreach

                    if (!found)
                    {
                        // The updated schema does not contain this index so it was removed.
                        if (removedIndexes == null)
                            removedIndexes = new List<SQLiteObjectName>();
                        removedIndexes.Add(idx.ObjectName);
                    } // if
                } // if
            } // foreach

            // Now scan from the direction of the updated schema - see if we can find an index
            // that exists in the updated schema but not in the original schema.
            foreach (SQLiteCreateIndexStatement idx in updIndexes.Values)
            {
                if (SQLiteParser.Utils.Chop(idx.OnTable).ToLower() ==
                    SQLiteParser.Utils.Chop(tableName).ToLower())
                {
                    // Go look for the same index in the original schema. If not found -
                    // add it to the changedIndexes list
                    bool found = false;
                    foreach (SQLiteCreateIndexStatement idx2 in origIndexes.Values)
                    {
                        if (idx2.ObjectName.Equals(idx.ObjectName))
                        {
                            found = true;
                            break;
                        }
                    } // foreach

                    if (!found)
                    {
                        // The original schema does not contain this index so it was added
                        if (changedIndexes == null)
                            changedIndexes = new List<SQLiteCreateIndexStatement>();
                        changedIndexes.Add(idx);
                    } // if
                } // if
            } // foreach

            if (changedIndexes == null && removedIndexes == null)
                return false;
            if (changedIndexes != null && removedIndexes != null)
                return true;
            if (changedIndexes == null)
                changedIndexes = new List<SQLiteCreateIndexStatement>();
            else
                removedIndexes = new List<SQLiteObjectName>();
            return true;
        }

        /// <summary>
        /// Checks if any of the table triggers has changed/added/removed between the original
        /// version and the updated version.
        /// </summary>
        /// <param name="tableName">The table name to check</param>
        /// <param name="origTriggers">The dictionary that includes all the original table indexes</param>
        /// <param name="updTriggers">The dictionary that includes all the updated table indexes</param>
        /// <param name="changedTriggers">In case changes were found - it will contain the list of
        /// all indexes that were</param>
        /// <param name="removedTriggers">The removed triggers.</param>
        /// <returns></returns>
        public static bool TableTriggersWereChanged(string tableName,
            Dictionary<string, SQLiteDdlStatement> origTriggers,
            Dictionary<string, SQLiteDdlStatement> updTriggers,
            out List<SQLiteCreateTriggerStatement> changedTriggers,
            out List<SQLiteObjectName> removedTriggers)
        {
            changedTriggers = null;
            removedTriggers = null;
            foreach (SQLiteCreateTriggerStatement trg in origTriggers.Values)
            {
                if (SQLiteParser.Utils.Chop(trg.TableName.ToString()).ToLower() ==
                    SQLiteParser.Utils.Chop(tableName).ToLower())
                {
                    // Go look for the same trigger in the updated schema. If not found - 
                    // add its naem to the 'removedTriggers' list.
                    bool found = false;
                    foreach (SQLiteCreateTriggerStatement trg2 in updTriggers.Values)
                    {
                        if (trg2.ObjectName.Equals(trg.ObjectName))
                        {
                            found = true;

                            // Object was found in the updated schema - check if the two triggers
                            // are equal
                            if (!trg2.Equals(trg))
                            {
                                if (changedTriggers == null)
                                    changedTriggers = new List<SQLiteCreateTriggerStatement>();
                                changedTriggers.Add(trg2);
                            }

                            break;
                        }
                    } // foreach

                    if (!found)
                    {
                        // The updated schema does not contain this trigger so it was removed.
                        if (removedTriggers == null)
                            removedTriggers = new List<SQLiteObjectName>();
                        removedTriggers.Add(trg.ObjectName);
                    } // if
                } // if
            } // foreach

            // Now scan from the direction of the updated schema - see if we can find an trigger
            // that exists in the updated schema but not in the original schema.
            foreach (SQLiteCreateTriggerStatement trg in updTriggers.Values)
            {
                if (SQLiteParser.Utils.Chop(trg.TableName.ToString()).ToLower() ==
                    SQLiteParser.Utils.Chop(tableName).ToLower())
                {
                    // Go look for the same trigger in the original schema. If not found -
                    // add it to the changedTriggers list
                    bool found = false;
                    foreach (SQLiteCreateTriggerStatement trg2 in origTriggers.Values)
                    {
                        if (trg2.ObjectName.Equals(trg.ObjectName))
                        {
                            found = true;
                            break;
                        }
                    } // foreach

                    if (!found)
                    {
                        // The original schema does not contain this index so it was added
                        if (changedTriggers == null)
                            changedTriggers = new List<SQLiteCreateTriggerStatement>();
                        changedTriggers.Add(trg);
                    } // if
                } // if
            } // foreach

            if (changedTriggers == null && removedTriggers == null)
                return false;
            if (changedTriggers != null && removedTriggers != null)
                return true;
            if (changedTriggers == null)
                changedTriggers = new List<SQLiteCreateTriggerStatement>();
            else
                removedTriggers = new List<SQLiteObjectName>();
            return true;
        }


        /// <summary>
        /// Creates a new CREATE-TABLE statement from the right table schema object
        /// by changing columns order to match the columns from the left table schema object.
        /// </summary>
        /// <returns>An updated CREATE-TABLE schema object whose columns are in the same order
        /// as the columns of the left table.</returns>
        public static SQLiteCreateTableStatement ReOrderTableColumns(
            SQLiteCreateTableStatement basis, SQLiteCreateTableStatement reordered)
        {
            if (basis.Columns == null || reordered.Columns == null)
                return (SQLiteCreateTableStatement)reordered.Clone();

            List<SQLiteColumnStatement> basisCols = basis.Columns;
            List<SQLiteColumnStatement> reorderedCols = reordered.Columns;

            List<SQLiteColumnStatement> extra = new List<SQLiteColumnStatement>();
            List<SQLiteColumnStatement> clist = new List<SQLiteColumnStatement>();
            for (int i = 0; i < basisCols.Count; i++)
            {
                SQLiteColumnStatement lcol = basisCols[i];
                foreach (SQLiteColumnStatement rcol in reorderedCols)
                {
                    if (rcol.ObjectName.Equals(lcol.ObjectName))
                    {
                        clist.Add((SQLiteColumnStatement)rcol.Clone());
                        break;
                    }
                } // foreach
            } // for
            foreach (SQLiteColumnStatement rcol in reorderedCols)
            {
                if (!Utils.ColumnListContains(clist, rcol))
                    extra.Add((SQLiteColumnStatement)rcol.Clone());
            } // foreach

            clist.AddRange(extra);

            SQLiteCreateTableStatement res = (SQLiteCreateTableStatement)reordered.Clone();
            res.Columns = clist;
            return res;
        }























        public static int GetDaysInMonth(int month, int year)
        {
            if (month < 1 || month > 12) throw new System.ArgumentOutOfRangeException("Месяц", month, "Месяц должен иметь значение от 1 до 12");

            if (1 == month || 3 == month || 5 == month || 7 == month || 8 == month || 10 == month || 12 == month) return 31;
            else if (2 == month)
            {
                if (0 == (year % 4))
                {
                    if (0 == (year % 400)) return 29;
                    else if (0 == (year % 100)) return 28;
                    return 29;
                }
                return 28;
            }
            return 30;
        }

        #region Проверка СНИЛСа
        /// <summary>
        /// Проверка СНИЛСа
        /// </summary>
        /// <param name="ssn">
        /// СНИЛС для проверки
        /// </param>
        /// <returns>
        /// Истина, если верен, ложь, если нет.
        /// </returns>
        public static bool ValidateSSN(string ssn)
        {
            string ssnR = ssn.Remove(11);
            int cd = Convert.ToInt32(ssn.Remove(0, ssn.Length - 2));

            int summ = 0;
            int os = 0;

            char[] ssnChar = ssnR.ToCharArray();
            List<string> ssnN = new List<string>();

            foreach (char item in ssnChar)
                if (item.ToString() != "-")
                    ssnN.Add(item.ToString());

            for (int i = 0; i < ssnN.Count; i++)
                summ += Convert.ToInt32(ssnN[i].ToString()) * (ssnN.Count - i);

            if (summ < 101 && summ != 100)
            {
                os = summ;
            }
            else if (summ == 100 || summ == 101)
            {
                os = 0;
            }
            else if (summ > 101)
            {
                summ = summ % 101;

                if (summ < 100)
                {
                    os = summ;
                }
                else if (summ == 100 || summ == 101)
                {
                    os = 0;
                }
            }

            return os == cd;
        }

        #endregion

        #region Получение проверочной части СНИЛСа
        /// <summary>
        /// Получение проверочной части СНИЛСа
        /// </summary>
        /// <param name="ssn">
        /// СНИЛС без проверочной части для расчета
        /// </param>
        /// <returns>
        /// Проверочное число
        /// </returns>
        public static string GetControlSumSSN(string ssn)
        {
            int summ = 0;
            int os = 0;

            char[] ssnChar = ssn.ToCharArray();
            List<string> ssnN = new List<string>();

            foreach (char item in ssnChar)
                if (item.ToString() != "-")
                    ssnN.Add(item.ToString());

            for (int i = 0; i < ssnN.Count; i++)
                summ += Convert.ToInt32(ssnN[i].ToString()) * (ssnN.Count - i);

            if (summ < 101 && summ != 100)
            {
                os = summ;
            }
            else if (summ == 100 || summ == 101)
            {
                os = 0;
            }
            else if (summ > 101)
            {
                summ = summ % 101;

                if (summ < 100)
                {
                    os = summ;
                }
                else if (summ == 100 || summ == 101)
                {
                    os = 0;
                }
            }

            string result = os.ToString().PadLeft(2, '0');

            return result;
        }

        #endregion

        /// <summary>
        /// Конвертация Рег.номера
        /// </summary>
        /// <param name="s">Рег.номер вида ############</param>
        /// <returns>Рег.номер вида ###-###-######</returns>
        public static string ParseRegNum(string s)
        {
            string reg = "";

            if (!String.IsNullOrEmpty(s) && s.Length == 12)
                reg = s.Substring(0, 3) + "-" + s.Substring(3, 3) + "-" + s.Substring(6, 6);

            return reg;
        }

        public static string decToStr(decimal? sum)
        {
            if (!sum.HasValue)
                sum = 0;

            string result = Math.Round(sum.Value, 2).ToString().Replace(',', '.');
            if (!result.Contains("."))
            {
                result = result + ".00";
            }
            else if ((result.Length - 1 - result.IndexOf('.')) < 2)
            {
                result = result + "0";
            }

            return result;
        }

        /// <summary>
        /// Оставляем в строке только цифры
        /// </summary>
        /// <param name="sa">исходная строка - цифры + буквы</param>
        /// <returns></returns>
        public static string removeCharsFromString(string sa)
        {
            // регулярное выражение - "брать только цыфры"
            Regex REG = new Regex("[0-9]*");
            // ищем совпадения в исходной строке
            MatchCollection mc = REG.Matches(sa);

            StringBuilder sb = new StringBuilder();
            // создаем строку результата
            foreach (Match matc in mc)
                sb.Append(matc.Value);
            // а это твой результат - только цифры
            string onlyDigits = sb.ToString();
            return (onlyDigits);
        }

        /// <summary>
        /// Конвертация СНИЛС
        /// </summary>
        /// <param name="num"></param>
        /// <param name="cn">СНИЛС ###-###-### ##</param>
        /// <returns></returns>
        public static string ParseSNILS(string num, short? cn)
        {
            string snils = !String.IsNullOrEmpty(num) ? (num.PadLeft(9, '0').Substring(0, 3) + "-" + num.PadLeft(9, '0').Substring(3, 3) + "-" + num.PadLeft(9, '0').Substring(6, 3) + " " + ((cn.HasValue && cn.Value != 100) ? cn.Value.ToString().PadLeft(2, '0') : "")) : "";

            return snils;
        }


        public static PU.Models.SNILSObject ParseSNILS_XML(string num, bool parseCN)
        {
            PU.Models.SNILSObject snils = new Models.SNILSObject();
            string InsuranceNum = "";

            if (num.Contains(' ')) // если снилс вида 999-999-999 99
            {
                var insn = num.Trim().PadLeft(14, '0').Split(' ');
                InsuranceNum = insn[0];

                if (parseCN)
                    try
                    {
                        snils.contrNum = byte.Parse(insn[1]);
                    }
                    catch
                    { }
            }
            else // иначе прдполагаем то 999-999-999-99
            {
                var insn = num.Trim().PadLeft(14, '0');
                if (insn.ElementAt(11) == '-')
                {
                    InsuranceNum = insn.Substring(0, 11);

                    if (parseCN)
                        try
                        {
                            snils.contrNum = byte.Parse(insn.Substring(12, 2));
                        }
                        catch
                        { }
                }

            }

            while (InsuranceNum.Contains("-"))
                InsuranceNum = InsuranceNum.Remove(InsuranceNum.IndexOf('-'), 1);

            snils.num = InsuranceNum.PadLeft(9, '0');

            return snils;
        }

        private static readonly Type IEnumerableType = typeof(IEnumerable);
        private static readonly Type StringType = typeof(string);
        /// <summary>
        /// Копирование записи LINQ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(this T source)
        {


            var clone = (T)Activator.CreateInstance(typeof(T));

            foreach (var prop in source.GetType().GetProperties().Where(x => !x.Name.Contains("Reference") && x.Name != "EntityState" && x.Name != "EntityKey" && !x.PropertyType.FullName.Contains("EntityCollection")))
            {
                prop.SetValue(clone, prop.GetValue(source, null), null);
            }

            return clone;
        }

        public static T SetZeroValues<T>(this T source)
        {
            foreach (var prop in source.GetType().GetProperties())
            {
                string itemName = prop.Name.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    string type = prop.PropertyType.FullName;
                    type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                    type = type.Split(',')[0].Split('.')[1].ToLower();
                    switch (type)
                    {
                        case "decimal":
                            prop.SetValue(source, (decimal)0, null);
                            break;
                        case "integer":
                            prop.SetValue(source, (int)0, null);
                            break;
                        case "int64":
                            prop.SetValue(source, (long)0, null);
                            break;
                        //case "string":
                        //    prop.SetValue(source, "", null);
                        //    break;
                    }

                }
            }
            return source;
        }

        /// <summary>
        /// Проверка установлен ли в системе VfpOleDb
        /// </summary>
        /// <returns></returns>
        public static bool CheckVfpOleDb()
        {
            bool result = Registry.ClassesRoot.OpenSubKey("TypeLib\\{50BAEECA-ED25-11D2-B97B-000000000000}") != null;

            if (result)
            {
                Registry.ClassesRoot.Close();
            }

            return result;
        }

        public static string CorrectUNCpathToSqlite(string path)
        {
            string correctPath = (path.StartsWith(@"\\") && !path.StartsWith(@"\\\") && !path.StartsWith(@"\\\\")) ? path.Insert(0, @"\\") : path;


            return correctPath;
        }


    }
}
