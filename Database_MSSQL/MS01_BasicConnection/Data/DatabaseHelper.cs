using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using MS01_BasicConnection.Models;

namespace MS01_BasicConnection.Data
{
    /// <summary>
    /// 資料庫輔助類別 - 處理基本連線和查詢
    /// </summary>
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// 測試資料庫連線
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                // 使用 using 語句確保連線被正確釋放
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"連線失敗: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 取得資料庫版本
        /// </summary>
        public string GetServerVersion()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return connection.ServerVersion;
                }
            }
            catch (SqlException ex)
            {
                return $"錯誤: {ex.Message}";
            }
        }

        /// <summary>
        /// 初始化資料庫（建立資料表）
        /// </summary>
        public void InitializeDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // 建立 Customers 資料表
                    string createTableSql = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
                        BEGIN
                            CREATE TABLE Customers (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                Name NVARCHAR(100) NOT NULL,
                                Email NVARCHAR(100) NOT NULL UNIQUE,
                                Phone NVARCHAR(20),
                                City NVARCHAR(50),
                                CreatedAt DATETIME2 DEFAULT GETDATE()
                            );
                        END
                    ";

                    using (var command = new SqlCommand(createTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // 插入範例資料（如果資料表是空的）
                    string checkDataSql = "SELECT COUNT(*) FROM Customers";
                    using (var command = new SqlCommand(checkDataSql, connection))
                    {
                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                        {
                            InsertSampleData(connection);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"初始化資料庫失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 插入範例資料
        /// </summary>
        private void InsertSampleData(SqlConnection connection)
        {
            string insertSql = @"
                INSERT INTO Customers (Name, Email, Phone, City) VALUES
                ('王小明', 'wang@example.com', '0912-345-678', '台北市'),
                ('李美美', 'li@example.com', '0923-456-789', '新北市'),
                ('張大力', 'zhang@example.com', '0934-567-890', '台中市'),
                ('陳小華', 'chen@example.com', '0945-678-901', '高雄市'),
                ('林雅婷', 'lin@example.com', '0956-789-012', '台南市')
            ";

            using (var command = new SqlCommand(insertSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 取得所有客戶（使用 ADO.NET）
        /// </summary>
        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // SQL 查詢語句
                    string sql = "SELECT Id, Name, Email, Phone, City, CreatedAt FROM Customers ORDER BY Id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        // 使用 DataReader 讀取結果
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var customer = new Customer
                                {
                                    Id = reader.GetInt32(0),                    // Id
                                    Name = reader.GetString(1),                 // Name
                                    Email = reader.GetString(2),                // Email
                                    Phone = reader.IsDBNull(3) ? null : reader.GetString(3),   // Phone
                                    City = reader.IsDBNull(4) ? null : reader.GetString(4),    // City
                                    CreatedAt = reader.GetDateTime(5)           // CreatedAt
                                };

                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"查詢客戶資料失敗: {ex.Message}", ex);
            }

            return customers;
        }

        /// <summary>
        /// 取得客戶數量
        /// </summary>
        public int GetCustomerCount()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sql = "SELECT COUNT(*) FROM Customers";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        // ExecuteScalar 用於返回單一值
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"查詢客戶數量失敗: {ex.Message}", ex);
            }
        }
    }
}
