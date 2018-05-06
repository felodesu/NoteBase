using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;

namespace NoteBase.Models
{
    public class DbConnection : IDisposable
    {
        private DbModel dbModel;

        public DbConnection(DbModel dbModel)
        {
            this.dbModel = dbModel;
        }

        public void Dispose()
        {
            dbModel.Dispose();
        }


		////DbSetNotes actions

		public async Task CreateNote(Note note)
		{
			try
			{
				dbModel.DbSetNotes.Add(note);

				await dbModel.SaveChangesAsync();
			}
			catch (Exception e)
			{
				throw new DbUpdateException("Error occured while adding note to the database", e);
			}
		}

		public async Task<List<SharedNotes>> GetUserNotes(int user_id)
		{
			try
			{
				List<SharedNotes> userNotes = await (from n in dbModel.DbSetNotes
													 where (n.UserId == user_id)
													 orderby n.Timestamp
													 select new SharedNotes
													 {
														 Note_Id = n.Note_Id,
														 Header = n.Header,
														 Content = n.Content,
														 Timestamp = DateTime.FromBinary(n.Timestamp)
													 })
													   .ToListAsync();

				if (userNotes != null)
				{
					foreach (var note in userNotes)
					{
						note.Shared_To_Usernames = await (from s in dbModel.DbSetShares
														  where (s.Owner_Id == user_id && s.Note_Id == note.Note_Id)
														  select new
														  {
															  s.User.Id,
															  s.User.UserName
														  })
														  .ToDictionaryAsync(user => user.Id, user => user.UserName);
					}
				}

				return userNotes;
			}
			catch (Exception e)
			{
				throw new Exception("Error occured on database query", e);
			}
		}

		//DbSetShares actions

		public async Task ShareNote(int note_id, List<int> user_id, int owner_id)
		{
			try
			{
				var shares = await (from s in dbModel.DbSetShares
									where (s.Owner_Id == owner_id && s.Note_Id == note_id)
									select s)
									.ToListAsync();

				if (shares != null)
				{
					dbModel.DbSetShares.RemoveRange(shares);
					await dbModel.SaveChangesAsync();
				}
				Shares share;

				foreach (var user in user_id)
				{
					share = new Shares
					{
						Owner_Id = owner_id,
						Note_Id = note_id,
						UserId = user
					};

					dbModel.DbSetShares.Add(share);
				}

				await dbModel.SaveChangesAsync();

			}
			catch (Exception e)
			{
				throw new DbUpdateException("Error occured while adding new share to the database", e);
			}
		}

		public async Task<List<SharedNotes>> GetSharedNotes(int user_id)
		{
			try
			{
				List<SharedNotes> sharedNotes = await (from shares in dbModel.DbSetShares
													   join users in dbModel.DbSetUsers on shares.Owner_Id equals users.Id
													   where ((shares.UserId == user_id || shares.UserId == -1) && shares.Owner_Id != user_id)
													   orderby shares.Note.Timestamp ascending
													   select new SharedNotes
													   {
														   Note_Id = shares.Note.Note_Id,
														   Header = shares.Note.Header,
														   Content = shares.Note.Content,
														   Timestamp = DateTime.FromBinary(shares.Note.Timestamp),
														   Owner = users.UserName
													   })
													   .ToListAsync();
				return sharedNotes;
			}
			catch (Exception e)
			{
				throw new Exception("Error occured on database query", e);
			}
		}

	}
}
