using ChatUp.Models;
using System.Collections.Generic;

namespace ChatUp.ViewModels
{
    public class DiscussionViewModel
    {
        public GroupeModel Groupe { get; set; }
        public MessageModel Message { get; set; }
        public UtilisateurModel Utilisateur { get; set; }
    }
}