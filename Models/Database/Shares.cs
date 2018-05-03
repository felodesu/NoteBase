using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteBase.Models
{
    public class Shares
    {
        public int User_Id { get; set; }
        public int Note_Id { get; set; }
        public int Owner_Id { get; set; }

        public Users User { get; set; }
        public Note Note { get; set; }
    }

}
