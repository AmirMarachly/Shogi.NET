using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces
{
    public interface IMove
    {
        List<(int,int)> Move((int,int) startPos, Board board,bool isSente);
        List<(int, int)> EvolvedMove((int, int) startPos, Board board, bool isSente);
    }
}
