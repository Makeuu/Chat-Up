using ChatUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUp.Dal
{
    public interface IDalPieceJointe : IDalContenu
    {
        PieceJointeModel CreerPieceJointe(string nom);
    }
}
