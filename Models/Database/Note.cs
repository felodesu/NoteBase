using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteBase.Models
{

        public class Note
        {
            [Key]
            public int Note_Id { get; set; }
            public long Timestamp { get; set; } 
            public string Header { get; set; }
            public string Content { get; set; }
            public int User_Id { get; set; }

            public Users User { get; set; }
        }

}
