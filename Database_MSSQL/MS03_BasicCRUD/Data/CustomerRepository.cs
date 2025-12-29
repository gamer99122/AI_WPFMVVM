using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using MS03_BasicCRUD.Models;

namespace MS03_BasicCRUD.Data
{
    /// <summary>
    /// 客戶資料存取類別 - 完整 CRUD 操作
    /// </summary>
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        #region Create (新增)

        /// <summary>
        /// 新增客戶
        /// </summary>
        /// <returns>新增的客戶 ID</returns>
        public int Add(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            // 驗證必填欄位
            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("客戶姓名不能為空");

            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Email 不能為空");

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // 使用 SCOPE_IDENTITY() 取得新增的 ID
                    string sql = @"
                        INSERT INTO Customers (Name, Email, Phone, City)
                        VALUES (@Name, @Email, @Phone, @City);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);
                    ";

                    // ExecuteScalar 返回第一行第一列的值（新增的 ID）
                    int newId = connection.ExecuteScalar<int>(sql, customer);

                    return newId;
                }
            }
            catch (SqlException ex)
            {
                // 檢查是否為 Email 重複錯誤
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new Exception("Email 已被使用", ex);
                }

                throw new Exception($"新增客戶失敗: {ex.Message}", ex);
            }
        }

        #endregion

        #region Read (查詢)

        /// <summary>
        /// 取得所有客戶
        /// </summary>
        public List<Customer> GetAll()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT * FROM Customers ORDER BY Id DESC";

                    var customers = connection.Query<Customer>(sql).ToList();

                    return customers;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"查詢客戶失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 根據 ID 取得客戶
        /// </summary>
        public Customer GetById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT * FROM Customers WHERE Id = @Id";

                    var customer = connection.QueryFirstOrDefault<Customer>(sql, new { Id = id });

                    return customer;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"查詢客戶失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 檢查 Email 是否已存在（排除特定 ID）
        /// </summary>
        public bool EmailExists(string email, int? excludeId = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql;
                    object parameters;

                    if (excludeId.HasValue)
                    {
                        // 更新時排除自己
                        sql = "SELECT COUNT(*) FROM Customers WHERE Email = @Email AND Id != @ExcludeId";
                        parameters = new { Email = email, ExcludeId = excludeId.Value };
                    }
                    else
                    {
                        // 新增時
                        sql = "SELECT COUNT(*) FROM Customers WHERE Email = @Email";
                        parameters = new { Email = email };
                    }

                    int count = connection.ExecuteScalar<int>(sql, parameters);

                    return count > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"檢查 Email 失敗: {ex.Message}", ex);
            }
        }

        #endregion

        #region Update (更新)

        /// <summary>
        /// 更新客戶
        /// </summary>
        /// <returns>是否更新成功</returns>
        public bool Update(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            // 驗證必填欄位
            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("客戶姓名不能為空");

            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Email 不能為空");

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = @"
                        UPDATE Customers
                        SET Name = @Name,
                            Email = @Email,
                            Phone = @Phone,
                            City = @City
                        WHERE Id = @Id
                    ";

                    // Execute 返回受影響的資料列數
                    int rowsAffected = connection.Execute(sql, customer);

                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                // 檢查是否為 Email 重複錯誤
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new Exception("Email 已被其他客戶使用", ex);
                }

                throw new Exception($"更新客戶失敗: {ex.Message}", ex);
            }
        }

        #endregion

        #region Delete (刪除)

        /// <summary>
        /// 刪除客戶
        /// </summary>
        /// <returns>是否刪除成功</returns>
        public bool Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = "DELETE FROM Customers WHERE Id = @Id";

                    int rowsAffected = connection.Execute(sql, new { Id = id });

                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                // 檢查是否有外鍵約束錯誤
                if (ex.Number == 547)
                {
                    throw new Exception("無法刪除：此客戶有關聯的訂單資料", ex);
                }

                throw new Exception($"刪除客戶失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 批次刪除客戶
        /// </summary>
        public int DeleteMultiple(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return 0;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = "DELETE FROM Customers WHERE Id IN @Ids";

                    int rowsAffected = connection.Execute(sql, new { Ids = ids });

                    return rowsAffected;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"批次刪除客戶失敗: {ex.Message}", ex);
            }
        }

        #endregion

        #region 統計查詢

        /// <summary>
        /// 取得客戶總數
        /// </summary>
        public int GetCount()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT COUNT(*) FROM Customers";

                    return connection.ExecuteScalar<int>(sql);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"查詢客戶總數失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 根據城市統計客戶數量
        /// </summary>
        public Dictionary<string, int> GetCountByCity()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = @"
                        SELECT City, COUNT(*) AS Count
                        FROM Customers
                        WHERE City IS NOT NULL
                        GROUP BY City
                        ORDER BY Count DESC
                    ";

                    var result = connection.Query<(string City, int Count)>(sql)
                        .ToDictionary(x => x.City, x => x.Count);

                    return result;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"統計客戶數量失敗: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
