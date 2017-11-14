using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Pool1984
{
    [DefaultEvent("ValueChanged")]
    public partial class ValueSetter : UserControl
    {
        private double value = 0.0;
        private double min = 0.0;
        private double max = 360.0;
        private bool suppressEvent = false;

        public event EventHandler ValueChanged;

        public double Value
        {
            get { return value; }
            set
            {
                this.value = value;
                SetScrollBar(this.value);
            }
        }

        public double Min
        {
            get { return min; }
            set
            {
                this.min = value;
                SetScrollBar(this.value);
            }
        }

        public double Max
        {
            get { return max; }
            set
            {
                this.max = value;
                SetScrollBar(this.value);
            }
        }

        public string Label
        {
            get { return SettingLabel.Text; }
            set { SettingLabel.Text = value; }
        }

        public string Dimension
        {
            get { return SettingDimension.Text; }
            set { SettingDimension.Text = value; }
        }

        private void SetScrollBar(double value)
        {
            suppressEvent = true;
            int scrollValue = (int)((value - min) * SettingScrollbar.Maximum / (max - min));
            SettingScrollbar.Value = Math.Min(Math.Max(scrollValue, SettingScrollbar.Minimum), SettingScrollbar.Maximum);
            suppressEvent = false;
        }

        public ValueSetter()
        {
            InitializeComponent();
            SetScrollBar(this.value);
        }

        private void SettingText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                double value;
                if (!Double.TryParse(SettingText.Text, out value))
                {
                    e.Handled = false;
                }
                else
                {
                    this.value = value;
                    SetScrollBar(this.value);
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }

        }

        private void SettingScrollbar_ValueChanged(object sender, EventArgs e)
        {
            this.value = (SettingScrollbar.Value * (max - min) / SettingScrollbar.Maximum + min);
            SettingText.Text = value.ToString("0.0");
            if (!suppressEvent) ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
