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
    /// Converter from a piece to the associated image
    /// </summary>
    class PieceToImageConverter : IValueConverter
    {
        /// <summary>
        /// Converte the piece to the image
        /// </summary>
        /// <param name="value">The piece</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used</param>
        /// <returns>The image</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PiecesType piece = (PiecesType) value;
            return $"/Shogi;component/Resources/{piece}Sente.png";
        }

        /// <summary>
        /// Convert back the image to the piece. NOT IMPLEMENTED!
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
