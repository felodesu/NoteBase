using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteBase.Models
{
    public class SharedNotes 
    {
        public int Note_Id { get; set; }
        public string Note_Header { get; set; }
        public string Note_Content { get; set; }
        public long Note_Timestamp { get; set; }

        public int Owner_Id { get; set; }
        public int Shared_To { get; set; }

        public List<int> Shared_To_UserId { get; set; } = new List<int>();

		public List<string> Shared_To_Usernames { get; set; }

        public static void Sort(List<SharedNotes> sharedNotes) => sharedNotes.Sort(new NotesComparer());

        public static List<SharedNotes> Distinct (List<SharedNotes> sharedNotes)
        {
            if (sharedNotes.Count == 0)
            {
                return sharedNotes;
            }

			List<SharedNotes> newList = new List<SharedNotes>
															{
																sharedNotes.First()
															};
			SharedNotes previousNote = newList.First();
            IComparer<SharedNotes> comparer = new NotesComparer();

            foreach (var note in sharedNotes)
            {
                if (comparer.Compare(note, previousNote) == 0)
                {
                    previousNote.Shared_To_UserId.Add(note.Shared_To);
                }
                else
                {
                    note.Shared_To_UserId.Add(note.Shared_To);
                    newList.Add(note);
                    previousNote = newList.Last();
                }
                
            }

            return  newList;
        }

		public async Task<List<SharedNotes>> AppendUsernameString (List<SharedNotes> sharedNotes, DbConnection conn)
		{
			if (sharedNotes.Count == 0) return sharedNotes;

			foreach (var note in sharedNotes)
			{
				note.Shared_To_Usernames = await conn.GetUsersById(note.Shared_To_UserId);
			}

			return sharedNotes;
		}
    }
}
