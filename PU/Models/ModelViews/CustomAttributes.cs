using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PU.Models.ModelViews
{
    /// <summary>
    ///  Visibility
    /// </summary>
    public class DisplayVisibleAttribute : Attribute
    {
        public Separator Separator { get; set; }

        public bool IsVisible { set; get; }
      
    }

    /// <summary>
    /// WrapText
    /// </summary>
    public class WrapTextAttribute : Attribute
    {
        private bool wrapText;

        public WrapTextAttribute(bool _wrapText)
        {
            wrapText = _wrapText;
        }

        public bool WrapText
        {
            get
            {
                return wrapText;
            }
        }
    }
    /// <summary>
    /// Column width settings
    /// </summary>
    public class DisplayWidthAttribute : Attribute
    {
        public int MaxWidth { get; set; }
    }
    /// <summary>
    /// Filtrable
    /// </summary>
    public class FiltrableAttribute : Attribute
    {
        private bool isFiltrable;

        public FiltrableAttribute(bool _isFiltrable)
        {
            isFiltrable = _isFiltrable;
        }

        public bool IsFiltrable
        {
            get
            {
                return isFiltrable;
            }
        }
    }

    /// <summary>
    /// Look up field
    /// </summary>
    public class LookUpAttribute : Attribute
    {
        public string DataSource { get; set; }
        public string ValueMember { get; set; }
        public string DisplayMember { get; set; }
    }

    /// <summary>
    ///  ReadOnly
    /// </summary>
    public class ReadOnlyAttribute : Attribute
    {
        private bool isReadOnly;

        public ReadOnlyAttribute(bool _isReadOnly)
        {
            isReadOnly = _isReadOnly;
        }

        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
        }
    }

    /// <summary>
    /// DisplayFieldAttribute
    /// </summary>
    public class DisplayFieldAttribute : Attribute
    {
        private string fieldName;
        public DisplayFieldAttribute(string _fieldName)
        {
            fieldName = _fieldName;
        }

        public string FieldName { get { return fieldName; } }
    }

    public enum Separator
    {
        None,
        Left,
        Right,
        LeftRight
    }

   
}
