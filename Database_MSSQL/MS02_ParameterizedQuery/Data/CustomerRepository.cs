using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using MS02_ParameterizedQuery.Models;

namespace MS02_ParameterizedQuery.Data
{
    /// <summary>
    /// 客戶資料存取類別 - 展示參數化查詢
    /// </summary>
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// 根據城市搜尋客戶（使用 Dapper）
        /// </summary>
        public List<Customer> SearchByCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return new List<Customer>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // ✅ 正確：使用參數化查詢
                    string sql = "SELECT * FROM Customers WHERE City = @City";

                    // Dapper 自動處理參數化
                    var customers = connection.Query<Customer>(sql, new { City = city }).ToList();

                    return customers;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"搜尋客戶失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 根據姓名模糊搜尋（使用 LIKE）
        /// </summary>
        public List<Customer> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Customer>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // ✅ 正確：LIKE 查詢也要使用參數化
                    string sql = "SELECT * FROM Customers WHERE Name LIKE @SearchPattern";

                    // 在程式碼中組合 LIKE 模式，而不是在 SQL 中
                    var searchPattern = $"%{name}%";

                    var customers = connection.Query<Customer>(sql, new { SearchPattern = searchPattern }).ToList();

                    return customers;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"搜尋客戶失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 根據 Email 查詢單一客戶
        /// </summary>
        public Customer GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT * FROM Customers WHERE Email = @Email";

                    // QueryFirstOrDefault: 返回第一筆或 null
                    var customer = connection.QueryFirstOrDefault<Customer>(sql, new { Email = email });

                    return customer;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"查詢客戶失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 多條件搜尋（展示多個參數）
        /// </summary>
        public List<Customer> SearchByCriteria(string city, string namePattern, DateTime? createdAfter)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // 動態建立 SQL（但仍使用參數化）
                    var conditions = new List<string>();
                    var parameters = new DynamicParameters();

                    if (!string.IsNullOrWhiteSpace(city))
                    {
                        conditions.Add("City = @City");
                        parameters.Add("City", city);
                    }

                    if (!string.IsNullOrWhiteSpace(namePattern))
                    {
                        conditions.Add("Name LIKE @NamePattern");
                        parameters.Add("NamePattern", $"%{namePattern}%");
                    }

                    if (createdAfter.HasValue)
                    {
                        conditions.Add("CreatedAt >= @CreatedAfter");
                        parameters.Add("CreatedAfter", createdAfter.Value);
                    }

                    // 如果沒有任何條件，返回空列表
                    if (conditions.Count == 0)
                        return new List<Customer>();

                    // 組合 SQL
                    string sql = $"SELECT * FROM Customers WHERE {string.Join(" AND ", conditions)}";

                    var customers = connection.Query<Customer>(sql, parameters).ToList();

                    return customers;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"搜尋客戶失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 根據 ID 列表查詢（IN 子句）
        /// </summary>
        public List<Customer> GetByIds(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return new List<Customer>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // ✅ 正確：Dapper 支援陣列參數
                    string sql = "SELECT * FROM Customers WHERE Id IN @Ids";

                    var customers = connection.Query<Customer>(sql, new { Ids = ids }).ToList();

                    return customers;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"查詢客戶失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 檢查 Email 是否已存在
        /// </summary>
        public bool EmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT COUNT(*) FROM Customers WHERE Email = @Email";

                    // ExecuteScalar: 返回單一值
                    int count = connection.ExecuteScalar<int>(sql, new { Email = email });

                    return count > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"檢查 Email 失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 取得所有城市列表（不重複）
        /// </summary>
        public List<string> GetAllCities()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT DISTINCT City FROM Customers WHERE City IS NOT NULL ORDER BY City";

                    var cities = connection.Query<string>(sql).ToList();

                    return cities;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"查詢城市列表失敗: {ex.Message}", ex);
            }
        }

        #region 錯誤示範（僅供教學，永遠不要這樣做！）

        /// <summary>
        /// ❌ 錯誤示範：SQL Injection 風險
        /// 這個方法僅供教學，展示不安全的做法
        /// 永遠不要在實際專案中這樣做！
        /// </summary>
        [Obsolete("此方法不安全，僅供教學示範 SQL Injection 風險", true)]
        public List<Customer> UnsafeSearchByCity(string city)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // ❌ 危險：直接拼接 SQL，可能被 SQL Injection 攻擊
                string sql = $"SELECT * FROM Customers WHERE City = '{city}'";

                // 如果 city = "台北市' OR '1'='1"
                // 實際執行: SELECT * FROM Customers WHERE City = '台北市' OR '1'='1'
                // 結果: 返回所有客戶！

                var customers = connection.Query<Customer>(sql).ToList();
                return customers;
            }
        }

        #endregion
    }
}
