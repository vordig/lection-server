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

    public IImmutableList<Book> GetBooks(Guid userId) => _books.Where(x => x.UserId == userId).ToImmutableList();
    public Book? GetBook(Guid id, Guid userId) => _books.SingleOrDefault(x => x.Id == id && x.UserId == userId);

    public Book AddBook(BookRequest request, Guid userId)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Author = request.Author,
            Name = request.Name
        };
        _books.Add(book);
        return book;
    }

    public Book? UpdateBook(Guid id, BookRequest request, Guid userId)
    {
        var book = GetBook(id, userId);
        if (book is null) return null;
        book.Author = request.Author;
        book.Name = request.Name;
        return book;
    }

    public void DeleteBook(Guid id, Guid userId)
    {
        var book = GetBook(id, userId);
        if (book is null) return;
        _books.Remove(book);
    }

    public void GenerateBooks(int count, Guid userId)
    {
        var books = _bookGenerator.Generate(count);
        foreach (var book in books)
            book.UserId = userId;
        _books.AddRange(books);
    }

    public void ClearBooks(Guid userId) => _books.RemoveAll(x => x.UserId == userId);
}