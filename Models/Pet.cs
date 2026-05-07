namespace PetstoreApiTests.Models;

public class Pet
{
    public long Id { get; set; }

    public Category? Category { get; set; }

    public string? Name { get; set; }

    public List<string>? PhotoUrls { get; set; }

    public List<Tag>? Tags { get; set; }

    public string? Status { get; set; }

}