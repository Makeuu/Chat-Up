using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatUp.Models
{
    public class Message
    {
        public string Contenu { get; set; }

        public Message(string Contenu) { }
    }
}