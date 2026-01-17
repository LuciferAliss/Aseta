using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;

// namespace Aseta.Application.Tags.AddTagsInventory;

// // public sealed class AddTagsInventoryCommandHandler(ITagRepository tagRepository, IInventoryRepository inventoryRepository) : ICommandHandler<AddTagsInventoryCommand>
// // {
// //     public async Task<Result> Handle(AddTagsInventoryCommand command, CancellationToken cancellationToken)
// //     {
// //         Inventory? inventory = await inventoryRepository.GetByIdAsync(command.InventoryId, true, cancellationToken);

// //         if (inventory is null)
// //         {
// //             return InventoryErrors.NotFound(command.InventoryId);
// //         }

// //     }
// // }
