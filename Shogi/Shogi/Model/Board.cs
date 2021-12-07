using Shogi.Model.pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model
{
    class Board
    {
        private int size;
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        private Piece?[,] board;
        public Piece? this[int i, int j]
        {
            get { return board[i, j]; }
            set { board[i, j] = value; }
        }

        public Board(int _size)
        {
            size = _size;
            board = new Piece[size, size];
        }

        public void Init()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = new Piece(PiecesType.Bishop);
                }
            }
        }
    }
}
