using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatUp.Models;

namespace ChatUp.Dal
{
    public class DalContenu : IDalContenu
    {
        /// <summary>
        /// Une instance de notre base de donnée
        /// </summary>
        private BddContext bdd;

        public ContenuModel CreerContenu()
        {
            throw new NotImplementedException();
        }
    }
}