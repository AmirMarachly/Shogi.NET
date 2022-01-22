using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces
{
    /// <summary>
    /// Interface for the strategy pattern for all available movement for pieces
    /// </summary>
    interface IMove
    {
        /// <summary>
        /// Movmement for an unpromoted piece in an empty board
        /// </summary>
        /// <param name="startPos">the current position of the piece</param>
        /// <param name="board">the board of the current game</param>
        /// <param name="isSente">true if the piece's owner is the player that have start the game</param>
        /// <returns>All available cell for the piece to move </returns>
        List<(int,int)> Move((int,int) startPos, Board board,bool isSente);

        /// <summary>
        /// Movmement for an promoted piece in an empty board
        /// </summary>
        /// <param name="startPos">the current position of the piece</param>
        /// <param name="board">the board of the current game</param>
        /// <param name="isSente">true if the piece's owner is the player that have start the game</param>
        /// <returns>All available cell for the piece to move </returns>
        List<(int, int)> PromotedMove((int, int) startPos, Board board, bool isSente);
    }
}
