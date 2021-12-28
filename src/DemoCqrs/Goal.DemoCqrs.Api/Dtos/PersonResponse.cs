using System;
using Goal.DemoCqrs.Domain.Aggregates.People;

namespace Goal.DemoCqrs.Api.Dtos
{
    public class PersonResponse
    {
        public string PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cpf { get; set; }

        public static explicit operator PersonResponse(Person person)
        {
            if (person == null)
                return null;

            return new PersonResponse
            {
                PersonId = person.Id,
                FirstName = person.Name.FirstName,
                LastName = person.Name.LastName,
                Cpf = person.Cpf?.Number
            };
        }
    }
}