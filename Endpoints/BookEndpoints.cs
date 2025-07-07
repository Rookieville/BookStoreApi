using bookapi_minimal.Contracts;
using bookapi_minimal.Interfaces;

namespace bookapi_minimal.Endpoints
{
    public static class BookEndPoint
    {
        public static IEndpointRouteBuilder MapBookEndPoint(this IEndpointRouteBuilder app)
        {
            // Endpoint to add a new book
            app.MapPost("/books", async (CreateBookRequest createBookRequest, IBookService bookService) =>
            {
                var result = await bookService.AddBookAsync(createBookRequest);
                return Results.Created($"/books/{result.Id}", result);
            })
            .RequireAuthorization("UserOrAdmin")
            .WithName("CreateBook")
            .WithSummary("Create a new book")
            .WithDescription("Add a new book to the store (requires authentication)");


            // Endpoint to get all books
            app.MapGet("/books", async (IBookService bookService) =>
            {
                var result = await bookService.GetBooksAsync();
                return Results.Ok(result);
            })
            .WithName("GetAllBooks")
            .WithSummary("Get all books")
            .WithDescription("Retrieve all books from the store (public endpoint)");

            // Endpoint to get a book by ID
            app.MapGet("/books/{id:guid}", async (Guid id, IBookService bookService) =>
            {
                var result = await bookService.GetBookByIdAsync(id);
                return result != null ? Results.Ok(result) : Results.NotFound();
            })
            .WithName("GetBookById")
            .WithSummary("Get book by ID")
            .WithDescription("Retrieve a specific book by its ID (public endpoint)");


            // Endpoint to update a book by ID
            app.MapPut("/books/{id:guid}", async (Guid id, UpdateBookRequest updateBookRequest, IBookService bookService) =>
            {
                var result = await bookService.UpdateBookAsync(id, updateBookRequest);
                return result != null ? Results.Ok(result) : Results.NotFound();
            })
            .RequireAuthorization("UserOrAdmin")
            .WithName("UpdateBook")
            .WithSummary("Update a book")
            .WithDescription("Update an existing book (requires authentication)");

            // Endpoint to delete a book by ID
            app.MapDelete("/books/{id:guid}", async (Guid id, IBookService bookService) =>
            {
                var result = await bookService.DeleteBookAsync(id);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .RequireAuthorization("AdminOnly")
            .WithName("DeleteBook")
            .WithSummary("Delete a book")
            .WithDescription("Delete a book from the store (requires admin role)");

            return app;
        }
    }
}