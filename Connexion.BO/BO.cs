using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connexion.BO
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Phone { get; set; }
        public DateTime Birthday { get; set; }
        public byte[] Photo { get; set; }


        public Person()
        {
        }

        public Person(int id, string name, long phone, DateTime birthday, byte[] photo)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Birthday = birthday;
            Photo = photo;


        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Person);
        }

        public bool Equals(Person other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
    }
