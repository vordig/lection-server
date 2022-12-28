using System.Collections.Immutable;
using Bogus;
using LectionServer.Contracts;
using LectionServer.Models;

namespace LectionServer.Services;

public class BookService
{
    private readonly List<Book> _books = new();

    private readonly Faker<Book> _bookGenerator = new Faker<Book>("ru")
        .RuleFor(x => x.Id, faker => faker.Random.Guid())
        .RuleFor(x => x.Name, faker => faker.Lorem.Sentence())
        .RuleFor(x => x.Author, faker => $"{faker.Name.LastName()} {faker.Name.FirstName()[0]}.");

    public IImmutableList<Book> GetBooks() => _books.ToImmutableList();
    public Book? GetBook(Guid id) => _books.SingleOrDefault(x => x.Id == id);

    public Book AddBook(BookRequest request)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Author = request.Author,
            Name = request.Name
        };
        _books.Add(book);
        return book;
    }

    public Book? UpdateBook(Guid id, BookRequest request)
    {
        var book = GetBook(id);
        if (book is null) return null;
        book.Author = request.Author;
        book.Name = request.Name;
        return book;
    }

    public void DeleteBook(Guid id)
    {
        var book = GetBook(id);
        if (book is null) return;
        _books.Remove(book);
    }

    public void GenerateBooks(int count) => _books.AddRange(_bookGenerator.Generate(count));
    public void ClearBooks() => _books.Clear();
}