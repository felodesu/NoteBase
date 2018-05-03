using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


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

        public async Task CreateNewUser(Users user)
        {
            try
            {
                dbModel.DbSetUsers.Add(new Users { Name = user.Name, Password = user.Password });
                await dbModel.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DbUpdateException("Error occured while adding user to the database", e);
            }
        }

        public bool CheckUserExists(string name)
        {
            try
            {
                var queryResult = (from u in dbModel.DbSetUsers
                                   where u.Name == name
                                   select u).SingleOrDefault();
                return queryResult == null ? false : true;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured on database query", e);
            }
        }

        public bool ValidateUser(Users user)
        {
            try
            {
                if (CheckUserExists(user.Name))
                {
                    var userPassword = (from u in dbModel.DbSetUsers
                                        where u.Name == user.Name
                                        select u.Password).SingleOrDefault();

                    if (userPassword == null) return false;

                    if (userPassword.Equals(user.Password))
                        return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured on database query", e);
            }
        }

        public async Task<int> GetUserId(string username)
        {
            try
            {
                int userId = await (from u in dbModel.DbSetUsers
                          where u.Name == username
                          select u.User_Id).SingleOrDefaultAsync();
                return userId;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured on database query", e);
            }
        }

        public async Task CreateNote(Note note, string user)
        {
            try
            {
                note.User_Id = await GetUserId(user);
                dbModel.DbSetNotes.Add(note);
                await dbModel.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DbUpdateException("Error occured while adding note to the database", e);
            }
        }

        public async Task<List<SharedNotes>> GetUserNotes(string user)
        {
            try
            {
                int user_id = await GetUserId(user);
                List<SharedNotes> sharedNotes = await (from shares in dbModel.DbSetShares
                                                       join note in dbModel.DbSetNotes on shares.Note_Id equals note.Note_Id
                                                       where (note.User_Id == user_id)
                                                       orderby note.Timestamp ascending
                                                       select new SharedNotes
                                                       {
                                                           Note_Id = note.Note_Id,
                                                           Note_Header = note.Header,
                                                           Note_Content = note.Content,
                                                           Note_Timestamp = note.Timestamp,
                                                           Owner_Id = note.User_Id,
                                                           Shared_To = shares.User_Id
                                                       }
                                                        ).ToListAsync();
                SharedNotes.Sort(sharedNotes);
                return sharedNotes;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured on database query", e);
            }
        }

        public async Task<List<Users>> GetUsers()
        {
            try
            {
                List<Users> users = await (from user in dbModel.DbSetUsers
                                           orderby user.Name ascending
                                           select user).ToListAsync();
                return users;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured on database query", e);
            }
        }

		public async Task<List<string>> GetUsersById(List<int> users)
		{
			try
			{
				List<string> usernames = await (from user in dbModel.DbSetUsers
										   orderby user.Name ascending
										   select user.Name).ToListAsync();
				return usernames;
			}
			catch (Exception e)
			{
				throw new Exception("Error occured on database query", e);
			}
		}

		public async Task ShareNote(int note_id, int user_id, string owner)
        {
            try
            {
                Shares share = new Shares
                {
                    Owner_Id = await GetUserId(owner),
                    Note_Id = note_id,
                    User_Id = user_id
                };
                dbModel.DbSetShares.Add(share);

                await dbModel.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DbUpdateException("Error occured while adding new share to the database", e);
            }
        }

        public async Task<List<SharedNotes>> GetSharedNotes (string user)
        {
            try
            {
                int user_id = await GetUserId(user);
                List<SharedNotes> sharedNotes = await (from shares in dbModel.DbSetShares
                                                       join note in dbModel.DbSetNotes on shares.Note_Id equals note.Note_Id
                                                       where ((shares.User_Id == user_id || shares.User_Id == 0) && shares.Owner_Id != user_id)
                                                       orderby note.Timestamp ascending
                                                       select new SharedNotes
                                                       {
                                                           Note_Id = note.Note_Id,
                                                           Note_Header = note.Header,
                                                           Note_Content = note.Content,
                                                           Note_Timestamp = note.Timestamp,
                                                           Owner_Id = note.User_Id,
                                                           Shared_To = shares.User_Id
                                                        }
                                                        ).ToListAsync();
                return sharedNotes;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured on database query", e);
            }
        }
    }
}
