namespace NoteBase.Models
{
    public class Shares
    {
        public int Note_Id { get; set; }
        public int Owner_Id { get; set; }
		public int UserId { get; set; }

		public Users User { get; set; }
        public Note Note { get; set; }
    }

}
