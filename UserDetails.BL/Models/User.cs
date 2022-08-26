using System.ComponentModel.DataAnnotations;

namespace UserDetailsBL.Models
{
    public class User
    {
        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        [Required]
        public string LastName { get; set; }
    }
}
