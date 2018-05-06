using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NoteBase.Models
{
    public class Users : IdentityUser<int>
    {
        public List<Note> Notes { get; set; }
        public List<Shares> Shares { get; set; }
    }

	public class UserRole : IdentityRole<int>
	{
		public UserRole() : base()
		{
		}

		public UserRole(int roleId)
		{
			Id = roleId;
			Name = roleId.ToString();
		}
	}
}
