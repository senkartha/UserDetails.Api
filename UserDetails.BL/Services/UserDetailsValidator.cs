using System;
using UserDetailsBL.Interfaces;
using UserDetailsBL.Models;

namespace UserDetailsBL.Services
{
	public class UserDetailsValidator : IUserDetailsValidator
	{
		public UserDetailsValidator()
		{
		}

		public Boolean ValidateUserDetails(User user)
		{
			// Write your business validations here.
			return true;
		}
	}
}

