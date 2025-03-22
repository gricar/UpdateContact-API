namespace UpdateContact.Application.Persistence;

public interface IContactRepository
{
    Task<Domain.Entities.Contact?> GetAsync(Guid id);
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> IsDddAndPhoneUniqueAsync(int ddd, string phone);
}
