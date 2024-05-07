using Kolokwium1.DTOs;
using Microsoft.Data.SqlClient;

namespace Kolokwium1.Repositories;

public class BookRepository:IBooksRepository
{
    private readonly IConfiguration _configuration;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<AuthorsOfBookDTO> getAutorsOfBook(int id)
    {
       await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
       await using SqlCommand command = new SqlCommand();
        command.Connection = connection;

        command.CommandText = "select books.PK as bookPK, title,authors.first_name as firstName, authors.last_name as lastName from books  " +
                              "join books_authors on books.PK = books_authors.FK_book " +
                              "join authors on authors.PK = books_authors.FK_author " +
                              "where books.PK = @id ";
        command.Parameters.AddWithValue("@id", id);
        
        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();

        AuthorsOfBookDTO autorsOfBook = null;

        var bookPkOrdinary = reader.GetOrdinal("bookPK");
        var bookTitleOrdinary = reader.GetOrdinal("title");
        var authorFirstNameOrdinary = reader.GetOrdinal("firstName");
        var authorLastNameOrdinary = reader.GetOrdinal("lastName");

        while (await reader.ReadAsync())
        {
            if (autorsOfBook is null)
            {
                autorsOfBook = new AuthorsOfBookDTO()
                {
                    id = reader.GetInt32(bookPkOrdinary),
                    title = reader.GetString(bookTitleOrdinary),
                    authors = new List<Author>()
                    {
                        new Author()
                        {
                            firstName = reader.GetString(authorFirstNameOrdinary),
                            lastName = reader.GetString(authorLastNameOrdinary)
                        }
                    }

                };
            }
            else
            {
                autorsOfBook.authors.Add(new Author()
                {
                    firstName = reader.GetString(authorFirstNameOrdinary),
                    lastName = reader.GetString(authorLastNameOrdinary)
                });
            }
        }




        return autorsOfBook;
    }

   

    public async Task<int> addAuthor(Author author)
    {
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        await connection.OpenAsync();
        command.CommandText = "insert into authors values(@firstName, @lastName); select @@IDENTITY as ID";

        
        command.Parameters.AddWithValue("@firstName", author.firstName);
        command.Parameters.AddWithValue("@lastName", author.lastName);
        
        
        var idOfInsertedAuthor = Convert.ToInt32(await command.ExecuteScalarAsync());
        return idOfInsertedAuthor;
        
    }

    public async Task addBookAndAuthor(int idBook, int idAuthor)
    {
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        await connection.OpenAsync();
        command.CommandText = "insert into books_authors values(@idBook, @@idAuthors)";

        
        command.Parameters.AddWithValue("@idBook", idBook);
        command.Parameters.AddWithValue("@idAuthors", idBook);

        await command.ExecuteNonQueryAsync();

    }

    public async Task<int> addBook(String title)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;

        
        command.CommandText = "insert into books values(@title); select @@IDENTITY as ID";

        command.Parameters.AddWithValue("@title", title);
        
        await connection.OpenAsync();

        var idOfInsertedBook = Convert.ToInt32(await command.ExecuteScalarAsync());
        return idOfInsertedBook;
        
        
        

        
        
        
        
        
    }
}