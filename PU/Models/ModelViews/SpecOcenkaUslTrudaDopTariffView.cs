using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    [DisplayName("Дополнительные тарифы для специальной оценки условий труда")]
    public class SpecOcenkaUslTrudaDopTariffView : BaseDictionaryActions
    {


        [DisplayVisible(IsVisible =false)]
        public long ID { get; set; }

        [DisplayVisible(IsVisible =false)]
        public byte Type { get; set; }

        [DisplayName("Тариф взносов")]
        [DisplayWidth(MaxWidth = 120)]
        public decimal? Rate { get; set; }


        [DisplayName("Тип")]
        public string TypeString {
            get
            {
                if (this.Type == 0)
                {
                    return "Спецоценка";
                }
                else {
                    return "Аттестация рабочих мест";
                }
            }

            set
            {
                if (value == "Спецоценка")
                {
                    this.Type = 0;
                }
                else {

                    this.Type = 1;
                }

            } }

        


        [DisplayName("Начало")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DisplayWidth(MaxWidth = 80)]
        [Filtrable(false)]
        public DateTime? DateBegin { get; set; }


        [DisplayName("Конец")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DisplayWidth(MaxWidth = 80)]
        [Filtrable(false)]
        public DateTime? DateEnd { get; set; }


        [DisplayVisible(IsVisible =false)]
        public long? SpecOcenkaUslTrudaID { get; set; }


        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname) { }

        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }

        [DisplayVisible(IsVisible =false)]
        public override void Close(Form form, RadGridView radGridView, string classname) { }

        [DisplayVisible(IsVisible =false)]
        public override void Synchronization(Form form, RadGridView radGridView, string classname) { }


    }
}
