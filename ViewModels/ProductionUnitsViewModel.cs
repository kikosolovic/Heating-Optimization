using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Heating_Optimization.Models;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Avalonia.Data.Converters;

namespace Heating_Optimization
{
    public partial class ProductionUnitsViewModel : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isPointerOver = (bool)value;
            string target = parameter as string;

            if (target == "Background")
                return isPointerOver ? 0.15 : 1.0;
            else if (target == "Text")
                return isPointerOver ? 1.0 : 0.0;
            else
                return 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
