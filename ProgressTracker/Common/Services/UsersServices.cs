using System;
using System.Collections.Generic;
using System.Linq;
using Common.Entities;

namespace Common.Services
{
    public class UsersServices
    {
        private static List<User> Items { get; set; }

        static UsersServices()
        {
            Items = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Username = "gosho",
                    Password = "1234",
                    FirstName = "Zele",
                    LastName = "Oriz"
                }
            };
        }

        public List<User> GetAll()
        {
            return Items;
        }

        public User GetById(int id)
        {
            return Items.FirstOrDefault(i => i.Id == id);
        }

        public void Save(User item)
        {
            if (item.Id > 0)
            {
                User forUpdate = Items.FirstOrDefault(i => i.Id == item.Id);

                if (forUpdate == null)
                {
                    throw new Exception("User not found!");
                }

                forUpdate.Id = item.Id;
                forUpdate.Username = item.Username;
                forUpdate.Password = item.Password;
                forUpdate.FirstName = item.FirstName;
                forUpdate.LastName = item.LastName;
            }
            else
            {
                if (Items.Count <= 0)
                {
                    item.Id = 1;
                }
                else
                {
                    item.Id = Items.Max(i => i.Id) + 1;
                }
                Items.Add(item);
            }
        }

        public void Delete(User item)
        {
            User forDelete = Items.FirstOrDefault(i => i.Id == item.Id);

            if (forDelete == null)
            {
                throw new Exception("User not found!");
            }

            Items.Remove(forDelete);
        }
    }
}
