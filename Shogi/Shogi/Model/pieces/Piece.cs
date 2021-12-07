using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces
{
    class Piece
    {
        private PiecesType type;
        public PiecesType Type
        {
            get { return type; }
        }

        private bool isEvolved;
        public bool IsEvolved
        {
            get { return isEvolved; }
            set { isEvolved = value; }
        }

        public Piece(PiecesType _type, bool _isEvolved = false)
        {
            type = _type;
            isEvolved = _isEvolved;
        }
    }
}
