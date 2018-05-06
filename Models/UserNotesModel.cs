using System.Collections.Generic;

namespace NoteBase.Models
{
    public class UserNotesModel : UserNote
    {
        public List<int> User_Id { get; set; }
        public List<SharedNotes> SharedNotes { get; set; }
}
}
