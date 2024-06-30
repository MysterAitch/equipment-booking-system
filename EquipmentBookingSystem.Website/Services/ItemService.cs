using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using EquipmentBookingSystem.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EquipmentBookingSystem.Website.Services;

public class ItemService : IItemService
{
    private readonly WebsiteDbContext _context;

    public ItemService(WebsiteDbContext context)
    {
        _context = context;
    }


    public async Task<Item> CreateNew(User currentUser, Item item)
    {
        // insert new item entity
        var itemEntity = EquipmentBookingSystem.Persistence.Models.Item.FromDomain(item);
        _context.Add(itemEntity);
        await _context.SaveChangesAsync();

        // get the new item entity
        var newItemEntity = await GetById(item.Id) ?? throw new ApplicationException("Item not found after creation.");

        // return the new item entity
        return newItemEntity;
    }

    public async Task<Item?> GetById(ItemId? itemId)
    {
        if (itemId is null)
        {
            return null;
        }

        var itemEntity = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .FirstOrDefaultAsync(m => m.Id == itemId.Value.Value);

        if (itemEntity is null)
        {
            return null;
        }

        var domainItem = itemEntity.ToDomain();

        return domainItem;
    }

    public async Task Delete(ItemId itemId)
    {
        var itemEntity = await _context.Item
            .FirstOrDefaultAsync(m => m.Id == itemId.Value);

        if (itemEntity is not null)
        {
            // TODO: Soft delete
            _context.Item.Remove(itemEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<RecordChangeEntry>> ChangesForItem(ItemId itemId)
    {
        var changeEntities = await _context.Audits
            .Where(a => a.EntityId == itemId.Value)
            .Where(a => !a.PropertyName.Equals("CreatedDate"))
            .Where(a => !a.PropertyName.Equals("UpdatedDate"))
            .Where(a => !a.PropertyName.Equals("CreatedBy"))
            .Where(a => !a.PropertyName.Equals("UpdatedBy"))
            .OrderByDescending(a => a.ChangeTimeUtc)
            .ToListAsync();

        var recordChangeEntries = changeEntities
            .Select(a => new RecordChangeEntry()
            {
                Id = a.Id,
                UserEmail = a.UserEmail,
                ChangeTimeUtc = a.ChangeTimeUtc,
                EntityId = a.EntityId,
                TableName = a.TableName,
                PropertyName = a.PropertyName,
                PropertyType = a.PropertyType,
                OldValue = a.OldValue,
                NewValue = a.NewValue,
            })
            .ToList();

        return recordChangeEntries;
    }

    public async Task Update(ItemId itemId, User currentUser, Item item)
    {
        // get the item entity
        var itemEntity = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .FirstOrDefaultAsync(m => m.Id == itemId.Value);

        if (itemEntity is null)
        {
            throw new Exception("Item not found");
        }

        // update the item entity
        itemEntity!.Manufacturer = item.Manufacturer;
        itemEntity.Model = item.Model;
        itemEntity.DamageNotes = item.DamageNotes;
        itemEntity.Notes = item.Notes;
        itemEntity.Identifiers = item.Identifiers
            .Select(EquipmentBookingSystem.Persistence.Models.ItemIdentifier.FromDomain)
            .ToHashSet();

        // save the changes
        await _context.SaveChangesAsync();

    }

    public async Task<ItemIdentifier> CreateIdentifier(ItemIdentifier itemIdentifier)
    {
        if (itemIdentifier.Id is null)
        {
            throw new Exception("Item identifier must have an ID");
        }

        // insert new item identifier entity
        var itemIdentifierEntity = EquipmentBookingSystem.Persistence.Models.ItemIdentifier.FromDomain(itemIdentifier);
        _context.Add(itemIdentifierEntity);
        await _context.SaveChangesAsync();

        // get the new item identifier entity
        var newItemIdentifierEntity = await _context.ItemIdentifiers
            .FirstOrDefaultAsync(m => m.Id == itemIdentifier.Id.Value.Value);

        // return the new item identifier entity
        return newItemIdentifierEntity!.ToDomain();
    }

    public async Task<IList<Item>> GetVisibleItems(User currentUser)
    {
        var itemEntities = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .ToListAsync();

        return itemEntities
            .Select(item => item.ToDomain())
            .ToList();
    }

}
