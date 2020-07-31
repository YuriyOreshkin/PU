using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PU.Models;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.IO;
using PU.Classes;

namespace PU.FormsRSW2014
{
	public partial class Diction : Telerik.WinControls.UI.RadForm
	{
		private pu6Entities db = new pu6Entities();

		public SpecOcenkaUslTruda ocenka { get; set; }
		public string DictName;
        public string action { get; set; }
        public string Code { get; set; }
        public long id_izmer_ed { get; set; }
		public long item_id { get; set; }

		public Diction()
		{
			InitializeComponent();
			SelfRef = this;
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


		public static Diction SelfRef
		{
			get;
			set;
		}

		#region Обновление данных в таблице
		public void dataGrid_upd()
		{
			IEnumerable<DictionContainer> ds = null;

			switch (DictName)
			{
				case "TerrUsl":
					ds = db.TerrUsl.OrderBy(x => x.Code).Select(x => new DictionContainer
					{
						ID = x.ID,
						Code = x.Code,
						Name = x.Name,
						DateBegin = x.DateBegin,
						DateEnd = x.DateEnd
					});
					break;
				case "IschislStrahStajOsn":
					ds = db.IschislStrahStajOsn.OrderBy(x => x.Code).Select(x => new DictionContainer
					{
						ID = x.ID,
						Code = x.Code,
						Name = x.Name,
						DateBegin = x.DateBegin,
						DateEnd = x.DateEnd
					});
					break;
				case "UslDosrNazn":
					ds = db.UslDosrNazn.OrderBy(x => x.Code).Select(x => new DictionContainer
					{
						ID = x.ID,
						Code = x.Code,
						Name = x.Name,
						DateBegin = x.DateBegin,
						DateEnd = x.DateEnd
					});
					break;
				case "OsobUslTruda":
					ds = db.OsobUslTruda.OrderBy(x => x.Code).Select(x => new DictionContainer
					{
						ID = x.ID,
						Code = x.Code,
						Name = x.Name,
						DateBegin = x.DateBegin,
						DateEnd = x.DateEnd
					});
					break;
				case "VidTrudDeyat":
					ds = db.VidTrudDeyat.OrderBy(x => x.Code).Select(x => new DictionContainer
					{
						ID = x.ID,
						Code = x.Code,
						Name = x.Name,
						DateBegin = x.DateBegin,
						DateEnd = x.DateEnd
					});
					break;
				case "SpecOcenkaUslTruda":
					ds = db.SpecOcenkaUslTruda.OrderBy(x => x.Code).Select(x => new DictionContainer
					{
						ID = x.ID,
						Code = x.Code,
						Name = x.Name,
						DateBegin = x.DateBegin,
						DateEnd = x.DateEnd
					});
					break;
				case "IschislStrahStajDop":
					ds = db.IschislStrahStajDop.OrderBy(x => x.Code).Select(x => new DictionContainer
					{
						ID = x.ID,
						Code = x.Code,
						Name = x.Name,
						DateBegin = x.DateBegin,
						DateEnd = x.DateEnd
					});
					break;
			}



			BindingSource b = new BindingSource();
			b.DataSource = ds;
			radGridView1.DataSource = null;
			radGridView1.DataSource = b;

			//  radGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

			radGridView1.Columns["ID"].IsVisible = false;
			radGridView1.Columns["Code"].HeaderText = "Код";
			radGridView1.Columns["Code"].Width = radGridView1.Width / 10 * 2;

			radGridView1.Columns["Name"].HeaderText = "Наименование";
			radGridView1.Columns["Name"].Width = radGridView1.Width / 10 * 4;

			radGridView1.Columns["DateBegin"].HeaderText = "Начало";
			radGridView1.Columns["DateBegin"].Width = radGridView1.Width / 10 * 2;
			radGridView1.Columns["DateBegin"].FormatString = "{0:dd.MM.yyyy}";

			radGridView1.Columns["DateEnd"].HeaderText = "Конец";
			radGridView1.Columns["DateEnd"].Width = radGridView1.Width / 10 * 2 - 3;
			radGridView1.Columns["DateEnd"].FormatString = "{0:dd.MM.yyyy}";


			radGridView1.Refresh();

		}
		#endregion

		#region Добавление записи

		private void radButton1_Click(object sender, EventArgs e)
		{
			DictionEdit child = new DictionEdit();
			child.Owner = this;
			child.action = "add";

			if (DictName == "UslDosrNazn")
			{
				child.radLabel4.Visible = true;
				child.radDropDownList1.Visible = true;

				var EdIzmList = db.UslDosrNaznEdIzm;

				child.radDropDownList1.DataSource = null;
				child.radDropDownList1.Items.Clear();
				child.radDropDownList1.Items.Add(new RadListDataItem("",0));
				foreach (var item in EdIzmList)
				{
					child.radDropDownList1.Items.Add(new RadListDataItem(item.Name, item.ID));
				}

				child.radDropDownList1.SelectedIndex = 0;

			}


			child.ThemeName = this.ThemeName;
			child.ShowInTaskbar = false;
			child.ShowDialog();
		}

		public string add(DictionContainer dc)
		{
			string result = "";
			bool checkExist = false;

			int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;

			try
			{
				switch (DictName)
				{
					#region Территориальные условия
					case "TerrUsl":
						if (!db.TerrUsl.Any(x => x.Code == dc.Code && x.Name == dc.Name))
						{
							TerrUsl newItem = new TerrUsl()
							{
								Code = dc.Code,
								Name = dc.Name
							};
							if (dc.DateBegin != null)
							{
								newItem.DateBegin = dc.DateBegin.Value.Date;
							}
							if (dc.DateEnd != null)
							{
								newItem.DateEnd = dc.DateEnd.Value.Date;
							}
							db.TerrUsl.AddObject(newItem);
						}
						else
							checkExist = true;
						break;
					#endregion
					#region Исчисление страхового стажа: Основание
					case "IschislStrahStajOsn":
						if (!db.IschislStrahStajOsn.Any(x => x.Code == dc.Code && x.Name == dc.Name))
						{
							IschislStrahStajOsn newItem = new IschislStrahStajOsn()
							{
								Code = dc.Code,
								Name = dc.Name
							};
							if (dc.DateBegin != null)
							{
								newItem.DateBegin = dc.DateBegin.Value.Date;
							}
							if (dc.DateEnd != null)
							{
								newItem.DateEnd = dc.DateEnd.Value.Date;
							}
							db.IschislStrahStajOsn.AddObject(newItem);
						}
						else
							checkExist = true;
						break;
					#endregion
					#region Условия для досрочного назначения Трудовой пенсии: Основание
					case "UslDosrNazn":
						if (!db.UslDosrNazn.Any(x => x.Code == dc.Code && x.Name == dc.Name))
						{
							UslDosrNazn newItem = new UslDosrNazn()
							{
								Code = dc.Code,
								Name = dc.Name
							};
							if (dc.DateBegin != null)
							{
								newItem.DateBegin = dc.DateBegin.Value.Date;
							}
							if (dc.DateEnd != null)
							{
								newItem.DateEnd = dc.DateEnd.Value.Date;
							}

							if (id_izmer_ed != 0)
							{
								newItem.EdIzmID = id_izmer_ed;
							}

							db.UslDosrNazn.AddObject(newItem);
						}
						else
							checkExist = true;
						break;
					#endregion
					#region Особые условия труда
					case "OsobUslTruda":
						if (!db.OsobUslTruda.Any(x => x.Code == dc.Code && x.Name == dc.Name))
						{
							OsobUslTruda newItem = new OsobUslTruda()
							{
								Code = dc.Code,
								Name = dc.Name
							};
							if (dc.DateBegin != null)
							{
								newItem.DateBegin = dc.DateBegin.Value.Date;
							}
							if (dc.DateEnd != null)
							{
								newItem.DateEnd = dc.DateEnd.Value.Date;
							}
							db.OsobUslTruda.AddObject(newItem);
						}
						else
							checkExist = true;
						break;
					#endregion
					#region Виды трудовой или иной общественно полезной деятельности
					case "VidTrudDeyat":
						if (!db.VidTrudDeyat.Any(x => x.Code == dc.Code && x.Name == dc.Name))
						{
							VidTrudDeyat newItem = new VidTrudDeyat()
							{
								Code = dc.Code,
								Name = dc.Name
							};
							if (dc.DateBegin != null)
							{
								newItem.DateBegin = dc.DateBegin.Value.Date;
							}
							if (dc.DateEnd != null)
							{
								newItem.DateEnd = dc.DateEnd.Value.Date;
							}
							db.VidTrudDeyat.AddObject(newItem);
						}
						else
							checkExist = true;
						break;
					#endregion
					#region Специальная оценка условий труда
					case "SpecOcenkaUslTruda":
						if (!db.SpecOcenkaUslTruda.Any(x => x.Code == dc.Code && x.Name == dc.Name))
						{
							SpecOcenkaUslTruda newItem = new SpecOcenkaUslTruda()
							{
								Code = dc.Code,
								Name = dc.Name
							};
							if (dc.DateBegin != null)
							{
								newItem.DateBegin = dc.DateBegin.Value.Date;
							}
							if (dc.DateEnd != null)
							{
								newItem.DateEnd = dc.DateEnd.Value.Date;
							}
							db.SpecOcenkaUslTruda.AddObject(newItem);
						}
						else
							checkExist = true;
						break;
					#endregion
					#region Исчисление страхового стажа: Доп. сведения
					case "IschislStrahStajDop":
						if (!db.IschislStrahStajDop.Any(x => x.Code == dc.Code && x.Name == dc.Name))
						{
							IschislStrahStajDop newItem = new IschislStrahStajDop()
							{
								Code = dc.Code,
								Name = dc.Name
							};
							if (dc.DateBegin != null)
							{
								newItem.DateBegin = dc.DateBegin.Value.Date;
							}
							if (dc.DateEnd != null)
							{
								newItem.DateEnd = dc.DateEnd.Value.Date;
							}
							db.IschislStrahStajDop.AddObject(newItem);
						}
						else
							checkExist = true;
						break;
					#endregion
				}


				if (!checkExist)
				{
					db.SaveChanges();
					dataGrid_upd();
					radGridView1.Rows[rowindex].IsCurrent = true;
				}
				else
					result = "Запись с указанными параметрами уже есть в БД";

			}
			catch (Exception e)
			{
				result = e.Message;
			}

			return result;
		}

		#endregion

		#region Редактирование записи
		private void radButton2_Click(object sender, EventArgs e)
		{
			if (radGridView1.RowCount != 0)
			{
				int rowindex = radGridView1.CurrentRow.Index;

				DictionEdit child = new DictionEdit();
				child.Owner = this;
				child.ThemeName = this.ThemeName;
				child.radTextBox1.Text = radGridView1.Rows[rowindex].Cells[1].Value.ToString();
				child.radTextBox2.Text = radGridView1.Rows[rowindex].Cells[2].Value.ToString();

                DateTime start;
                DateTime end;
                if (radGridView1.Rows[rowindex].Cells[3].Value != null)
                    if (DateTime.TryParse(radGridView1.Rows[rowindex].Cells[3].Value.ToString(), out start))
                        child.dateBegin.EditValue = start;
                if (radGridView1.Rows[rowindex].Cells[4].Value != null)
                    if (DateTime.TryParse(radGridView1.Rows[rowindex].Cells[4].Value.ToString(), out end))
                        child.dateEnd.EditValue = end;

				if (DictName == "UslDosrNazn")
				{
					child.radLabel4.Visible = true;
					child.radDropDownList1.Visible = true;
					long id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
					UslDosrNazn udn = db.UslDosrNazn.FirstOrDefault(x => x.ID == id);

					var EdIzmList = db.UslDosrNaznEdIzm;

					child.radDropDownList1.DataSource = null;
					child.radDropDownList1.Items.Clear();
					child.radDropDownList1.Items.Add(new RadListDataItem("", 0));
					foreach (var item in EdIzmList)
					{
						child.radDropDownList1.Items.Add(new RadListDataItem(item.Name,item.ID));
					}

					if (udn.EdIzmID.HasValue)
					{
						child.radDropDownList1.SelectedItem = child.radDropDownList1.Items.First(x => x.Value.ToString() == udn.EdIzmID.Value.ToString());
					}
					else
					{
						child.radDropDownList1.SelectedIndex = 0;
					}


				}

				child.action = "edit";

				//= Convert.ToInt32(radGridView1.Rows[rowindex].Cells[0].Value);
				child.ShowInTaskbar = false;

				child.ShowDialog();

			}
			else
			{
				Telerik.WinControls.RadMessageBox.SetThemeName(radGridView1.ThemeName);
				RadMessageBox.Show(this, "Нет данных для редактирования!", "Ошибка"); 
			}

		}

		public string edit(DictionContainer dc)
		{

			string result = "";
			bool checkExist = false;

			int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
			long ID = 0;
			try
			{

				switch (DictName)
				{
					#region Территориальные условия
					case "TerrUsl":
						ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

						if (!db.TerrUsl.Any(x => x.Code == dc.Code && x.Name == dc.Name && x.ID != ID))
						{
							TerrUsl Item = db.TerrUsl.FirstOrDefault(x => x.ID == ID);
							Item.Code = dc.Code;
							Item.Name = dc.Name;

							if (dc.DateBegin != null)
							{
								Item.DateBegin = dc.DateBegin.Value.Date;
							}
							else
								Item.DateBegin = null;
							if (dc.DateEnd != null)
							{
								Item.DateEnd = dc.DateEnd.Value.Date;
							}
							else
								Item.DateEnd = null;

							db.ObjectStateManager.ChangeObjectState(Item, EntityState.Modified);
						}
						else
						{
							checkExist = true;
						}
						break;
					#endregion
					#region Исчисление страхового стажа: Основание
					case "IschislStrahStajOsn":
						ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

						if (!db.IschislStrahStajOsn.Any(x => x.Code == dc.Code && x.Name == dc.Name && x.ID != ID))
						{
							IschislStrahStajOsn Item = db.IschislStrahStajOsn.FirstOrDefault(x => x.ID == ID);
							Item.Code = dc.Code;
							Item.Name = dc.Name;

							if (dc.DateBegin != null)
							{
								Item.DateBegin = dc.DateBegin.Value.Date;
							}
							else
								Item.DateBegin = null;
							if (dc.DateEnd != null)
							{
								Item.DateEnd = dc.DateEnd.Value.Date;
							}
							else
								Item.DateEnd = null;

							db.ObjectStateManager.ChangeObjectState(Item, EntityState.Modified);
						}
						else
						{
							checkExist = true;
						}
						break;
					#endregion
					#region Условия для досрочного назначения Трудовой пенсии: Основание
					case "UslDosrNazn":
						ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

						if (!db.UslDosrNazn.Any(x => x.Code == dc.Code && x.Name == dc.Name && x.ID != ID))
						{
							UslDosrNazn Item = db.UslDosrNazn.FirstOrDefault(x => x.ID == ID);
							Item.Code = dc.Code;
							Item.Name = dc.Name;

							if (dc.DateBegin != null)
							{
								Item.DateBegin = dc.DateBegin.Value.Date;
							}
							else
								Item.DateBegin = null;
							if (dc.DateEnd != null)
							{
								Item.DateEnd = dc.DateEnd.Value.Date;
							}
							else
								Item.DateEnd = null;

							if (id_izmer_ed != 0)
							{
								Item.EdIzmID = id_izmer_ed;
							}
							else
							{
								Item.EdIzmID = null;
							}

							db.ObjectStateManager.ChangeObjectState(Item, EntityState.Modified);
						}
						else
						{
							checkExist = true;
						}
						break;
					#endregion
					#region Особые условия труда
					case "OsobUslTruda":
						ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

						if (!db.OsobUslTruda.Any(x => x.Code == dc.Code && x.Name == dc.Name && x.ID != ID))
						{
							OsobUslTruda Item = db.OsobUslTruda.FirstOrDefault(x => x.ID == ID);
							Item.Code = dc.Code;
							Item.Name = dc.Name;

							if (dc.DateBegin != null)
							{
								Item.DateBegin = dc.DateBegin.Value.Date;
							}
							else
								Item.DateBegin = null;
							if (dc.DateEnd != null)
							{
								Item.DateEnd = dc.DateEnd.Value.Date;
							}
							else
								Item.DateEnd = null;

							db.ObjectStateManager.ChangeObjectState(Item, EntityState.Modified);
						}
						else
						{
							checkExist = true;
						}
						break;
					#endregion
					#region Виды трудовой или иной общественно полезной деятельности
					case "VidTrudDeyat":
						ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

						if (!db.VidTrudDeyat.Any(x => x.Code == dc.Code && x.Name == dc.Name && x.ID != ID))
						{
							VidTrudDeyat Item = db.VidTrudDeyat.FirstOrDefault(x => x.ID == ID);
							Item.Code = dc.Code;
							Item.Name = dc.Name;

							if (dc.DateBegin != null)
							{
								Item.DateBegin = dc.DateBegin.Value.Date;
							}
							else
								Item.DateBegin = null;
							if (dc.DateEnd != null)
							{
								Item.DateEnd = dc.DateEnd.Value.Date;
							}
							else
								Item.DateEnd = null;

							db.ObjectStateManager.ChangeObjectState(Item, EntityState.Modified);
						}
						else
						{
							checkExist = true;
						}
						break;
					#endregion
					#region Специальная оценка условий труда
					case "SpecOcenkaUslTruda":
						ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

						if (!db.SpecOcenkaUslTruda.Any(x => x.Code == dc.Code && x.Name == dc.Name && x.ID != ID))
						{
							SpecOcenkaUslTruda Item = db.SpecOcenkaUslTruda.FirstOrDefault(x => x.ID == ID);
							Item.Code = dc.Code;
							Item.Name = dc.Name;

							if (dc.DateBegin != null)
							{
								Item.DateBegin = dc.DateBegin.Value.Date;
							}
							else
								Item.DateBegin = null;
							if (dc.DateEnd != null)
							{
								Item.DateEnd = dc.DateEnd.Value.Date;
							}
							else
								Item.DateEnd = null;

							db.ObjectStateManager.ChangeObjectState(Item, EntityState.Modified);
						}
						else
						{
							checkExist = true;
						}
						break;
					#endregion
					#region Исчисление страхового стажа: Доп. сведения
					case "IschislStrahStajDop":
						ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

						if (!db.IschislStrahStajDop.Any(x => x.Code == dc.Code && x.Name == dc.Name && x.ID != ID))
						{
							IschislStrahStajDop Item = db.IschislStrahStajDop.FirstOrDefault(x => x.ID == ID);
							Item.Code = dc.Code;
							Item.Name = dc.Name;

							if (dc.DateBegin != null)
							{
								Item.DateBegin = dc.DateBegin.Value.Date;
							}
							else
								Item.DateBegin = null;
							if (dc.DateEnd != null)
							{
								Item.DateEnd = dc.DateEnd.Value.Date;
							}
							else
								Item.DateEnd = null;

							db.ObjectStateManager.ChangeObjectState(Item, EntityState.Modified);
						}
						else
						{
							checkExist = true;
						}
						break;
					#endregion
				}


				if (!checkExist)
				{
					db.SaveChanges();
					dataGrid_upd();
					radGridView1.Rows[rowindex].IsCurrent = true;
				}
				else
					result = "Запись с указанными параметрами уже есть в БД";

			}
			catch (Exception e)
			{
				result = e.Message;
			}


			return result;
		}

		private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
		{
			switch (action)
			{
				case "selection":
					btnSelection_Click(null, null);
					break;
				default:
					radButton2_Click(null, null);
					break;
			}
		}

		#endregion

		#region Удаление
		private void radButton3_Click(object sender, EventArgs e)
		{
			if (radGridView1.RowCount != 0)
			{

				int rowindex = radGridView1.CurrentRow.Index;
				string title = radGridView1.Rows[rowindex].Cells[2].Value.ToString();
				long id = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
				DialogResult dialogResult = RadMessageBox.Show(this,"Уверены что хотите удалить текущую запись?", title, MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					try
					{
						rowindex = rowindex + 1 == radGridView1.RowCount ? rowindex - 1 : rowindex;

						switch (DictName)
						{
							#region Территориальные условия
							case "TerrUsl":
								TerrUsl Item1 = db.TerrUsl.FirstOrDefault(x => x.ID == id);
								db.TerrUsl.DeleteObject(Item1);
								break;
							#endregion
							#region Исчисление страхового стажа: Основание
							case "IschislStrahStajOsn":
								IschislStrahStajOsn Item2 = db.IschislStrahStajOsn.FirstOrDefault(x => x.ID == id);
								db.IschislStrahStajOsn.DeleteObject(Item2);
								break;
							#endregion
							#region Условия для досрочного назначения Трудовой пенсии: Основание
							case "UslDosrNazn":
								UslDosrNazn Item3 = db.UslDosrNazn.FirstOrDefault(x => x.ID == id);
								db.UslDosrNazn.DeleteObject(Item3);
								break;
							#endregion
							#region Особые условия труда
							case "OsobUslTruda":
								OsobUslTruda Item4 = db.OsobUslTruda.FirstOrDefault(x => x.ID == id);
								db.OsobUslTruda.DeleteObject(Item4);
								break;
							#endregion
							#region Виды трудовой или иной общественно полезной деятельности
							case "VidTrudDeyat":
								VidTrudDeyat Item5 = db.VidTrudDeyat.FirstOrDefault(x => x.ID == id);
								db.VidTrudDeyat.DeleteObject(Item5);
								break;
							#endregion
							#region Специальная оценка условий труда
							case "SpecOcenkaUslTruda":
								SpecOcenkaUslTruda Item6 = db.SpecOcenkaUslTruda.FirstOrDefault(x => x.ID == id);
								db.SpecOcenkaUslTruda.DeleteObject(Item6);
								break;
							#endregion
							#region Исчисление страхового стажа: Доп. сведения
							case "IschislStrahStajDop":
								IschislStrahStajDop Item7 = db.IschislStrahStajDop.FirstOrDefault(x => x.ID == id);
								db.IschislStrahStajDop.DeleteObject(Item7);
								break;
							#endregion
						}


						db.SaveChanges();
						dataGrid_upd();

						if (radGridView1.RowCount != 0)
						{
							radGridView1.Rows[rowindex].IsCurrent = true;
						}

					}
					catch (Exception ex)
					{
						RadMessageBox.Show(this, "При удалении данных произошла ошибка!   " + ex.Data, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error); 


					}

				}
				else if (dialogResult == DialogResult.No)
				{
					//do something else
				}

			}
			else
				RadMessageBox.Show(this,"Нет данных для удаления!");
		}

		#endregion

		private void radButton4_Click(object sender, EventArgs e)
		{
			this.Close();
		}



		private void Diction_Load(object sender, EventArgs e)
		{
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
			
			switch (DictName)
			{
				case "TerrUsl":
					this.Text = "Территориальные условия";
					break;
				case "IschislStrahStajOsn":
					this.Text = "Исчисление страхового стажа: Основание";
					break;
				case "UslDosrNazn":
					this.Text = "Условия для досрочного назначения Трудовой пенсии: Основание";
					break;
				case "OsobUslTruda":
					this.Text = "Особые условия труда";
					break;
				case "VidTrudDeyat":
					this.Text = "Виды трудовой или иной общественно полезной деятельности";
					break;
				case "SpecOcenkaUslTruda":
					this.Text = "Специальная оценка условий труда";
					this.tariffBtn.Visible = true;
					break;
				case "IschislStrahStajDop":
					this.Text = "Исчисление страхового стажа: Доп. сведения";
					break;
			}

			dataGrid_upd();
		}

		private void btnSelection_Click(object sender, EventArgs e)
		{
			if (action == "selection")
			{
				int rowindex = radGridView1.CurrentRow.Index;
				item_id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());

                try
                {
                    Code = radGridView1.Rows[rowindex].Cells["Code"].Value.ToString();
                }
                catch { }

				switch (DictName)
				{
					case "TerrUsl":
						break;
					case "IschislStrahStajOsn":
						break;
					case "UslDosrNazn":
						break;
					case "OsobUslTruda":
						break;
					case "VidTrudDeyat":
						break;
					case "SpecOcenkaUslTruda":
						ocenka = db.SpecOcenkaUslTruda.FirstOrDefault(x => x.ID == item_id);
						break;
					case "IschislStrahStajDop":
						break;
				}


				this.btnSelection.Visible = false;
				this.Close();
			}
		}

		private void tariffBtn_Click(object sender, EventArgs e)
		{
			switch (DictName)
			{
				case "TerrUsl":
					break;
				case "IschislStrahStajOsn":
					break;
				case "UslDosrNazn":
					break;
				case "OsobUslTruda":
					break;
				case "VidTrudDeyat":
					break;
				case "SpecOcenkaUslTruda":
					if (radGridView1.RowCount != 0)
					{
						int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
						long ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

						SpecOcenkaDopTariff child = new SpecOcenkaDopTariff();
						child.ThemeName = this.ThemeName;
						child.Owner = this;
						child.ShowInTaskbar = false;
						child.SpecOcenka = db.SpecOcenkaUslTruda.FirstOrDefault(x => x.ID == ID);
						child.ShowDialog();
					}
					else
					{
						RadMessageBox.Show(this, "Не выбрана запись", "");
					}
					break;
				case "IschislStrahStajDop":
					break;
			}




		}

		private void Diction_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.tariffBtn.Visible = false;
		}

        string path = "";
        bool synchResult = true;

        private void synchBtn_Click(object sender, EventArgs e)
        {
            path = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");
            if (File.Exists(path))
            {
                this.Cursor = Cursors.WaitCursor;

                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(synchronization);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                bw.RunWorkerAsync();

            }
            else
            {
                RadMessageBox.Show(this, "Не найдена эталонная база данных! Синхронизация остановлена!", "Ошибка синхронизации");
            }
        }

        private void synchronization(object sender, DoWorkEventArgs e)
        {
            synchResult = UpdateDictionaries.updateTable(DictName, path, this.ThemeName, this);
            if (DictName == "SpecOcenkaUslTruda" && synchResult)
            {
                synchResult = UpdateDictionaries.updateTable("SpecOcenkaUslTrudaDopTariff", path, this.ThemeName, this);
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;
            db = new pu6Entities();
            dataGrid_upd();
            if (synchResult)
            {
                Methods.showAlert("Синхронизация завершена", "Данные успешно синхронизированы!", this.ThemeName);
            }
            else
            {
 //               Methods.showAlert("Синхронизация завершена", "Во время синхронизации произошла ошибка!", this.ThemeName);
            }
        }

	}
}
