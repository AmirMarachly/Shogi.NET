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
            Tokin,
            Empty
        }

        public struct Cell
        {
            public Piece piece;
            public bool isSente;
        }

        private ObservableCollection<Cell> grid;

        public ObservableCollection<Cell> Grid
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
            ObservableCollection<Cell> newGrid = new ObservableCollection<Cell>();

            Array pieces = Enum.GetValues(typeof(Piece));
            Random random = new Random();

            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; ++j)
                {
                    Piece newPiece = (Piece) pieces.GetValue(random.Next(pieces.Length));
                    newGrid.Add(new Cell() {piece = newPiece, isSente = random.Next(2) == 0});
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
