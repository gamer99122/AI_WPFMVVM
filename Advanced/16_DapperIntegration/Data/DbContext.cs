using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace DapperIntegrationExample.Data
{
    /// <summary>
    /// 資料庫上下文類別
    /// 管理資料庫連線
    /// </summary>
    public class AppDbContext : IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// 取得資料庫連線
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqliteConnection(_connectionString);
                }

                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                return _connection;
            }
        }

        /// <summary>
        /// 初始化資料庫架構
        /// </summary>
        public void Initialize()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS Customers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    Phone TEXT,
                    Address TEXT,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT
                );

                CREATE TABLE IF NOT EXISTS Orders (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerId INTEGER NOT NULL,
                    OrderDate TEXT NOT NULL,
                    TotalAmount REAL NOT NULL,
                    Status TEXT NOT NULL,
                    FOREIGN KEY (CustomerId) REFERENCES Customers (Id)
                );

                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Category TEXT,
                    Price REAL NOT NULL,
                    Stock INTEGER NOT NULL DEFAULT 0,
                    CreatedAt TEXT NOT NULL
                );
            ";
            createTableCommand.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
