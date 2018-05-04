using System.Collections.Generic;

namespace NoteBase.Models
{
    public class SharedNotes 
    {
        public int Note_Id { get; set; }
        public string Note_Header { get; set; }
        public string Note_Content { get; set; }
        public long Note_Timestamp { get; set; }

        public int Owner_Id { get; set; }
        public string Owner { get; set; }

		public Dictionary<int,string> Shared_To_Usernames { get; set; }

    }
}
