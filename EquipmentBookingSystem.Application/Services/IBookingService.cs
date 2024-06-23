using System.Collections.Generic;
using System.Threading.Tasks;
using EquipmentBookingSystem.Domain.Models;

namespace EquipmentBookingSystem.Application.Services;

public interface IBookingService
{
    Task<Booking> CreateNew(User user, Booking booking);

    Task Update(Booking.BookingId bookingId, User user, Booking booking);

    Task Delete(Booking.BookingId bookingId);

    // var booking = await _context.Booking
    //     .Include(b => b.Items)
    //     .ThenInclude(i => i.Identifiers)
    //     .FirstOrDefaultAsync(m => m.Id == id);
    Task<Booking?> GetById(Booking.BookingId bookingId);

    // Task<IEnumerable<Booking>> GetAll();

    /*
     * Changes = await _context.Audits
            .Where(a => a.EntityId == id)
            .Where(a => !a.PropertyName.Equals("CreatedDate"))
            .Where(a => !a.PropertyName.Equals("UpdatedDate"))
            .Where(a => !a.PropertyName.Equals("CreatedBy"))
            .Where(a => !a.PropertyName.Equals("UpdatedBy"))
            .OrderByDescending(a => a.ChangeTimeUtc)
            .ToListAsync();
     *
     */
    Task<IEnumerable<RecordChangeEntry>> ChangesForBooking(Booking.BookingId bookingId);

    /*
     *  Items = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .ToListAsync();
     */
    Task<IEnumerable<Item>> ItemsPotentiallyAvailableForBooking(Booking.BookingId bookingId);

    /*
        Bookings = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .ToListAsync();
    */
    Task<IEnumerable<Booking>> GetVisibleBookings(User user);

    Task<IEnumerable<Booking>> BookingsForItem(User? user, Item.ItemId itemId);
    Task<IEnumerable<Booking>> BookingsForItems(User currentUser, List<Item.ItemId> itemIds);
}
