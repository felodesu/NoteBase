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

        //DbSetUsers actions

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
                                   select u)
                                   .SingleOrDefault();
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
                                        select u.Password)
                                        .SingleOrDefault();

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
                                    select u.User_Id)
                                    .SingleOrDefaultAsync();
                return userId;
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
                List<Users> users = await (from u in dbModel.DbSetUsers
                                           orderby u.Name ascending
                                           select u).ToListAsync();
                return users;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured on database query", e);
            }
        }

        //DbSetNotes actions

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



        public async Task<List<SharedNotes>> GetUserNotes(string username)
        {
            try
            {
                int user_id = await GetUserId(username);
                List<SharedNotes> userNotes = await (from n in dbModel.DbSetNotes
                                                       where (n.User_Id == user_id)
                                                       orderby n.Timestamp
                                                       select new SharedNotes
                                                       {
                                                           Note_Id = n.Note_Id,
                                                           Header = n.Header,
                                                           Content = n.Content,
                                                           Timestamp = DateTime.FromBinary(n.Timestamp)
                                                       })
                                                       .ToListAsync();

                foreach (var note in userNotes)
                {
                    note.Shared_To_Usernames = await (from s in dbModel.DbSetShares
                                                      where (s.Owner_Id == user_id && s.Note_Id == note.Note_Id)
                                                      select new
                                                      {
                                                          s.User.User_Id,
                                                          s.User.Name
                                                      })
                                                      .ToDictionaryAsync(user => user.User_Id, user => user.Name);
                }

                return userNotes;
            }
            catch (Exception e)
            {
                throw new Exception("Error occured on database query", e);
            }
        }

        //DbSetShares actions

        public async Task ShareNote(int note_id, List<int> user_id, string owner)
        {
            try
            {
                int owner_id = await GetUserId(owner);

                var shares = await (from s in dbModel.DbSetShares
                                       where (s.Owner_Id == owner_id && s.Note_Id == note_id)
                                       select s).ToListAsync();

                dbModel.DbSetShares.RemoveRange(shares);

                await dbModel.SaveChangesAsync();

                Shares share;

                foreach (var user in user_id)
                {
                    share = new Shares
                    {
                        Owner_Id = owner_id,
                        Note_Id = note_id,
                        User_Id = user
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

        public async Task<List<SharedNotes>> GetSharedNotes (string user)
        {
            try
            {
                int user_id = await GetUserId(user);

                List<SharedNotes> sharedNotes = await (from shares in dbModel.DbSetShares
                                                       join users in dbModel.DbSetUsers on shares.Owner_Id equals users.User_Id
                                                       where ((shares.User_Id == user_id || shares.User_Id == -1) && shares.Owner_Id != user_id)
                                                       orderby shares.Note.Timestamp ascending
                                                       select new SharedNotes
                                                       {
                                                           Note_Id = shares.Note.Note_Id,
                                                           Header = shares.Note.Header,
                                                           Content = shares.Note.Content,
                                                           Timestamp = DateTime.FromBinary(shares.Note.Timestamp),
                                                           Owner = users.Name
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
