using System;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Categories.GetAll;

public sealed record GetAllCategoriesQuery : IQuery<CategoriesResponse>;
