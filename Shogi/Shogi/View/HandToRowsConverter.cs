using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static Shogi.ViewModel.ShogiViewModel;

namespace Shogi.View
{
    class HandToRowsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<Cell> cells = value as ObservableCollection<Cell>;

            if (cells != null)
            {
                if (cells.Count > 1)
                {
                    return (cells.Count - 1) / 5 + 1;
                }
            }

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
