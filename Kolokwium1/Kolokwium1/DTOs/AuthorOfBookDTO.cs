namespace Kolokwium1.DTOs;

public class AuthorsOfBookDTO
{
    public int id { get; set; }
    public String title { get; set; } = String.Empty;
    public List<Author> authors { get; set; } = new List<Author>();
}

public class Author
{
    public String firstName { get; set; } = String.Empty;
    public String lastName { get; set; } = String.Empty;
}