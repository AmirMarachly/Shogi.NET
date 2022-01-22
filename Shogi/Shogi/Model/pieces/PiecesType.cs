using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces
{
    /// <summary>
    /// This enum define all piece name it's use to know what <see cref="IMove"/> child class will be use to define the possible move of a piece
    /// </summary>
    public enum PiecesType
    {
        Osho,
        Gyokusho,
        Hisha,
        Kakugyo,
        Kinsho,
        Ginsho,
        Keima,
        Kyosha,
        Fuhyo,
        Ryuo = 10,
        Ryuma,
        Narigin = 13,
        Narikei,
        Narikyo,
        Tokin
    }
}
