using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shogi.Model.pieces.move
{
    class KakugyoMove : IMove
    {
        public List<(int, int)> EvolvedMove((int, int) startPos, bool isSente)
        {
            List<(int, int)> result = Move(startPos, isSente);

            result.Add((startPos.Item1, startPos.Item2 + 1));
            result.Add((startPos.Item1, startPos.Item2 - 1));
            result.Add((startPos.Item1 + 1, startPos.Item2));
            result.Add((startPos.Item1 - 1, startPos.Item2));

            return result;
        }

        public List<(int, int)> Move((int, int) startPos, bool isSente)
        {
            List<(int, int)> result = new List<(int, int)>();

            (int, int)[] directions = new (int, int)[4];

            directions[0] = (-1, 1);
            directions[1] = (-1, -1);
            directions[2] = (1, 1);
            directions[3] = (1, -1);

            (int, int) tmpPos = startPos;

            for (int i = 0; i < directions.Length; i++)
            {
                while(tmpPos.Item1 < 9 && tmpPos.Item1 >= 0 && tmpPos.Item2 < 9 && tmpPos.Item2 >= 0)
                {
                    tmpPos = (tmpPos.Item1 + directions[i].Item1, tmpPos.Item2 + directions[i].Item2);
                    result.Add(tmpPos);
                }

                tmpPos = startPos;
            }

            return result;
        }
    }
}
