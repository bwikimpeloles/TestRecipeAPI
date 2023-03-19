using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestRecipeAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TestRecipeAPI.Entities;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Client;

namespace TestRecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRecipeController : ControllerBase
    {
        private readonly DataContext _context;

        public TestRecipeController(DataContext context)
        {
            _context = context;
        }

        //recipe

        [HttpGet]
        public async Task<ActionResult<List<TestRecipe>>> GetTestRecipes()
        {
            return Ok(await _context.TestRecipes.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<TestRecipe>>> CreateTestRecipe(TestRecipe recipe)
        {
            _context.TestRecipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(await _context.TestRecipes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<TestRecipe>>> UpdateTestRecipe(TestRecipe recipe)
        {
            var dbRecipe = await _context.TestRecipes.FindAsync(recipe.Id);
            if (dbRecipe == null)
                return BadRequest("Recipe not found.");

            dbRecipe.Name = recipe.Name;
            dbRecipe.Ingredient = recipe.Ingredient;
            dbRecipe.Instruction = recipe.Instruction;
            dbRecipe.Favourite = recipe.Favourite;



            await _context.SaveChangesAsync();

            return Ok(await _context.TestRecipes.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<TestRecipe>>> DeleteTestRecipe(int id)
        {
            var dbRecipe = await _context.TestRecipes.FindAsync(id);
            if (dbRecipe == null)
                return BadRequest("Recipe not found.");

            _context.TestRecipes.Remove(dbRecipe);
            await _context.SaveChangesAsync();

            return Ok(await _context.TestRecipes.ToListAsync());
        }

        [HttpPost("favourite")]
        //AccountRecipe => Favourite
        public async Task<ActionResult<Account>> AddFavourite(int accountsId, int testRecipeId)
        {
            var accounts = await _context.Accounts.Where(c => c.Id == accountsId)
                .Include(c => c.TestRecipes)
                .FirstOrDefaultAsync();
            if (accounts == null)
                return NotFound();

            var recipes = await _context.TestRecipes.FindAsync(testRecipeId);
            if (recipes == null)
                return NotFound();

            accounts.TestRecipes.Add(recipes);
            await _context.SaveChangesAsync();

            return accounts;
        }

        [HttpDelete("favourite")]
        //AccountRecipe => Favourite
        public async Task<ActionResult<Account>> DeleteFavourite(int accountsId, int testRecipeId)
        {
            var accounts = await _context.Accounts.Where(c => c.Id == accountsId)
                .Include(c => c.TestRecipes)
                .FirstOrDefaultAsync();
            if (accounts == null)
                return NotFound();

            var recipes = await _context.TestRecipes.FindAsync(testRecipeId);
            if (recipes == null)
                return NotFound();

            accounts.TestRecipes.Remove(recipes);
            await _context.SaveChangesAsync();

            return accounts;
        }

        [HttpGet("favouriteaccount")]
        public async Task<ActionResult<IEnumerable<Account>>> GetFavourites(int accountsId)
        {
            var accounts = await _context.Accounts.Where(c => c.Id == accountsId)
                .Include(c => c.TestRecipes)
                .FirstOrDefaultAsync();
            if (accounts == null)
                return NotFound();

            return await _context.Accounts
                .Include(x => x.TestRecipes).Where(c => c.Id == accountsId)
                        .ToListAsync();
        }

        [HttpGet("favouriterecipe")]
        public async Task<ActionResult<IEnumerable<TestRecipe>>> GetFavouritesRecipe(int testRecipesId)
        {
            var recipes = await _context.TestRecipes.Where(c => c.Id == testRecipesId)
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync();
            if (recipes == null)
                return NotFound();

            return await _context.TestRecipes
                .Include(x => x.Accounts).Where(c => c.Id == testRecipesId)
                        .ToListAsync();
        }


        //account
        [HttpGet("GetAccount")]
        public async Task<ActionResult<List<Account>>> GetAccount()
        {
            return Ok(await _context.Accounts.ToListAsync());
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Account accountObj)
        {
            if (accountObj == null) return BadRequest();

            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Username == accountObj.Username && x.Password == accountObj.Password);
            if (account == null) return NotFound(new { Message = "Account Not Found!" });

            account.Token = CreateJwtToken(account);
            return Ok(new
            {
                Token = account.Token,
                Message = "Login Success!"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccount([FromBody] Account accountObj)
        {
            if (accountObj == null) return BadRequest();

            //check username exist
            if (await CheckUserNameExistAsync(accountObj.Username))
                return BadRequest(new { Message = "Username Already Exist!" });

            //chek username exist
            if (await CheckUserEmailExistAsync(accountObj.Email))
                return BadRequest(new { Message = "Email Already Exist!" });


            await _context.Accounts.AddAsync(accountObj);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = "",
                Message = "Account Registered!."
            });
        }

        private Task<bool> CheckUserNameExistAsync(string username)
        {
            return _context.Accounts.AnyAsync(x => x.Username == username);
        }

        private Task<bool> CheckUserEmailExistAsync(string email)
        {
            return _context.Accounts.AnyAsync(x => x.Email == email);
        }

        private string CreateJwtToken(Account account)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Sid, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.Role),
                new Claim(ClaimTypes.Name, account.Username),
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        [Authorize]
        [HttpGet("allusers")]
        public async Task<ActionResult<Account>> GetAllAccounts()
        {
            return Ok(await _context.Accounts.ToListAsync());
        }


    }
}