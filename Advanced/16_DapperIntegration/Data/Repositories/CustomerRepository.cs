using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperIntegrationExample.Models;

namespace DapperIntegrationExample.Data.Repositories
{
    /// <summary>
    /// 客戶資料存取介面
    /// </summary>
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<int> AddAsync(Customer customer);
        Task<bool> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Customer>> SearchByNameAsync(string name);
    }

    /// <summary>
    /// 客戶資料存取實作
    /// 使用 Dapper 進行資料庫操作
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// 取得所有客戶
        /// </summary>
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var sql = "SELECT * FROM Customers ORDER BY CreatedAt DESC";
            return await _dbContext.Connection.QueryAsync<Customer>(sql);
        }

        /// <summary>
        /// 根據 ID 取得客戶
        /// </summary>
        public async Task<Customer> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Customers WHERE Id = @Id";
            return await _dbContext.Connection.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });
        }

        /// <summary>
        /// 新增客戶
        /// </summary>
        public async Task<int> AddAsync(Customer customer)
        {
            var sql = @"
                INSERT INTO Customers (Name, Email, Phone, Address, CreatedAt)
                VALUES (@Name, @Email, @Phone, @Address, @CreatedAt);
                SELECT last_insert_rowid();
            ";

            customer.CreatedAt = DateTime.Now;
            return await _dbContext.Connection.ExecuteScalarAsync<int>(sql, customer);
        }

        /// <summary>
        /// 更新客戶
        /// </summary>
        public async Task<bool> UpdateAsync(Customer customer)
        {
            var sql = @"
                UPDATE Customers
                SET Name = @Name,
                    Email = @Email,
                    Phone = @Phone,
                    Address = @Address,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id
            ";

            customer.UpdatedAt = DateTime.Now;
            var rowsAffected = await _dbContext.Connection.ExecuteAsync(sql, customer);
            return rowsAffected > 0;
        }

        /// <summary>
        /// 刪除客戶
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Customers WHERE Id = @Id";
            var rowsAffected = await _dbContext.Connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        /// <summary>
        /// 搜尋客戶（依姓名）
        /// </summary>
        public async Task<IEnumerable<Customer>> SearchByNameAsync(string name)
        {
            var sql = "SELECT * FROM Customers WHERE Name LIKE @Name";
            return await _dbContext.Connection.QueryAsync<Customer>(sql, new { Name = $"%{name}%" });
        }
    }
}
