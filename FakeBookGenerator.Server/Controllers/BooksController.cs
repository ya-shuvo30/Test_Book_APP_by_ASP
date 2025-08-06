using Microsoft.AspNetCore.Mvc;
using FakeBookGenerator.Shared;
using FakeBookGenerator.Server.Services;
using System.Collections.Generic;

namespace FakeBookGenerator.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookGeneratorService _bookGeneratorService;

    // The service is injected into the controller's constructor
    public BooksController(BookGeneratorService bookGeneratorService)
    {
        _bookGeneratorService = bookGeneratorService;
    }

    [HttpGet]
    public IEnumerable<Book> Get([FromQuery] BookQueryParameters query)
    {
        // Call the service to generate books and return them
        return _bookGeneratorService.GenerateBooks(query);
    }
}
