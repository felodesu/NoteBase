using System.Collections.Generic;
using System;

namespace NoteBase.Models
{
    public class SharedNotes : UserNote
    {
        public int Owner_Id { get; set; }
        public string Owner { get; set; }

		public Dictionary<int,string> Shared_To_Usernames { get; set; }

    }
}
