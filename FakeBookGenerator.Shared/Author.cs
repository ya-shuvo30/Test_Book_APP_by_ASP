namespace FakeBookGenerator.Shared;

public class Author
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
