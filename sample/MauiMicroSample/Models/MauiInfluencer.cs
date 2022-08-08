namespace MauiMicroSample.Models;

#nullable enable
public record MauiInfluencer
{
    public string? Name { get; init; }
    public string? GitHub { get; init; }
    public string? Twitter { get; init; }
    public string? Bio { get; init; }
    public string? Avatar { get; init; }
    public bool Mvp { get; init; }
    public bool Microsoft { get; init; }
}
