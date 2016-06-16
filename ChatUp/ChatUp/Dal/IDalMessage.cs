using ChatUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUp.Dal
{
    public interface IDalMessage: IDalContenu
    {
        MessageModel CreerMessage(string message);
    }
}
