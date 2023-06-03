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
    public class BooksController : ControllerBase
    {
        private readonly BookRepository _bookRepository;
        private readonly UserRepository _userRepository;

        public BooksController(BookRepository bookRepository, UserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks(int? userId)
        {
            if (userId.HasValue)
            {
                var books = _bookRepository.GetByUserId(userId.Value);
                return Ok(books);
            }
            else
            {
                return Ok(_bookRepository.GetAll());
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public ActionResult<Book> PostBook(Book book)
        {
            if (book.UserId.HasValue)
            {
                var user = _userRepository.Get(book.UserId.Value);
                if (user == null)
                {
                    return BadRequest("User with provided UserId does not exist");
                }
            }

            _bookRepository.Add(book);

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult PutBook(int id, Book updatedBook)
        {
            var existingBook = _bookRepository.Get(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            if (updatedBook.UserId.HasValue)
            {
                var user = _userRepository.Get(updatedBook.UserId.Value);
                if (user == null)
                {
                    return BadRequest("User with provided UserId does not exist");
                }
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.PublishedYear = updatedBook.PublishedYear;
            existingBook.UserId = updatedBook.UserId;

            _bookRepository.Update(existingBook);

            return Ok(existingBook);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            _bookRepository.Delete(id);
            return NoContent();
        }

        [HttpDelete("")]
        public IActionResult DeleteAllBooks()
        {
            _bookRepository.DeleteAll();
            return NoContent();
        }
    }
}