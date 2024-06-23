using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using EquipmentBookingSystem.Persistence.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task Update(Booking.BookingId bookingId, User user, Booking booking)
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

            // Remove items that are not in the updated booking
            // Note: Cannot remove and re-add, because entity framework will be tracking the objects
            // "InvalidOperationException: The instance of entity type 'Item' cannot be tracked because another instance with the key value '{Id: [...]}' is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached"
            var newIds = booking.Items.Select(item => item.Id?.Value ?? throw new NullReferenceException(nameof(item.Id))).ToHashSet();
            bookingEntity.Items.RemoveWhere(item => newIds.Contains(item.Id));


            // Add items that are in the updated booking but not in the current booking
            var currentIds = bookingEntity.Items.Select(item => item.Id).ToHashSet();
            foreach (var item in booking.Items.Where(item => item.Id is not null && !currentIds.Contains(item.Id.Value)))
            {
                bookingEntity.Items.Add(EquipmentBookingSystem.Persistence.Models.Item.FromDomain(item));
            }


            _context.Update(bookingEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(Booking.BookingId bookingId)
    {
        var bookingEntity = await _context.Booking
            .FirstOrDefaultAsync(m => m.Id == bookingId.Value);

        if (bookingEntity is not null)
        {
            _context.Booking.Remove(bookingEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Booking?> GetById(Booking.BookingId bookingId)
    {
        var bookingEntity = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .FirstOrDefaultAsync(m => m.Id == bookingId.Value);

        if (bookingEntity is null)
        {
            return null;
        }

        var domainBooking = bookingEntity.ToDomain();

        return domainBooking;
    }

    public async Task<IEnumerable<RecordChangeEntry>> ChangesForBooking(Booking.BookingId bookingId)
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

    public async Task<IEnumerable<Item>> ItemsPotentiallyAvailableForBooking(Booking.BookingId bookingId)
    {
        var itemEntities = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .ToListAsync();

        var domainItems = itemEntities
            .Select( booking => booking.ToDomain());

        return domainItems;
    }

    public async Task<IEnumerable<Booking>> GetVisibleBookings(User user)
    {
        var bookingEntities = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .ToListAsync();

        var domainBookings = bookingEntities
            .Select( booking => booking.ToDomain());

        return domainBookings;
    }

    public async Task<IEnumerable<Booking>> BookingsForItem(User? user, Item.ItemId itemId)
    {
        var bookingEntities = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .Where(b => b.Items.Any(i => i.Id == itemId.Value))
            .ToListAsync();

        var domainBookings = bookingEntities
            .Select( booking => booking.ToDomain());

        return domainBookings;
    }

    public async Task<IEnumerable<Booking>> BookingsForItems(User currentUser, List<Item.ItemId> itemIds)
    {
        var itemIdValues = itemIds.Select(id => id.Value).ToList();
        var bookingEntities = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .Where(b => b.Items.Any(i => itemIdValues.Contains(i.Id)))
            .ToListAsync();

        var domainBookings = bookingEntities
            .Select( booking => booking.ToDomain());

        return domainBookings;
    }
}
