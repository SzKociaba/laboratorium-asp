namespace BookStore.Web.Models;

public interface IBookService
{
    IEnumerable<Book> FindAll();
    Book? FineById(int id);
    void Add(Book book);
    void Edit(Book book);
    void Delete(int id);
}