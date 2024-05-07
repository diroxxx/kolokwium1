using Kolokwium1.DTOs;
using Kolokwium1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController:ControllerBase
{
    private readonly IBooksRepository _booksRepository;
    public BooksController(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }
    
    [HttpGet]
    [Route("api/books/{id}/authors")]
    public async Task<IActionResult> getAutorsOfBook(int id)
    {
        var authors = await _booksRepository.getAutorsOfBook(id);
        return Ok(authors);
    }

    [HttpPost]
    [Route("api/books")]
    public async Task<IActionResult> addBookWithAuthors(AddBook addBook)
    {
        var idBook = await _booksRepository.addBook(addBook.title);
        
        AuthorsOfBookDTO authorsOfBookDto = new AuthorsOfBookDTO();
        authorsOfBookDto.id = idBook;
        authorsOfBookDto.title = addBook.title;
        authorsOfBookDto.authors= new List<Author>(addBook.authors);

        foreach (var author in addBook.authors)
        {
            
            var idAuthor = await _booksRepository.addAuthor(author);

          await _booksRepository.addBookAndAuthor(idBook, idAuthor);
        }


        return Created(Request.Path.Value ?? $"api/{idBook}/authors", authorsOfBookDto);
    }
    




}