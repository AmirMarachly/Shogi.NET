using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shogi.ViewModel.ShogiViewModel;

namespace Shogi.ViewModel
{
    public class Cell
    {
        public Piece Piece
        {
            get;
            set;
        }

        public bool IsSente
        {
            get;
            set;
        }

        public bool Highlighted
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }
    }
}
