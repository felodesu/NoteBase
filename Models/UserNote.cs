using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NoteBase.Models
{
    public abstract class UserNote
    {
        public int Note_Id { get; set; }

        [Required(ErrorMessage = "Add header")]
        [MaxLength(30, ErrorMessage = "Maximum number of characters is 30")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Add content")]
        [DataType(DataType.MultilineText)]
        [MaxLength(140, ErrorMessage = "Maximum number of characters is 140")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Select date and time")]
        [Remote(action: "CheckDateTimeValid", controller: "NoteLibrary", ErrorMessage = "Select date and time in the future")]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }
    }
}
