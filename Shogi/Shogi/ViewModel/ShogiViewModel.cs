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

            public static explicit operator Cell(Piece p)
            {
                return new Cell { Piece = p, Index = p.Pos.Item1 * 9 + p.Pos.Item2 };
            }

            public int Index
            {
                get;
                set;
            }

            public bool IsAvaibleMove
            {
                get;
                set;
            }

            public bool IsAttackMove
            {
                get;
                set;
            }

            public bool IsSelected
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

        public ICommand OnBoardClicked
        {
            get 
            {
                return new RelayCommand(BoardClicked);
            }
        }

        private Player sente;
        private Player gote;
        private Board board;

        private Cell selectedCell;

        public ShogiViewModel()
        {
            sente = new Player("Amir", true, true);
            gote = new Player("Ulys", false, false);

            board = new Board(sente, gote);
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
                    ObservableBoard.Add(new Cell() { Piece = board[i, j], Index = i * 9 + j});
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
                    ObservableBoard[i * 9 + j].Piece = board[i, j];
                }
            }

            List<Cell> uniqueSenteHand = sente.PiecesInHand
                .GroupBy(p => p.PieceType)
                .Select(g => g.FirstOrDefault())
                .Select(p => (Cell)p)
                .ToList();

            SenteHand = new ObservableCollection<Cell>(uniqueSenteHand);

            List<Cell> uniqueGoteHand = gote.PiecesInHand
                .GroupBy(p => p.PieceType)
                .Select(g => g.FirstOrDefault())
                .Select(p => (Cell)p)
                .ToList();

            GoteHand = new ObservableCollection<Cell>(uniqueGoteHand);

            RefreshBoard();
        }

        private void ResetHighlight()
        {
            foreach (Cell cell in ObservableBoard)
            {
                cell.IsSelected = false;
                cell.IsAvaibleMove = false;
                cell.IsAttackMove = false;
            }
        }

        private void BoardClicked(object sender)
        {
            Cell cell = sender as Cell;

            if (cell == null)
            {
                return;
            }

            if (cell.IsAvaibleMove || cell.IsAttackMove)
            {
                board.MoveAPiece(selectedCell.Piece, (cell.Index / 9, cell.Index % 9));

                UpdateBoard();
                ResetHighlight();

                return;
            }

            ResetHighlight();

            if (cell.Piece == null)
            {
                return;
            }

            cell.IsSelected = true;
            selectedCell = cell;

            Dictionary<string, List<(int, int)>> moves = cell.Piece.GetPossibleMove(board);

            foreach ((int, int) move in moves["avaibleMove"])
            {
                ObservableBoard[move.Item1 * 9 + move.Item2].IsAvaibleMove = true;
            }

            foreach ((int, int) move in moves["attackMove"])
            {
                ObservableBoard[move.Item1 * 9 + move.Item2].IsAttackMove = true;
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
