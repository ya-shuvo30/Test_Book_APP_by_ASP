namespace FakeBookGenerator.Shared;

public class BookQueryParameters
{
    public string Locale { get; set; } = "en_US";
    public int Seed { get; set; }
    public double Likes { get; set; }
    public double Reviews { get; set; }
    public int Page { get; set; } = 1;
}
