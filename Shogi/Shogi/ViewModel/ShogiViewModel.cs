using Shogi.Model;
using Shogi.Model.pieces;
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
        public class Cell
        {
            public Piece Piece
            {
                get;
                set;
            }

            public bool IsHighlighted
            {
                get;
                set;
            }
        }

        private ObservableCollection<Cell> observableBoard;

        public ObservableCollection<Cell> ObservableBoard
        {
            get
            {
                return observableBoard;
            }

            set
            {
                observableBoard = value;
                OnPropertyChanged("ObservableBoard");
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

        private Player player1;
        private Player player2;
        private Board board;

        public ShogiViewModel()
        {
            player1 = new Player("Amir", true, true);
            player2 = new Player("Ulys", false, false);

            board = new Board(player1, player2);
            InitBoard();
        }

        private void RefreshBoard()
        {
            CollectionViewSource.GetDefaultView(ObservableBoard).Refresh();
            CollectionViewSource.GetDefaultView(SenteHand).Refresh();
            CollectionViewSource.GetDefaultView(GoteHand).Refresh();
        }

        private void InitBoard()
        {
            ObservableBoard = new();
            SenteHand = new();
            GoteHand = new();

            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; j++)
                {
                    ObservableBoard.Add(new Cell() { Piece = board[i, j] });
                }
            }

            RefreshBoard();
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < 9; ++i)
            {
                for (int j = 0; j < 9; j++)
                {
                    ObservableBoard[i * 9 + j].Piece = board[j, i];
                }
            }

            RefreshBoard();
        }

        private void ResetHighlight()
        {
            foreach (Cell cell in ObservableBoard)
            {
                cell.IsHighlighted = false;
            }
        }

        private void GridClicked(object sender)
        {
            Cell cell = sender as Cell;

            if (cell == null)
            {
                return;
            }

            ResetHighlight();
            var moves = cell.Piece.GetPossibleMove(board);

            foreach ((int, int) move in moves["avaibleMove"])
            {
                ObservableBoard[move.Item1 * 9 + move.Item2].IsHighlighted = true;
            }

            foreach ((int, int) move in moves["attackMove"])
            {
                ObservableBoard[move.Item1 * 9 + move.Item2].IsHighlighted = true;
            }

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
