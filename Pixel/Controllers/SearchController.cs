using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixel.Database;
using System.Linq;
using System.Threading.Tasks;

namespace Pixel.Controllers
{
    public class SearchController : Controller
    {
        private readonly AccountContext _context;
        public SearchController(AccountContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> FindPerson(string userName)
        {
            var users = await _context.UsersModel.Where(s => s.UserLogin.Contains(userName)).Select(s => s.UserLogin).Take(5).ToListAsync();
            return PartialView("_Search", users);
        }
    }
}
