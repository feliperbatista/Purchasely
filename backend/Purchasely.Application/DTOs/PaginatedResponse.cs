namespace Purchasely.Application.DTOs;

public record PaginatedResponse<T>(
    IReadOnlyList<T> Items,
    int CurrentPage,
    int PageSize,
    int TotalCount
)
{
    public int ItemsCount => Items.Count;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages; 
}