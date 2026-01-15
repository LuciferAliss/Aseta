namespace Aseta.Application.Categories.GetAll;

public sealed record CategoriesResponse(ICollection<CategoryResponse> Categories);
