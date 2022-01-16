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
    /// <summary>
    /// Converter to count the number of pieces in hand
    /// </summary>
    class HandToRowsConverter : IValueConverter
    {
        /// <summary>
        /// Count the number of pieces in hand
        /// </summary>
        /// <param name="value">The hand</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used</param>
        /// <returns>The number of pieces</returns>
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

        /// <summary>
        /// Convert back the number of pieces. NOT IMPLEMENTED!
        /// </summary>
        /// <param name="value">Not used</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
