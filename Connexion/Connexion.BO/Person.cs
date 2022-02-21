using System;
using System.Collections.Generic;

namespace Connexion.BO
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public byte[] Photo { get; set; }

        public Person()
        {

        }

        public Person(int id, string name, long phoneNumber, DateTime birthDay, byte[] photo)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
            BirthDay = birthDay;
            Photo = photo;
        }

        public override bool Equals(object obj)
        {
            return obj is Person person &&
                   Id == person.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        public static bool operator ==(Person left, Person right)
        {
            return EqualityComparer<Person>.Default.Equals(left, right);
        }

        public static bool operator !=(Person left, Person right)
        {
            return !(left == right);
        }
    }
}
