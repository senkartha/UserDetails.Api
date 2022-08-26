using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using UserDetailsBL.Interfaces;
using UserDetailsBL.Models;

namespace UserDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserDetailsValidator userDetailsValidator;
        IDataSourceOperator<User> _userDataSource;

        public UserController(ILoggerFactory loggerFactory, IUserDetailsValidator userDetailsValidator, IDataSourceOperator<User> userDataSource)
        {
            this.logger = loggerFactory.CreateLogger("ItemPoolController");
            this.userDetailsValidator = userDetailsValidator;
            this._userDataSource = userDataSource;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (ModelState.IsValid && this.userDetailsValidator.ValidateUserDetails(user))
            {
                return new ObjectResult(await this._userDataSource.Save(user));
            }
            else
            {
                string validationErrors = string.Join(",", ModelState.Keys
                     .SelectMany(key => ModelState[key].Errors.Select(x => x.ErrorMessage)));
                return new BadRequestObjectResult(String.Format("Invalid user Details. Error : {0}", validationErrors));
            }
        }

    }
}
