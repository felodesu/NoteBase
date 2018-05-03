using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NoteBase.Models
{
    public class Users
    {
        [Key]
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public List<Note> Notes { get; set; }
        public List<Shares> Shares { get; set; }
    }

}
