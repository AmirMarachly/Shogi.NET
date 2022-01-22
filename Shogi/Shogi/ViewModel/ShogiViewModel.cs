using Shogi.Model;
using Shogi.Model.pieces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Shogi.ViewModel
{
    /// <summary>
    /// The viewmodel for the shogi
    /// </summary>
    public class ShogiViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Simple class representing a cell of the grid
        /// </summary>
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

        private bool isOnMenu = true;

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
                OnPropertyChanged("RandomPiece");
            }
        }

        public PiecesType RandomPiece
        {
            get
            {
                Array pieces = Enum.GetValues(typeof(PiecesType));
                return (PiecesType)pieces.GetValue(new Random().Next(pieces.Length));
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

        private bool canPromote;

        public bool CanPromote
        {
            get
            {
                return canPromote;
            }

            set
            {
                canPromote = value;
                OnPropertyChanged("CanPromote");
            }
        }

        public string Winner
        {
            get
            {
                if (IsOnMenu)
                {
                    return "";
                }

                if (sente.HasLost())
                {
                    return gote.Name;
                }

                if (gote.HasLost())
                {
                    return sente.Name;
                }

                return "";
            }
        }

        public ICommand OnPlayClicked
        {
            get
            {
                return new RelayCommand(Init);
            }
        }

        public ICommand OnQuitClicked
        {
            get
            {
                return new RelayCommand(o => App.Current.Shutdown());
            }
        }

        public ICommand OnMenuClicked
        {
            get
            {
                return new RelayCommand(o => IsOnMenu = true);
            }
        }

        public ICommand OnRulesClicked
        {
            get
            {
                return new RelayCommand(o => Process.Start(new ProcessStartInfo("https://en.wikipedia.org/wiki/Shogi#Rules") { UseShellExecute = true }));
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

        /// <summary>
        /// (Re)initilaize the boards and the players
        /// </summary>
        /// <param name="parameter">The paramater of the call</param>
        public void Init(object parameter)
        {
            sente = new Player("sente", true, true);
            gote = new Player("gote", false, false);

            board = new Board(sente, gote);
            InitBoard();

            IsOnMenu = false;
            CanPromote = false;
            OnPropertyChanged("Winner");
        }

        /// <summary>
        /// Refresh the board and the hands
        /// </summary>
        private void RefreshBoard()
        {
            CollectionViewSource.GetDefaultView(ObservableBoard).Refresh();
            CollectionViewSource.GetDefaultView(SenteHand).Refresh();
            CollectionViewSource.GetDefaultView(GoteHand).Refresh();
        }

        /// <summary>
        /// Init the board and the hands
        /// </summary>
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

        /// <summary>
        /// Update the board and the hands according to the model
        /// </summary>
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
            GoteHand = new ObservableCollection<Cell>(gote.PiecesInHand.ConvertAll(p => (Cell) p));

            RefreshBoard();
        }

        /// <summary>
        /// Reset all the highlights of the cells
        /// </summary>
        private void ResetHighlight()
        {
            foreach (Cell cell in ObservableBoard)
            {
                cell.IsSelected = false;
                cell.IsAvaibleMove = false;
                cell.IsAttackMove = false;
            }

            foreach (Cell cell in SenteHand)
            {
                cell.IsSelected = false;
                cell.IsAvaibleMove = false;
                cell.IsAttackMove = false;
            }

            foreach (Cell cell in GoteHand)
            {
                cell.IsSelected = false;
                cell.IsAvaibleMove = false;
                cell.IsAttackMove = false;
            }
        }

        /// <summary>
        /// Function called when the board is clicked
        /// </summary>
        /// <param name="parameter">The parameter of the call</param>
        private void BoardClicked(object parameter)
        {
            if (sente.HasLost() || gote.HasLost())
            {
                return;
            }

            Cell cell = parameter as Cell;

            if (cell == null)
            {
                return;
            }

            if (cell.IsAvaibleMove || cell.IsAttackMove)
            {
                (int, int) cellPos = (cell.Index / 9, cell.Index % 9);

                if (selectedFromHand)
                {
                    board.ParachuteAPiece(selectedPiece, cellPos);
                }
                else
                {
                    CanPromote = selectedPiece.CanPromote(cellPos);
                    board.MoveAPiece(selectedPiece, cellPos);
                }

                sente.HasPlayed();
                gote.HasPlayed();

                UpdateBoard();
                ResetHighlight();

                OnPropertyChanged("Winner");

                if (sente.HasLost() || gote.HasLost())
                {
                    CanPromote = false;
                }
                else
                {
                    cell.IsSelected = true;
                }

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
            CanPromote = false;

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

        /// <summary>
        /// Function called when the hand is clicked
        /// </summary>
        /// <param name="parameter">The parameter of the call</param>
        private void HandClicked(object parameter)
        {
            if (sente.HasLost() || gote.HasLost())
            {
                return;
            }

            Cell cell = parameter as Cell;

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
            CanPromote = false;

            List<(int, int)> moves = cell.Piece.GetPossibleParachute(board,
                    cell.Piece.Owner == sente ? gote : sente);

            foreach ((int, int) move in moves)
            {
                ObservableBoard[move.Item1 * 9 + move.Item2].IsAvaibleMove = true;
            }

            RefreshBoard();
        }

        /// <summary>
        /// Function called when the promote button is clicked
        /// </summary>
        /// <param name="parameter">The parameter of the call</param>
        private void PromoteClicked(object parameter)
        {
            selectedPiece.Promote();

            CanPromote = false;
            UpdateBoard();
        }

        /// <summary>
        /// The event raised when a property change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the event associated to the name of the property
        /// </summary>
        /// <param name="name">The name of the property</param>
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
