﻿using Chat.Domain.Models;

namespace Chat.Domain.Interfaces
{
    public interface IShoppingRepository : IGenericRepository<ShoppingModel>
    {
        Task<ShoppingModel?> GetAsync(int id);
        Task<List<ShoppingModel>> GetUserShoppingsAsync(string id);
    }
}
