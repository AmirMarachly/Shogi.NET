using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Shogi.ViewModel
{
    public class ShogiViewModel : INotifyPropertyChanged
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

        private ObservableCollection<Cell> board;

        public ObservableCollection<Cell> Board
        {
            get
            {
                return board;
            }

            set
            {
                board = value;
                OnPropertyChanged("Board");
            }
        }

        private ObservableCollection<Cell> senteHand;

        public ObservableCollection<Cell> SenteHand
        {
            get
            {
                return senteHand;
            }

            set
            {
                senteHand = value;
                OnPropertyChanged("SenteHand");
            }
        }

        private ObservableCollection<Cell> goteHand;

        public ObservableCollection<Cell> GoteHand
        {
            get
            {
                return goteHand;
            }

            set
            {
                goteHand = value;
                OnPropertyChanged("GoteHand");
            }
        }

        public ICommand OnGridClicked
        {
            get 
            {
                return new RelayCommand(GridClicked);
            }
        }

        public ShogiViewModel()
        {
            ObservableCollection<Cell> newGrid = new ObservableCollection<Cell>();

            Array pieces = Enum.GetValues(typeof(Piece));
            Random random = new Random();

            for (int i = 0; i < 81; ++i)
            {
                Piece newPiece = (Piece)pieces.GetValue(random.Next(pieces.Length));
                newGrid.Add(new Cell()
                {
                    Piece = newPiece,
                    IsSente = random.Next(2) == 0,
                    Highlighted = false,
                    Index = i
                });
            }

            Board = newGrid;

            ObservableCollection<Cell> newHand = new ObservableCollection<Cell>();

            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    Piece newPiece = (Piece)pieces.GetValue(random.Next(pieces.Length));
                    newHand.Add(new Cell() { Piece = newPiece, IsSente = random.Next(2) == 0});
                }
            }

            SenteHand = newHand;
            GoteHand = newHand;
        }

        private void RefreshBoard()
        {
            CollectionViewSource.GetDefaultView(Board).Refresh();
        }

        private void GridClicked(object sender)
        {
            if (sender is not Cell)
            {
                return;
            }

            Cell cell = (Cell)sender;
            cell.Highlighted = true;

            RefreshBoard();
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
