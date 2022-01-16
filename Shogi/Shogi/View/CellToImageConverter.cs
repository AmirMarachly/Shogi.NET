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
    /// Converter from cell to image
    /// </summary>
    class CellToImageConverter : IValueConverter
    {
        /// <summary>
        /// Convert a cell to the corresponding image
        /// </summary>
        /// <param name="value">The cell</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not user</param>
        /// <param name="culture">Not used</param>
        /// <returns>A string with the image</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Cell cell = value as Cell;

            if (cell != null)
            {
                if (cell.Piece != null)
                {
                    return $"/Shogi;component/Resources/{cell.Piece.PieceType}{(cell.Piece.Owner.IsSente ? "Sente" : "Gote")}.png";
                }
            }

            return $"/Shogi;component/Resources/Empty.png";
        }

        /// <summary>
        /// Convert back the image string to the cell. NOT IMPLEMENTED!
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
