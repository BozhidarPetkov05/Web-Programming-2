using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Common.Entities;

namespace Common.Services;

public class UserServices
{
    private static List<User> Items { get; set; }

    static UserServices()
    {
        Items = new List<User>()
        {
            new User()
            {
                Id = 1,
                Username = "gosho123",
                Password = "123456",
                FirstName = "Georgi",
                LastName = "Goshov"
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
                throw new Exception("Item not found");

            forUpdate.Username = item.Username;
            forUpdate.Password = item.Password;
            forUpdate.FirstName = item.FirstName;
            forUpdate.LastName = item.LastName;
        }
        else
        {
            item.Id = Items.Count <= 0
                                ? 1
                                : Items.Max(i => i.Id) + 1;
            Items.Add(item);
        }
    }

    public void Delete(User item)
    {
        User forDelete = Items.FirstOrDefault(i => i.Id == item.Id);
        if (forDelete == null)
            throw new Exception("Item not found");

        Items.Remove(forDelete);
    }
}
