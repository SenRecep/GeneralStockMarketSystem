﻿using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace GeneralStockMarket.AuthAPI.ExtensionMethods
{
    public static class DbContextCustomExtensionMethods
    {
        public static async Task Clear<T>(this DbSet<T> dbSet) where T : class => await Task.Run(() => dbSet.RemoveRange(dbSet));
    }
}
