using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.Data.SqlClient;
using Dapper;
using MS04_Transaction.Models;

namespace MS04_Transaction.Data
{
    /// <summary>
    /// 訂單資料存取類別 - 展示事務處理
    /// </summary>
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// 建立訂單（使用 TransactionScope）
        /// 確保訂單主檔、明細和庫存更新同時成功或失敗
        /// </summary>
        public int CreateOrder(Order order, List<OrderItem> items)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (items == null || items.Count == 0)
                throw new ArgumentException("訂單明細不能為空");

            try
            {
                int orderId = 0;

                // 使用 TransactionScope 管理分散式事務
                using (var scope = new TransactionScope())
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        // 步驟 1: 插入訂單主檔
                        string insertOrderSql = @"
                            INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, Status)
                            VALUES (@CustomerId, @OrderDate, @TotalAmount, @Status);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                        ";

                        orderId = connection.ExecuteScalar<int>(insertOrderSql, order);

                        // 步驟 2: 插入訂單明細
                        foreach (var item in items)
                        {
                            item.OrderId = orderId;

                            string insertItemSql = @"
                                INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice, Subtotal)
                                VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice, @Subtotal);
                            ";

                            connection.Execute(insertItemSql, item);

                            // 步驟 3: 更新庫存
                            string updateStockSql = @"
                                UPDATE Products
                                SET Stock = Stock - @Quantity
                                WHERE Id = @ProductId AND Stock >= @Quantity;
                            ";

                            int rowsAffected = connection.Execute(updateStockSql, new
                            {
                                item.ProductId,
                                item.Quantity
                            });

                            // 檢查庫存是否足夠
                            if (rowsAffected == 0)
                            {
                                throw new InvalidOperationException($"產品 ID {item.ProductId} 庫存不足");
                            }
                        }
                    }

                    // 所有操作成功，提交事務
                    scope.Complete();
                }

                return orderId;
            }
            catch (Exception ex)
            {
                // 發生錯誤，事務自動回滾
                throw new Exception($"建立訂單失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 取消訂單（使用 SqlTransaction）
        /// 更新訂單狀態並恢復庫存
        /// </summary>
        public bool CancelOrder(int orderId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // 開始事務
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 步驟 1: 檢查訂單狀態
                            string checkOrderSql = "SELECT Status FROM Orders WHERE Id = @OrderId";

                            string status = connection.ExecuteScalar<string>(
                                checkOrderSql,
                                new { OrderId = orderId },
                                transaction
                            );

                            if (status == null)
                            {
                                throw new Exception("訂單不存在");
                            }

                            if (status == "Cancelled")
                            {
                                throw new Exception("訂單已取消");
                            }

                            if (status == "Shipped" || status == "Delivered")
                            {
                                throw new Exception("訂單已出貨，無法取消");
                            }

                            // 步驟 2: 取得訂單明細
                            string getItemsSql = "SELECT * FROM OrderItems WHERE OrderId = @OrderId";

                            var items = connection.Query<OrderItem>(
                                getItemsSql,
                                new { OrderId = orderId },
                                transaction
                            );

                            // 步驟 3: 恢復庫存
                            foreach (var item in items)
                            {
                                string updateStockSql = @"
                                    UPDATE Products
                                    SET Stock = Stock + @Quantity
                                    WHERE Id = @ProductId;
                                ";

                                connection.Execute(
                                    updateStockSql,
                                    new { item.ProductId, item.Quantity },
                                    transaction
                                );
                            }

                            // 步驟 4: 更新訂單狀態
                            string updateOrderSql = @"
                                UPDATE Orders
                                SET Status = 'Cancelled'
                                WHERE Id = @OrderId;
                            ";

                            connection.Execute(
                                updateOrderSql,
                                new { OrderId = orderId },
                                transaction
                            );

                            // 提交事務
                            transaction.Commit();

                            return true;
                        }
                        catch
                        {
                            // 發生錯誤，回滾事務
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"取消訂單失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 轉移客戶的所有訂單到另一個客戶
        /// 展示批次更新的事務處理
        /// </summary>
        public int TransferOrders(int fromCustomerId, int toCustomerId)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        // 檢查目標客戶是否存在
                        string checkCustomerSql = "SELECT COUNT(*) FROM Customers WHERE Id = @CustomerId";

                        int count = connection.ExecuteScalar<int>(
                            checkCustomerSql,
                            new { CustomerId = toCustomerId }
                        );

                        if (count == 0)
                        {
                            throw new Exception("目標客戶不存在");
                        }

                        // 轉移訂單
                        string transferSql = @"
                            UPDATE Orders
                            SET CustomerId = @ToCustomerId
                            WHERE CustomerId = @FromCustomerId;
                        ";

                        int rowsAffected = connection.Execute(transferSql, new
                        {
                            FromCustomerId = fromCustomerId,
                            ToCustomerId = toCustomerId
                        });

                        scope.Complete();

                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"轉移訂單失敗: {ex.Message}", ex);
            }
        }
    }
}
