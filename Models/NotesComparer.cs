using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBase.Models
{
    public class NotesComparer : IComparer<SharedNotes>
    {
        public int Compare(SharedNotes a, SharedNotes b)
        {
            int compareByUserId = a.Owner_Id.CompareTo(b.Owner_Id);
            if (compareByUserId == 0)
            {
                return a.Note_Id.CompareTo(b.Note_Id);
            }
            return compareByUserId;
        }
    }
}
