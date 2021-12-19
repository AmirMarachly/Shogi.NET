using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.ViewModel
{
    class ShogiViewModel : INotifyPropertyChanged
    {
        public enum Piece
        {
            Osho,
            Gyokusho,
            Hisha,
            Ryuo,
            Kakugyo,
            Ryuma,
            Kinsho,
            Ginsho,
            Narigin,
            Keima,
            Narikei,
            Kyosha,
            Narikyo,
            Fuhyo,
            Tokin
        }

        private ObservableCollection<Tuple<Piece, int, int>> grid;

        public ObservableCollection<Tuple<Piece, int, int>> Grid
        {
            get
            {
                return grid;
            }

            set
            {
                grid = value;
                OnPropertyChanged("Grid");
            }
        }

        public ShogiViewModel()
        {
            ObservableCollection<Tuple<Piece, int, int>> newGrid 
                = new ObservableCollection<Tuple<Piece, int, int>>();

            Array pieces = Enum.GetValues(typeof(Piece));
            Random random = new Random();

            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    Piece newPiece = (Piece) pieces.GetValue(random.Next(pieces.Length));
                    newGrid.Add(Tuple.Create(newPiece, i, j));
                }
            }

            Grid = newGrid;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
