using Bogus;
using UpdateContact.Domain.Entities;

namespace UpdateContact.Domain.UnitTests.Fixtures;

public class ContactFixture
{
    private readonly Faker _faker;

    public ContactFixture()
    {
        _faker = new Faker();
    }

    public Contact CreateValidContact()
    {
        return Contact.Create(
            _faker.Person.FirstName,
            _faker.PickRandom(new[] { 11, 21, 31, 41 }),
            _faker.Phone.PhoneNumber("#########"),
            _faker.Person.Email);
    }
}
