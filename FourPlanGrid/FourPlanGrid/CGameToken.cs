using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace FourPlanGrid
{
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
    public class ColorToStringConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                return string.Format(culture, format, value);
            }
            else
            {
                return ColorConverter.ConvertFromString(value.ToString());
            }
        }
    }

    public class CGameToken : INotifyPropertyChanged
    {
        private Color mColor = Color.FromArgb(255,255,255,0);
        public Color Color
        {
            get { return this.mColor; }
            set
            {
                if (value != this.mColor)
                {
                    this.mColor = value;
                    NotifyPropertyChanged("Color");
                }
            }
        }
        public Tuple<int, int> Position { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        

        

    }
}
