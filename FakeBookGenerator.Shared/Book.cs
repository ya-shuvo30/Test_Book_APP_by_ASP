using System.Collections.Generic;

namespace FakeBookGenerator.Shared;

public class Book
{
    public int Id { get; set; }
    public required string Isbn { get; set; }
    public required string Title { get; set; }
    public List<Author> Authors { get; set; } = new();
    public required string Publisher { get; set; }
    public List<Review> Reviews { get; set; } = new();
    public int Likes { get; set; }
}
