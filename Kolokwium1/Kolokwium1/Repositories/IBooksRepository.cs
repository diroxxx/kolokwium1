using Kolokwium1.DTOs;

namespace Kolokwium1.Repositories;

public interface IBooksRepository
{
    Task<AuthorsOfBookDTO> getAutorsOfBook(int id);
    Task<bool> DoesBookExist(int id);
    Task<int> addBook(String title);
    Task<int> addAuthor(Author author);

    Task addBookAndAuthor(int idBook, int idAuthor);
}