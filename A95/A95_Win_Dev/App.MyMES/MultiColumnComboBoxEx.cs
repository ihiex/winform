using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace App.MyMES
{

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Windows.Forms.ComboBox))]
    public class MultiColumnComboBoxEx : ComboBox
    {
        #region member fields

        private const int m_minColumnPadding = 1;
        private const int m_maxColumnPadding = 100;
        private const int m_leftOffset = 1;
        private const int m_bottomPadding = 2;

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONDOUBLECLICK = 0x0203;
        private const int WM_ADDITEM = 0x0143;
        private const int WM_DELETEITEM = 0x0144;

        private List<string> m_columnNames = new List<string>();
        private int[] m_columnWidths = new int[0];

        private int m_maxItemWidth = 20;
        private int m_itemDropDownHeight = 12;
        private int m_maxDropDownHeight = 12 + m_bottomPadding;
        private int m_minDropDownHeight = 12 + m_bottomPadding;
        private int m_columnPadding = 5;

        private string m_displayColumnNames = string.Empty;
        private string m_textDisplayed = string.Empty;

        private int m_valueMemberColumnIndex = -1;
        private int m_displayMemberColumnIndex = -1;

        private bool m_displayMultiColumnsInBox = false;
        private bool m_displayVerticalLine = true;

        private Pen m_gridLinePen = new Pen(SystemColors.GrayText);
        private SolidBrush m_foreColorBrush = new SolidBrush(Color.Black);
        private SolidBrush m_backColorBrush = new SolidBrush(Color.White);
        private StringFormat m_stringFormat = new StringFormat();

        private string m_version = "1.0";  // first version

        #endregion

        #region constructor

        public MultiColumnComboBoxEx()
        {
            base.ItemHeight = m_itemDropDownHeight;
            base.DropDownWidth = base.Width;
            m_maxItemWidth = base.Width;
            base.DrawMode = DrawMode.OwnerDrawFixed;  // 必须在 base.ItemHeight 设置语句之后

            this.SetDropDownHeight();

            m_stringFormat.LineAlignment = StringAlignment.Center;
            m_stringFormat.Alignment = StringAlignment.Near;  // horizonal alignment

            m_version = "1.2";  // new version
        }

        private void Release()  // release unmanaged resource
        {
            m_gridLinePen.Dispose();
            m_foreColorBrush.Dispose();
            m_backColorBrush.Dispose();
            m_stringFormat.Dispose();
        }

        /// <summary>
        /// http://msdn.microsoft.com/zh-cn/library/b1yfkh5e(VS.80).aspx design mode, override Dispose(bool) only.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)  // release managed resource
                {
                    m_columnNames.Clear();
                    m_columnWidths = null;
                }
                this.Release();  // release unmanaged resource
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion

        #region properties

        [Category("Custom")]
        [DefaultValue(false)]
        [Description("Display multi columns text in box, it is valid when DropDownList.")]
        public bool DisplayMultiColumnsInBox
        {
            get { return m_displayMultiColumnsInBox; }
            set
            {
                m_displayMultiColumnsInBox = value;
                this.Invalidate();
            }
        }

        [Category("Custom")]
        [DefaultValue(true)]
        [Description("Display vertical separate line when multi columns.")]
        public bool DisplayVerticalLine
        {
            get { return m_displayVerticalLine; }
            set
            {
                m_displayVerticalLine = value;
                this.Invalidate();
            }
        }

        [Category("Custom")]
        [DefaultValue("")]
        [Description("Display column names and orders separated by comma(,) or |, all when empty.")]
        public string DisplayColumnNames
        {
            get { return m_displayColumnNames; }
            set
            {
                if (string.IsNullOrEmpty(value) == true)
                {
                    m_displayColumnNames = string.Empty;
                }
                else
                {
                    m_displayColumnNames = value.Trim();
                }

                if (string.IsNullOrEmpty(m_displayColumnNames) == false)
                {
                    if (m_displayColumnNames.EndsWith(",") || m_displayColumnNames.EndsWith("|") || m_displayColumnNames[0] == ',' || m_displayColumnNames[0] == '|')
                    {
                        throw new NotSupportedException("Can not ends/begins with comma(,) or |");
                    }
                }

                this.SetDisplayedColumns();
                this.SetDisplayMemberColumn();
                this.SetValueMemberColumn();

                this.RefreshIMultiColumntems();  // call OnMeasureItem()
            }
        }

        [Description("Disalble this property.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]  // hide in property editor window.
        public new int ItemHeight
        {
            get { return base.ItemHeight; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("ItemHeight must be positive number.");
                }
                base.ItemHeight = value;
                this.SetDropDownHeight();
                this.RefreshIMultiColumntems();  // call OnMeasureItem()
            }
        }

        [Category("Custom")]
        [DefaultValue(12)]
        [Description("Height of ComboBox itself, which is used instead of ItemHeight.")]
        public int ComboBoxHeight
        {
            get { return this.ItemHeight; }
            set { this.ItemHeight = value; }
        }

        [Category("Custom")]
        [DefaultValue(12)]
        [Description("Item height of dropDown list item.")]
        public int ItemDropDownHeight
        {
            get { return m_itemDropDownHeight; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("ItemHeight must be positive number.");
                }
                m_itemDropDownHeight = value;
                this.SetDropDownHeight();
                this.RefreshIMultiColumntems();  // call OnMeasureItem()
            }
        }

        [Category("Custom")]
        [DefaultValue(5)]
        [Description("Column padding.")]
        public int ColumnPadding
        {
            get { return m_columnPadding; }
            set
            {
                if (value < m_minColumnPadding || value > m_maxColumnPadding)
                {
                    throw new ArgumentOutOfRangeException("ColumnPadding must between " + m_minColumnPadding.ToString() + " and " + m_maxColumnPadding.ToString());
                }
                m_columnPadding = value;
                this.RefreshIMultiColumntems();
            }
        }

        public new int MaxDropDownItems  // capture MaxDropDownItems changed event
        {
            get { return base.MaxDropDownItems; }
            set
            {
                base.MaxDropDownItems = value;
                this.SetDropDownHeight();
            }
        }

        [Category("Custom")]
        [Description("Get the displayed text separated by comma(,) when multi columns shown.")]
        public string TextDisplayed
        {
            get { return m_textDisplayed; }
        }

        [Category("Custom")]
        [Description("version.")]
        public string Version
        {
            get { return m_version; }
        }

        [Description("Disalble this property.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]  // hide in property editor window.
        public new DrawMode DrawMode
        {
            get { return base.DrawMode; }
        }

        public new ComboBoxStyle DropDownStyle
        {
            get { return base.DropDownStyle; }
            set
            {
                if (value == ComboBoxStyle.Simple)
                {
                    throw new NotSupportedException("ComboBoxStyle.Simple is not supported");
                }
                base.DropDownStyle = value;
            }
        }

        [Description("Disalble this property.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]  // hide in property editor window.
        public new bool IntegralHeight
        {
            get { return base.IntegralHeight; }
        }

        private int TotalDropDownWidth
        {
            get
            {
                if (this.Items.Count == 0)  // no datasource
                {
                    return m_maxItemWidth;
                }

                int totalWidth = m_columnPadding;
                for (int k = 0; k < m_columnNames.Count; k++)
                {
                    totalWidth += m_columnWidths[k];
                }

                if (this.Items.Count > base.MaxDropDownItems || base.RightToLeft == RightToLeft.Yes)
                {
                    totalWidth += SystemInformation.VerticalScrollBarWidth;
                }

                if (totalWidth < m_maxItemWidth)
                {
                    return m_maxItemWidth;
                }

                return totalWidth;
            }
        }

        #endregion

        #region override event methods

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);

            this.SetDisplayedColumns();
            this.SetDisplayMemberColumn();
            this.SetValueMemberColumn();

            this.RefreshIMultiColumntems();  // call OnMeasureItem()
            base.Invalidate();

            m_textDisplayed = string.Empty;
        }

        protected override void OnValueMemberChanged(EventArgs e)
        {
            base.OnValueMemberChanged(e);

            this.SetValueMemberColumn();
            base.Invalidate();
        }

        protected override void OnDisplayMemberChanged(EventArgs e)
        {
            base.OnDisplayMemberChanged(e);

            this.SetDisplayMemberColumn();
            base.Invalidate();
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            if (base.Focused == false)
            {
                base.Focus();
            }

            base.DropDownWidth = this.TotalDropDownWidth;  // +SystemInformation.VerticalScrollBarWidth;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);

            m_foreColorBrush.Color = base.ForeColor;
            this.Invalidate();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            m_backColorBrush.Color = base.BackColor;
            this.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            m_maxItemWidth = base.Width;
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);

            this.RefreshIMultiColumntems();
            this.Invalidate();
        }

        /// <summary>
        /// Compute item widths and item height.
        /// </summary>
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            if (m_columnNames.Count == 0)  // no bound datasource
            {
                string item = Convert.ToString(Items[e.Index]);
                int width = (int)(e.Graphics.MeasureString(item, base.Font).Width) + m_columnPadding;
                if (width > m_maxItemWidth)
                {
                    m_maxItemWidth = width;
                }
            }
            else
            {
                for (int k = 0; k < m_columnNames.Count; k++)
                {
                    string item = Convert.ToString(FilterItemOnProperty(Items[e.Index], m_columnNames[k]));
                    int width = (int)(e.Graphics.MeasureString(item, base.Font).Width);
                    width += m_leftOffset + (int)m_gridLinePen.Width + m_columnPadding;
                    m_columnWidths[k] = Math.Max(m_columnWidths[k], width);
                }
            }

            e.ItemWidth = this.TotalDropDownWidth;
            e.ItemHeight = m_itemDropDownHeight;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (DesignMode)
            {
                return;
            }

            if (this.Items.Count == 0 || e.Index == -1)
            {
                return;
            }

            e.DrawBackground();

            SolidBrush fontColorBrush;  // ForeColor change in BoxEdit;
            if ((e.State & DrawItemState.Selected) != 0)
            {
                fontColorBrush = m_backColorBrush;
            }
            else
            {
                fontColorBrush = m_foreColorBrush;
            }

            if (m_columnNames.Count == 0)  // no binding datasource.
            {
                m_textDisplayed = Convert.ToString(base.Items[e.Index]);
                e.Graphics.DrawString(m_textDisplayed, base.Font, fontColorBrush, e.Bounds, m_stringFormat);
                return;
            }

            Rectangle boundRect = e.Bounds;

            if ((m_displayMultiColumnsInBox == false) && (e.State & DrawItemState.ComboBoxEdit) != 0)  // box text.
            {
                m_textDisplayed = string.Empty;

                if (m_displayMemberColumnIndex != -1)
                {
                    m_textDisplayed = Convert.ToString(FilterItemOnProperty(base.Items[e.Index], m_columnNames[m_displayMemberColumnIndex]));
                }

                if (this.RightToLeft == RightToLeft.Yes)
                {
                    m_stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    boundRect.Width += m_leftOffset;
                }
                else
                {
                    m_stringFormat.FormatFlags = 0;
                    boundRect.X = m_leftOffset;
                }

                e.Graphics.DrawString(m_textDisplayed, base.Font, fontColorBrush, boundRect, m_stringFormat);
                return;
            }

            m_textDisplayed = string.Empty;
            boundRect.X = 0;

            if (this.RightToLeft == RightToLeft.Yes)
            {
                m_stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

                if ((e.State & DrawItemState.ComboBoxEdit) != 0) // draw box text.
                {
                    boundRect.X = SystemInformation.VerticalScrollBarWidth + m_leftOffset;
                }
                else
                {
                    if (base.Items.Count < base.MaxDropDownItems)
                    {
                        boundRect.X = SystemInformation.VerticalScrollBarWidth;
                    }
                    else
                    {
                        boundRect.X = 0;
                    }
                }

                for (int k = m_columnNames.Count - 1; k >= 0; k--)
                {
                    string item = Convert.ToString(FilterItemOnProperty(base.Items[e.Index], m_columnNames[k]));
                    boundRect.Width = m_columnWidths[k];
                    e.Graphics.DrawString(item, base.Font, fontColorBrush, boundRect, m_stringFormat);

                    if (m_displayVerticalLine == true && (e.State & DrawItemState.ComboBoxEdit) == 0 && k > 0)  // draw vertical separator line.
                    {
                        e.Graphics.DrawLine(m_gridLinePen, boundRect.Right, boundRect.Top, boundRect.Right, boundRect.Bottom);
                    }

                    m_textDisplayed += item + ((k > 0) ? "," : "");
                    boundRect.X = boundRect.Right;
                }
                return;
            }
            else
            {
                if ((e.State & DrawItemState.ComboBoxEdit) != 0) // draw box text.
                {
                    boundRect.X = m_leftOffset;
                }

                m_stringFormat.FormatFlags = 0;
                int lastRight = 0;
                for (int k = 0; k < m_columnNames.Count; k++)
                {
                    if (lastRight > 0)
                    {
                        if (m_displayVerticalLine == true && (e.State & DrawItemState.ComboBoxEdit) == 0)  // draw vertical separator line.
                        {
                            e.Graphics.DrawLine(m_gridLinePen, boundRect.Right, boundRect.Top, boundRect.Right, boundRect.Bottom);
                        }

                        boundRect.X = lastRight;
                        m_textDisplayed += ",";
                    }

                    string item = Convert.ToString(FilterItemOnProperty(base.Items[e.Index], m_columnNames[k]));
                    boundRect.Width = m_columnWidths[k];

                    e.Graphics.DrawString(item, base.Font, fontColorBrush, boundRect, m_stringFormat);

                    m_textDisplayed += item;
                    lastRight = boundRect.Right + m_leftOffset;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONDOUBLECLICK)
            {
                if (this.Items.Count == 0)  // Items is empty.
                {
                    if (base.DrawMode != DrawMode.OwnerDrawFixed)
                    {
                        base.DrawMode = DrawMode.OwnerDrawFixed;
                        base.DropDownHeight = m_minDropDownHeight;
                        base.IntegralHeight = true;
                        base.DroppedDown = true;  // drop down once more.
                    }
                    else if (base.DropDownHeight != m_minDropDownHeight)
                    {
                        base.DropDownHeight = m_minDropDownHeight;
                        base.IntegralHeight = true;
                        base.DroppedDown = true;
                    }
                    else if (base.IntegralHeight == false)
                    {
                        base.IntegralHeight = true;
                        base.DroppedDown = true;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                }
                else  // Items is not empty.
                {
                    if (base.DrawMode != DrawMode.OwnerDrawVariable)
                    {
                        base.DrawMode = DrawMode.OwnerDrawVariable;
                        base.IntegralHeight = false;
                        base.DroppedDown = true;
                    }
                    else if (base.DropDownHeight != m_maxDropDownHeight)
                    {
                        base.DropDownHeight = m_maxDropDownHeight;
                        base.IntegralHeight = false;
                        base.DroppedDown = true;
                    }
                    else if (base.IntegralHeight == true)
                    {
                        base.IntegralHeight = false;
                        base.DroppedDown = true;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                }
            }
            else if (m.Msg == WM_ADDITEM && base.Items.Count == 1)  // add one item already.
            {
                if (base.DropDownHeight != m_maxDropDownHeight)
                {
                    base.DropDownHeight = m_maxDropDownHeight;
                }
                base.WndProc(ref m);
            }
            else if (m.Msg == WM_DELETEITEM && base.Items.Count == 1)  // will delete the last item.
            {
                if (base.DropDownHeight == m_maxDropDownHeight)
                {
                    base.DropDownHeight = m_itemDropDownHeight;
                }
                base.WndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Get index by searching assigned column instead of Items.IndexOf.
        /// </summary>
        public int ItemIndexOf(string itemValue, bool ignoreCase, string columnName)
        {
            if (this.DataSource == null || m_columnNames.Count == 0)
            {
                return this.ItemIndexOf(itemValue, ignoreCase);
            }
            else
            {
                for (int k = 0; k < base.Items.Count; k++)
                {
                    string item = Convert.ToString(FilterItemOnProperty(base.Items[k], columnName));
                    if (string.IsNullOrEmpty(item) == false)
                    {
                        if (string.Compare(item, itemValue, ignoreCase, CultureInfo.CurrentUICulture) == 0)
                        {
                            return k;
                        }
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// Get index by searching assigned column value(ignoreCase = true) instead of Items.IndexOf. 
        /// </summary>
        public int ItemIndexOf(string itemValue, string columnName)
        {
            return this.ItemIndexOf(itemValue, true, columnName);
        }

        /// <summary>
        /// Get index by searching DisplayMember value instead of Items.IndexOf.
        /// </summary>
        public int ItemIndexOf(string itemValue, bool ignoreCase)
        {
            if (this.DataSource == null || m_columnNames.Count == 0)
            {
                for (int k = 0; k < this.Items.Count; k++)
                {
                    string item = Convert.ToString(FilterItemOnProperty(base.Items[k]));
                    if (string.Compare(item, itemValue, ignoreCase, CultureInfo.CurrentUICulture) == 0)
                    {
                        return k;
                    }
                }
                return -1;
            }

            if (m_displayMemberColumnIndex == -1)
            {
                return -1;
            }

            for (int k = 0; k < base.Items.Count; k++)
            {
                string item = Convert.ToString(FilterItemOnProperty(base.Items[k], m_columnNames[m_displayMemberColumnIndex]));
                if (string.Compare(item, itemValue, ignoreCase, CultureInfo.CurrentUICulture) == 0)
                {
                    return k;
                }
            }

            return -1;
        }

        /// <summary>
        /// Get index by searching DisplayMember value(ignoreCase = true) instead of Items.IndexOf. 
        /// </summary>
        public int ItemIndexOf(string itemValue)
        {
            return this.ItemIndexOf(itemValue, true);
        }

        #endregion

        #region private methods

        private void SetDisplayedColumns()
        {
            m_columnNames.Clear();

            if (this.DataSource == null || this.DesignMode)
            {
                return;
            }

            PropertyDescriptorCollection propertyDescriptorCollection = this.DataManager.GetItemProperties();
            if (string.IsNullOrEmpty(m_displayColumnNames) == true)  // default, show all columns.
            {
                for (int k = 0; k < propertyDescriptorCollection.Count; k++)
                {
                    m_columnNames.Add(propertyDescriptorCollection[k].Name);
                }
            }
            else
            {
                char[] delimiterChars = { '|', ',' };
                string[] displayedColumns = m_displayColumnNames.Split(delimiterChars);

                for (int n = 0; n < displayedColumns.Length; n++)
                {
                    for (int k = 0; k < propertyDescriptorCollection.Count; k++)
                    {
                        string name = propertyDescriptorCollection[k].Name;
                        if (string.Compare(displayedColumns[n].Trim(), name, true, CultureInfo.CurrentUICulture) == 0)
                        {
                            if (m_columnNames.Contains(name) == false)
                            {
                                m_columnNames.Add(name);
                            }
                            break;
                        }
                    }
                }
            }
            m_columnWidths = new int[m_columnNames.Count];
        }

        private void SetValueMemberColumn()
        {
            if (this.DesignMode)
            {
                return;
            }

            m_valueMemberColumnIndex = -1;
            for (int k = 0; k < m_columnNames.Count; k++)
            {
                if (string.Compare(m_columnNames[k], base.ValueMember, true, CultureInfo.CurrentUICulture) == 0)
                {
                    m_valueMemberColumnIndex = k;
                    break;
                }
            }
        }

        private void SetDisplayMemberColumn()
        {
            if (this.DesignMode)
            {
                return;
            }

            m_displayMemberColumnIndex = -1;
            for (int k = 0; k < m_columnNames.Count; k++)
            {
                if (string.Compare(m_columnNames[k], base.DisplayMember, true, CultureInfo.CurrentUICulture) == 0)
                {
                    m_displayMemberColumnIndex = k;
                    break;
                }
            }
        }

        private void RefreshIMultiColumntems()
        {
            if (m_columnNames.Count == 0)
            {
                return;
            }

            m_columnWidths = new int[m_columnWidths.Length];

            if (base.DrawMode != DrawMode.OwnerDrawVariable)
            {
                base.DrawMode = DrawMode.OwnerDrawVariable;  // OnMeasureItem can be called in this mode.
                base.RefreshItems();  // will call OnMeasureItem.
                base.DrawMode = DrawMode.OwnerDrawFixed;
            }
            else
            {
                base.RefreshItems();
            }
        }

        private void SetDropDownHeight()
        {
            m_maxDropDownHeight = base.MaxDropDownItems * m_itemDropDownHeight + m_bottomPadding;
            m_minDropDownHeight = m_itemDropDownHeight + m_bottomPadding;

            base.DropDownHeight = m_maxDropDownHeight;
        }
        #endregion
    }
}