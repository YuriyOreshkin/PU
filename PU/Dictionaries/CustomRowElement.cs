using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Telerik.WinControls.UI;

namespace PU.Dictionaries
{
    public class CustomGridFilterElement : GridFilterRowElement
    {
        protected override SizeF MeasureOverride(SizeF availableSize)
        {
            SizeF baseSize = base.MeasureOverride(availableSize);
        
            baseSize.Height = 25;
            return baseSize;
        }
    }

    public class CustomHeaderElement : GridTableHeaderRowElement
    {
        protected override SizeF MeasureOverride(SizeF availableSize)
        {
            SizeF baseSize = base.MeasureOverride(availableSize);
            baseSize.Height = 25;
            this.RowInfo.Height = 25;
            return baseSize;
        }
    }

        public class CustomRowElement : GridDataRowElement
    {
        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(GridDataRowElement);
            }
        }
        protected override SizeF MeasureOverride(SizeF availableSize)
        {
            int rowMinHeight = 22;
            SizeF baseSize = base.MeasureOverride(availableSize);
            CellElementProvider provider = new CellElementProvider(this.TableElement);
            SizeF desiredSize = SizeF.Empty;
            foreach (GridViewColumn column in this.ViewTemplate.Columns)
            {
                if (!this.IsColumnVisible(column))
                {
                    continue;
                }
                GridDataCellElement cellElement = provider.GetElement(column, this) as GridDataCellElement;
                this.Children.Add(cellElement);
                if (cellElement != null)
                {
                    cellElement.Measure(new SizeF(column.Width, float.PositiveInfinity));
                    if (desiredSize.Height < cellElement.DesiredSize.Height)
                    {
                        desiredSize.Height = cellElement.DesiredSize.Height;
                    }
                }
                cellElement.Detach();
                this.Children.Remove(cellElement);
            }
            SizeF elementSize = this.TableElement.RowScroller.ElementProvider.GetElementSize(this.RowInfo);
            
            int oldHeight = RowInfo.Height == -1 ? (int)elementSize.Height : RowInfo.Height;
            this.RowInfo.SuspendPropertyNotifications();
            this.RowInfo.Height = (int)desiredSize.Height > rowMinHeight ? (int)desiredSize.Height : rowMinHeight;
            
            this.RowInfo.ResumePropertyNotifications();
            if (!this.RowInfo.IsPinned)
            {
                int delta = RowInfo.Height - oldHeight;
                TableElement.RowScroller.UpdateScrollRange(TableElement.RowScroller.Scrollbar.Maximum + delta, false);
            }
            baseSize.Height = this.RowInfo.Height;
            return baseSize;
        }
        private bool IsColumnVisible(GridViewColumn column)
        {
            return column.IsVisible;
        }
    }
}
