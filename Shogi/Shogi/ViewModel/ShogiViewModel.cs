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

        private bool isOnMenu;

        public bool IsOnMenu
        {
            get
            {
                return isOnMenu;
            }

            set
            {
                isOnMenu = value;
                OnPropertyChanged("IsOnMenu");
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

        private bool canUpgrade;

        public bool CanUpgrade
        {
            get
            {
                return canUpgrade;
            }

            set
            {
                canUpgrade = value;
                OnPropertyChanged("CanUpgrade");
            }
        }

        public string CurrentPlayer
        {
            get
            {
                return sente.IsPlaying ? "Sente" : "Gote";
            }
        }

        public ICommand OnPlayClicked
        {
            get
            {
                return new RelayCommand(o => IsOnMenu = false);
            }
        }

        public ICommand OnBoardClicked
        {
            get 
            {
                return new RelayCommand(BoardClicked);
            }
        }

        public ICommand OnHandClicked
        {
            get
            {
                return new RelayCommand(HandClicked);
            }
        }

        public ICommand OnPromoteClicked
        {
            get
            {
                return new RelayCommand(PromoteClicked);
            }
        }

        private Player sente;
        private Player gote;
        private Board board;

        private Piece selectedPiece;
        private bool selectedFromHand;

        public ShogiViewModel()
        {
            isOnMenu = true;

            sente = new Player("sente", true, true);
            gote = new Player("gote", false, false);

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

            SenteHand = new ObservableCollection<Cell>(sente.PiecesInHand.ConvertAll(p => (Cell) p));
            GoteHand = new ObservableCollection<Cell>(gote.PiecesInHand.ConvertAll(p => (Cell)p));

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
                if (selectedFromHand)
                {
                    board.ParachuteAPiece(selectedPiece, (cell.Index / 9, cell.Index % 9));
                }
                else
                {
                    board.MoveAPiece(selectedPiece, (cell.Index / 9, cell.Index % 9));
                }

                sente.HasPlayed();
                gote.HasPlayed();
                OnPropertyChanged("CurrentPlayer");

                UpdateBoard();
                ResetHighlight();

                cell.IsSelected = true;

                return;
            }

            if (cell.Piece == null)
            {
                return;
            }

            if (!cell.Piece.Owner.IsPlaying)
            {
                return;
            }

            ResetHighlight();

            cell.IsSelected = true;
            selectedPiece = cell.Piece;
            selectedFromHand = false;

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

        private void HandClicked(object sender)
        {
            Cell cell = sender as Cell;

            if (cell == null)
            {
                return;
            }

            if (cell.Piece == null)
            {
                return;
            }

            if (!cell.Piece.Owner.IsPlaying)
            {
                return;
            }

            ResetHighlight();

            cell.IsSelected = true;
            selectedPiece = cell.Piece;
            selectedFromHand = true;

            List<(int, int)> moves = board.GetEmptyCell();

            foreach ((int, int) move in moves)
            {
                ObservableBoard[move.Item1 * 9 + move.Item2].IsAvaibleMove = true;
            }

            RefreshBoard();
        }

        private void PromoteClicked(object sender)
        {
            selectedPiece.Evolve();
            UpdateBoard();
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
