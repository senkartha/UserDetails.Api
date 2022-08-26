using UserDetailsBL.Models;

namespace UserDetailsBL.Interfaces
{
    public interface IUserDetailsValidator
    {
        public Boolean ValidateUserDetails(User user);
    }
}