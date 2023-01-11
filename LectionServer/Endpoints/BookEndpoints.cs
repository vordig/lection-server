using LectionServer.Contracts;
using LectionServer.Endpoints.Data;
using LectionServer.Models;
using LectionServer.Services;

namespace LectionServer.Endpoints;

public static class BookEndpoints
{
    private const string EndpointsTag = "Books";

    public static void MapBookEndpoints(this WebApplication app)
    {
        app.MapGet("api/books", GetBooks)
            .RequireAuthorization()
            .Produces<IEnumerable<Book>>()
            .WithDescription("Get all books")
            .WithTags(EndpointsTag)
            .WithOpenApi();
        
        app.MapGet("api/books/{id:guid}", GetBook)
            .RequireAuthorization()
            .Produces<Book>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Get a book with an id")
            .WithTags(EndpointsTag)
            .WithOpenApi();
        
        app.MapPost("api/books", AddBook)
            .RequireAuthorization()
            .Accepts<BookRequest>("application/json")
            .Produces<Book>(StatusCodes.Status201Created)
            .WithDescription("Create a new book")
            .WithTags(EndpointsTag)
            .WithOpenApi();
        
        app.MapPut("api/books/{id:guid}", UpdateBook)
            .RequireAuthorization()
            .Accepts<BookRequest>("application/json")
            .Produces<Book>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Update a book with an id")
            .WithTags(EndpointsTag)
            .WithOpenApi();
        
        app.MapDelete("api/books/{id:guid}", DeleteBook)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .WithDescription("Delete a book with an id")
            .WithTags(EndpointsTag)
            .WithOpenApi();
        
        app.MapPost("api/books/generate/{count:int}", GenerateBooks)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .WithDescription("Generate a list of new books with COUNT items and add it to your collection")
            .WithTags(EndpointsTag)
            .WithOpenApi();
        
        app.MapDelete("api/books", ClearBooks)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .WithDescription("Delete all books")
            .WithTags(EndpointsTag)
            .WithOpenApi();
    }
    
    private static IResult GetBooks(BookService bookService, RequestData requestData, CancellationToken cancellationToken)
    {
        var result = bookService.GetBooks(requestData.UserId);
        return Results.Json(result);
    }
    
    private static IResult GetBook(BookService bookService, RequestData requestData, Guid id, CancellationToken cancellationToken)
    {
        var result = bookService.GetBook(id, requestData.UserId);
        return result is null ? Results.NotFound() : Results.Json(result);
    }
    
    private static IResult AddBook(BookService bookService, RequestData requestData, BookRequest request, CancellationToken cancellationToken)
    {
        var result = bookService.AddBook(request, requestData.UserId);
        return Results.Created($"api/books/{result.Id}", result);
    }
    
    private static IResult UpdateBook(BookService bookService, RequestData requestData, Guid id, BookRequest request, CancellationToken cancellationToken)
    {
        var result = bookService.UpdateBook(id, request, requestData.UserId);
        return result is null ? Results.NotFound() : Results.Json(result);
    }
    
    private static IResult DeleteBook(BookService bookService, RequestData requestData, Guid id, CancellationToken cancellationToken)
    {
        bookService.DeleteBook(id, requestData.UserId);
        return Results.Ok();
    }
    
    private static IResult GenerateBooks(BookService bookService, RequestData requestData, int count, CancellationToken cancellationToken)
    {
        bookService.GenerateBooks(count, requestData.UserId);
        return Results.Ok();
    }
    
    private static IResult ClearBooks(BookService bookService, RequestData requestData, CancellationToken cancellationToken)
    {
        bookService.ClearBooks(requestData.UserId);
        return Results.Ok();
    }
}