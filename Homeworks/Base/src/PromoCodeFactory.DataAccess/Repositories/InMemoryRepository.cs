using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected static List<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data.ToList();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data.AsEnumerable());
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task CreateAsync(T item)
        {
            Data.Add(item);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T item)
        {
            var existingItem = Data.FirstOrDefault(x => x.Id == item.Id);
            if (existingItem != null)
            {
                var index = Data.IndexOf(existingItem);
                Data[index] = item;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var itemToRemove = Data.FirstOrDefault(x => x.Id == id);
            if (itemToRemove != null)
            {
                Data.Remove(itemToRemove);
            }
            return Task.CompletedTask;
        }
    }
}