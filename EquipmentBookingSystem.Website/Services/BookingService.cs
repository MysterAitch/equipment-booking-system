using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using EquipmentBookingSystem.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace EquipmentBookingSystem.Website.Services;

public class BookingService : IBookingService
{
    private readonly WebsiteDbContext _context;

    public BookingService(WebsiteDbContext context)
    {
        _context = context;
    }


    public async Task<Booking> CreateNew(User user, Booking booking)
    {
        // insert new booking entity
        var bookingEntity = EquipmentBookingSystem.Persistence.Models.Booking.FromDomain(booking);
        _context.Add(bookingEntity);
        await _context.SaveChangesAsync();

        // get the new booking entity
        var newBookingEntity = await GetById(booking.Id);

        // return the new booking entity
        return newBookingEntity!;
    }

    public async Task Update(BookingId bookingId, User user, Booking booking)
    {
        var bookingEntity = await _context.Booking
            .Include(b => b.Items)
            .FirstOrDefaultAsync(m => m.Id == bookingId.Value);

        if (bookingEntity is not null)
        {
            bookingEntity.BookingStart = booking.BookingStart;
            bookingEntity.BookingEnd = booking.BookingEnd;
            bookingEntity.EventStart = booking.EventStart;
            bookingEntity.EventEnd = booking.EventEnd;
            bookingEntity.BookedBy = booking.BookedBy?.Email ?? string.Empty;
            bookingEntity.BookedFor = booking.BookedFor ?? string.Empty;
            bookingEntity.SjaEventDipsId = booking.SjaEventDipsId;
            bookingEntity.SjaEventName = booking.SjaEventName;
            bookingEntity.SjaEventType = booking.SjaEventType;
            bookingEntity.Notes = booking.Notes ?? string.Empty;

            _context.Update(bookingEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(BookingId bookingId)
    {
        var bookingEntity = await _context.Booking
            .FirstOrDefaultAsync(m => m.Id == bookingId.Value);

        if (bookingEntity is not null)
        {
            _context.Booking.Remove(bookingEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Booking?> GetById(BookingId? bookingId)
    {
        if (bookingId is null)
        {
            return null;
        }

        var bookingEntity = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .FirstOrDefaultAsync(m => m.Id == bookingId.Value.Value);

        if (bookingEntity is null)
        {
            return null;
        }

        var domainBooking = bookingEntity.ToDomain();

        return domainBooking;
    }

    public async Task<IEnumerable<RecordChangeEntry>> ChangesForBooking(BookingId bookingId)
    {
        var changeEntities = await _context.Audits
            .Where(a => a.EntityId == bookingId.Value)
            .Where(a => !a.PropertyName.Equals("CreatedDate"))
            .Where(a => !a.PropertyName.Equals("UpdatedDate"))
            .Where(a => !a.PropertyName.Equals("CreatedBy"))
            .Where(a => !a.PropertyName.Equals("UpdatedBy"))
            .OrderByDescending(a => a.ChangeTimeUtc)
            .ToListAsync();

        var recordChangeEntries = changeEntities
            .Select(a => a.ToDomain());

        return recordChangeEntries;
    }

    public async Task<IEnumerable<Item>> ItemsPotentiallyAvailableForBooking(User user, BookingId bookingId)
    {
        var itemEntities = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .ToListAsync();

        var domainItems = itemEntities
            .Select(booking => booking.ToDomain());

        return domainItems;
    }

    public async Task<IEnumerable<Booking>> GetVisibleBookings(User user)
    {
        var bookingEntities = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .ToListAsync();

        var domainBookings = bookingEntities
            .Select(booking => booking.ToDomain());

        return domainBookings;
    }

    public async Task<IEnumerable<Booking>> BookingsForItem(User? user, ItemId itemId)
    {
        var bookingEntities = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .Where(b => b.Items.Any(i => i.Id == itemId.Value))
            .ToListAsync();

        var domainBookings = bookingEntities
            .Select(booking => booking.ToDomain());

        return domainBookings;
    }

    public async Task<IEnumerable<Booking>> BookingsForItems(User user, List<ItemId> itemIds)
    {
        var itemIdValues = itemIds.Select(id => id.Value).ToList();
        var bookingEntities = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .Where(b => b.Items.Any(i => itemIdValues.Contains(i.Id)))
            .ToListAsync();

        var domainBookings = bookingEntities
            .Select(booking => booking.ToDomain());

        return domainBookings;
    }

    public async Task UpdateItems(BookingId bookingId, List<Guid> selectedItemIds)
    {
        var bookingEntity = _context.Booking
            .Include(b => b.Items)
            .FirstOrDefault(m => m.Id == bookingId.Value);

        if (bookingEntity is null)
        {
            throw new InvalidOperationException("Booking not found");
        }

        var availableItems = await _context.Item
            // .Where()
            .ToListAsync();

        var availableItemIds = availableItems.Select(x => x.Id).ToHashSet();
        if(selectedItemIds.Any(guid => !availableItemIds.Contains(guid)))
        {
            throw new InvalidOperationException("Item not found, not able to attache to booking");
        }

        var selectedItems = availableItems
            .Where(i => selectedItemIds.Contains(i.Id))
            .ToList();

        bookingEntity.Items.Clear();
        bookingEntity.Items.AddRange(selectedItems);

        _context.Update(bookingEntity);
        foreach (var bookingEntityItem in bookingEntity.Items)
        {
            _context.Update(bookingEntityItem);
        }

        await _context.SaveChangesAsync();
    }
}
