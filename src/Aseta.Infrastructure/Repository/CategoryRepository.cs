using System;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class CategoryRepository(AppDbContext context) : Repository<Category, int>(context), ICategoryRepository
{
}
