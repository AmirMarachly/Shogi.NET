using Shogi.Model.pieces;
using Shogi.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using static Shogi.ViewModel.ShogiViewModel;

namespace Shogi.View
{
    /// <summary>
    /// Converter to display the winner
    /// </summary>
    class WinnerTextConverter : IValueConverter
    {
        /// <summary>
        /// Convert the winner name to the display string
        /// </summary>
        /// <param name="value">The winner name</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string winner = value as string;

            if (winner != null)
            {
                return $"{winner} win";
            }

            return "";
        }

        /// <summary>
        /// Convert back the display string to the name. NOT IMPLEMENTED!
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
