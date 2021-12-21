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
    class PieceToImageConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
