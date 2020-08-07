namespace PU.Models.ModelViews
{
    public class UslDosrNaznView : BaseDictionaryView
    {
        [DisplayVisible(false)]
        [LookUp(DataSource = "UslDosrNaznEdIzm", ValueMember  ="ID",DisplayMember ="Name")]
        public long? EdIzmID { get; set; }
    }
}
