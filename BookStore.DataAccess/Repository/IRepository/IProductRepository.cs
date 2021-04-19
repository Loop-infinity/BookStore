using Bookstore.DataAccess.Repository.IRepository;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IProductRepository: IRepository<Product>
    {
        public void Update(Product product);
    }
}
