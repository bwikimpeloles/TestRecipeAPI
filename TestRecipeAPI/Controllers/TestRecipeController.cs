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

        //favourite
        [HttpGet("GetFavourite")]
        public async Task<ActionResult<List<Favourite>>> GetFavourite()
        {
            return Ok(await _context.Favourites.ToListAsync());
        }

        [HttpPost("CreateFavourite")]
        public async Task<ActionResult<List<TestRecipe>>> CreateFavourite(Favourite favourite)
        {
            _context.Favourites.Add(favourite);
            await _context.SaveChangesAsync();

            return Ok(await _context.Favourites.ToListAsync());
        }

        [HttpPut("UpdateFavourite")]
        public async Task<ActionResult<List<TestRecipe>>> UpdateFavourite(Favourite favourite)
        {
            var dbFavourite = await _context.Favourites.FindAsync(favourite.Id);
            if (dbFavourite == null)
                return BadRequest("Favourite not found.");

            dbFavourite.ProductId = favourite.ProductId;
            dbFavourite.Username = favourite.Username;
            dbFavourite.FavouriteBool = favourite.FavouriteBool;

            await _context.SaveChangesAsync();

            return Ok(await _context.Favourites.ToListAsync());
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
            if (account == null) return NotFound( new {Message = "Account Not Found!"});

            account.Token = CreateJwtToken(account);
            return Ok(new
            {
                Token = account.Token,
                Message = "Login Success!."
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccount([FromBody] Account accountObj)
        {
            if (accountObj == null) return BadRequest();

            //check username exist
            if(await CheckUserNameExistAsync(accountObj.Username))
                return BadRequest(new { Message = "Username Already Exist!" });

            //chek username exist
            if (await CheckUserEmailExistAsync(accountObj.Email))
                return BadRequest(new { Message = "Email Already Exist!" });


            await _context.Accounts.AddAsync(accountObj);   
            await _context.SaveChangesAsync();  

            return Ok(new
            {
                Token="",
                Message = "Account Registered!."
            });
        }

        private Task<bool> CheckUserNameExistAsync(string username) { 
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

        [HttpGet("userfavourite/{username}/{productid}")]
        public Task<bool> CheckUserFavourite(string username, int productid)
        {
            return _context.Favourites.AnyAsync(x => x.Username == username && x.ProductId == productid && x.FavouriteBool == true);
        }

    }
}