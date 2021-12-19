using Shogi.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Shogi.View
{
    class PieceToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ShogiViewModel.Cell)
            {
                ShogiViewModel.Cell cell = (ShogiViewModel.Cell) value;
                return $"/Shogi;component/Resources/{cell.piece}{(cell.isSente ? "Sente" : "Gote")}.png";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
