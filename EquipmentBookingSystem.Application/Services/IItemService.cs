using System.Collections.Generic;
using System.Threading.Tasks;
using EquipmentBookingSystem.Domain.Models;

namespace EquipmentBookingSystem.Application.Services;

public interface IItemService
{
    Task<Item> CreateNew(User currentUser, Item item);

    /*
     *
        var item = await _context.Item
            .FirstOrDefaultAsync(m => m.Id == id);

        var item = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .FirstOrDefaultAsync(m => m.Id == id);
     */
    Task<Item?> GetById(ItemId? itemId);

    Task Delete(ItemId itemId);

    /*
     Changes = await _context.Audits
            .Where(a => a.EntityId == id)
            .Where(a => !a.PropertyName.Equals("CreatedDate"))
            .Where(a => !a.PropertyName.Equals("UpdatedDate"))
            .Where(a => !a.PropertyName.Equals("CreatedBy"))
            .Where(a => !a.PropertyName.Equals("UpdatedBy"))
            .OrderByDescending(a => a.ChangeTimeUtc)
            .ToListAsync();
    */
    Task<IEnumerable<RecordChangeEntry>> ChangesForItem(ItemId itemId);


    Task Update(ItemId itemId, User currentUser, Item item);

    Task<ItemIdentifier> CreateIdentifier(ItemIdentifier itemIdentifier);


    /*
        Item = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .ToListAsync();
    */
    Task<IList<Item>> GetVisibleItems(User currentUser);
}
