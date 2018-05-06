using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace NoteBase.Models
{
    public class UserNotesModel : UserNote
    {
        public List<int> User_Id { get; set; }
        public List<SharedNotes> SharedNotes { get; set; }
}
}
