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
        private bool isVisible;

        public DisplayVisibleAttribute(bool _isVisible)
        {
            isVisible = _isVisible;
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
        }
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

    public class LookUpAttribute : Attribute
    {
        public string DataSource { get; set; }
        public string ValueMember { get; set; }
        public string DisplayMember { get; set; }
    }
}
