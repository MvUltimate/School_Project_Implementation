using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Models;

namespace WebApi_SchoolProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SAPsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public SAPsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: api/SAPs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SAP>>> GetSAPs()
        {
            return await _context.SAPs.ToListAsync();
        }
    }
}
