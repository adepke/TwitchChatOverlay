/*
	Copyright (c) 2018 Andrew Depke
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TwitchChatOverlay
{
    [ValueConversion(typeof(Color?), typeof(SolidColorBrush))]
    class NullableColorSolidBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color?)value ?? default(Color));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return null;
        }
    }
}
