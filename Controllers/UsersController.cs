using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly BookRepository _bookRepository;

        public UsersController(UserRepository userRepository, BookRepository bookRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(_userRepository.GetAll());
        }
    
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userRepository.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    
        [HttpPost]
        public ActionResult<User> CreateUser(User newUser)
        {
            var createdUser = _userRepository.Add(newUser);
        
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }
        
        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public ActionResult<User> UpdateUser(int id, User updatedUser)
        {
            var existingUser = _userRepository.Get(id);
            if (existingUser == null)
            {
                return NotFound();
            }
        
            existingUser.Name = updatedUser.Name;
            _userRepository.Update(existingUser);
        
            return Ok(existingUser);
        }
    
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.Get(id);
            if (user == null)
            {
                return NotFound();
            }
    
            var books = _bookRepository.GetAll().Where(b => b.UserId == id);
            foreach (var book in books)
            {
                book.UserId = null;
                _bookRepository.Update(book);
            }
    
            _userRepository.Delete(id);
            return NoContent();
        }
        

        [HttpDelete("")]
        public IActionResult DeleteAllUsers()
        {
            _userRepository.DeleteAll();
            return NoContent();
        }
    }
}