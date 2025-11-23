# План рефакторинга пагинации

## 1. Цель

Создать универсальный, переиспользуемый и типобезопасный механизм для keyset-пагинации, который можно будет легко применять к любым сущностям в проекте (Inventories, Items, Categories и т.д.), избегая дублирования кода.

## 2. Проблема текущего подхода

Если мы просто скопируем существующую логику пагинации из `InventoryRepository` в `ItemRepository` и другие репозитории, мы столкнемся с проблемами:
- **Дублирование кода:** Любое изменение в логике пагинации (например, исправление бага или улучшение) придется вносить в нескольких местах.
- **Плохая поддерживаемость:** Кодовая база станет сложнее для понимания и поддержки.
- **Ошибки:** При копировании легко допустить ошибку, забыв изменить имя типа или поля.

## 3. Предлагаемое решение: Паттерн "Строитель"

Мы создадим универсальный класс `KeysetPaginator<T>`, который будет отвечать за всю сложную логику построения запроса для пагинации. Этот класс будет настраиваться для конкретной сущности с помощью текучего интерфейса (fluent API), а затем выполнять пагинацию.

Пример использования в репозитории:
```csharp
// 1. Создаем и настраиваем пагинатор
var paginator = new KeysetPaginator<Inventory>(query)
    .AddSortableField("Name", entity => entity.Name)
    .AddSortableField("Date", entity => entity.CreatedAt);

// 2. Выполняем пагинацию
var pagedResult = await paginator.PaginateAsync(
    sortBy: "Name",
    sortOrder: "asc",
    pageSize: 20,
    cursor: "eyJuYW1lIjoiSXRlbSAxMCIsImlkIjoiZjc4YzRhM2Qt...sda"
);
```

## 4. План реализации

### Шаг 1: Создание общего интерфейса `IEntity`

Чтобы наш универсальный пагинатор мог работать с любой сущностью, ему нужен способ гарантированно получить ее уникальный идентификатор `Id`. Для этого мы создадим простой интерфейс и реализуем его в наших основных сущностях.

**Действие:** Создать файл `src/Aseta.Domain/Abstractions/Primitives/IEntity.cs` со следующим содержимым:
```csharp
namespace Aseta.Domain.Abstractions.Primitives;

public interface IEntity
{
    Guid Id { get; }
}
```

**Действие:** Модифицировать классы сущностей (`Inventory`, `Item`, `Category` и т.д.), чтобы они реализовывали этот интерфейс.
Пример для `Inventory.cs`:
```csharp
// ... using Aseta.Domain.Abstractions.Primitives;
public class Inventory : IEntity 
{
    // Guid Id { get; private set; } // Это свойство уже должно существовать
    // ...
}
```

### Шаг 2: Создание каркаса класса `KeysetPaginator<T>`

Этот класс будет ядром нашего решения. Он будет generic (`<T>`) и будет ограничен сущностями, реализующими `IEntity`.

**Действие:** Создать файл `src/Aseta.Infrastructure/Pagination/KeysetPaginator.cs` со следующим каркасом:
```csharp
using Aseta.Domain.Abstractions.Primitives;
using System.Linq.Expressions;

namespace Aseta.Infrastructure.Pagination;

// Ограничиваем T сущностями, у которых есть Guid Id
public class KeysetPaginator<T> where T : class, IEntity 
{
    private readonly IQueryable<T> _query;
    private readonly Dictionary<string, Expression<Func<T, object>>> _sortFields = new();

    public KeysetPaginator(IQueryable<T> query)
    {
        _query = query;
    }

    // Метод для добавления полей, по которым можно сортировать
    public KeysetPaginator<T> AddSortableField(string key, Expression<Func<T, object>> keySelector)
    {
        _sortFields.Add(key, keySelector);
        return this;
    }

    // Основной метод, выполняющий пагинацию
    public async Task<(ICollection<T> items, string? nextCursor, bool hasNextPage)> PaginateAsync(
        string sortBy,
        string sortOrder,
        int pageSize,
        string? cursor,
        CancellationToken cancellationToken = default)
    {
        // ... Здесь будет основная логика ...
        throw new NotImplementedException();
    }
}
```

### Шаг 3: Реализация логики `KeysetPaginator<T>`

Это самый сложный шаг. Мы реализуем `PaginateAsync` и вспомогательные приватные методы внутри `KeysetPaginator`.
- **Сортировка:** Метод будет находить нужное `Expression` в словаре `_sortFields` и динамически строить `OrderBy().ThenBy()`.
- **Фильтрация по курсору:** Будет построена универсальная `WHERE` клауза с помощью `Expression Trees`. Это позволит нам не использовать гигантский `switch`, а строить запрос на лету для любого типа данных.
- **Кодирование/декодирование курсора:** Логика будет обобщена для работы с разными типами данных.

### Шаг 4: Рефакторинг `InventoryRepository`

После реализации пагинатора мы обновим репозиторий, чтобы он использовал новый механизм.

**Действие:** Заменить текущую реализацию `GetPaginatedWithKeysetAsync` в `InventoryRepository.cs` на новую.

*Было:*
```csharp
public async Task<(ICollection<Inventory> inventories, ...)> GetPaginatedWithKeysetAsync(...) 
{
    // ... много кода для фильтрации, сортировки, пагинации ...
}
```

*Станет:*
```csharp
public async Task<(ICollection<Inventory> inventories, string? nextCursor, bool hasNextPage)> GetPaginatedWithKeysetAsync(
    InventoryQueryPaginationParameters parameters,
    CancellationToken cancellationToken)
{
    IQueryable<Inventory> query = _dbSet.AsNoTracking()
                                      .Include(i => i.Creator)
                                      .Include(i => i.Tags);

    query = ApplyFilters(query, parameters); // Фильтры остаются в репозитории

    var paginator = new KeysetPaginator<Inventory>(query)
        .AddSortableField(nameof(SortBy.Name), i => i.Name)
        .AddSortableField(nameof(SortBy.Date), i => i.CreatedAt)
        .AddSortableField(nameof(SortBy.Creator), i => i.Creator.UserName)
        .AddSortableField(nameof(SortBy.NumberOfItems), i => i.ItemsCount);

    return await paginator.PaginateAsync(
        parameters.SortBy.ToString(),
        parameters.SortOrder,
        parameters.PageSize,
        parameters.Cursor,
        cancellationToken
    );
}
```
*Примечание: `ApplyFilters` остается отдельным методом в репозитории, т.к. фильтрация специфична для каждой сущности.*

## 5. Ожидаемый результат

- Вся сложная логика пагинации инкапсулирована в одном классе (`KeysetPaginator<T>`).
- Репозитории содержат только код, относящийся к их сущности (фильтрация и настройка пагинатора).
- Добавление пагинации в новый репозиторий сводится к нескольким строкам кода.
- Код становится чистым, поддерживаемым и масштабируемым.

---
Жду ваших правок и комментариев.
