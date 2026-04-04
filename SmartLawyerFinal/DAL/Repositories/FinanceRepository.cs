using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SmartLawyerFinal.DAL.Helpers;
using SmartLawyerFinal.Models;
using System.Collections.Generic;
using System.Data;

namespace SmartLawyerFinal.DAL.Repositories
{
    public class FinanceRepository
    {
        // ── Fees ──────────────────────────────────────

        public List<Fee> GetAllFees()
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
        SELECT 
            f.*,
            c.CaseNumber,
            c.Title AS CaseTitle,
            cl.FullName AS ClientName,
            ISNULL(p.PaidAmount, 0) AS TotalPaid
        FROM Finance.Fees f
        INNER JOIN Legal.Cases c   ON f.CaseId   = c.Id
        INNER JOIN Legal.Clients cl ON f.ClientId = cl.Id
        LEFT JOIN (
            SELECT FeeId, SUM(Amount) AS PaidAmount
            FROM Finance.ActualPayments
            GROUP BY FeeId
        ) p ON p.FeeId = f.Id
        ORDER BY f.CreatedAt DESC";
            return conn.Query<Fee>(sql).AsList();
        }

        public Fee GetFeeById(int id)
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
        SELECT 
            f.*,
            c.CaseNumber,
            c.Title AS CaseTitle,
            cl.FullName AS ClientName,
            ISNULL(p.PaidAmount, 0) AS TotalPaid
        FROM Finance.Fees f
        INNER JOIN Legal.Cases c    ON f.CaseId   = c.Id
        INNER JOIN Legal.Clients cl ON f.ClientId = cl.Id
        LEFT JOIN (
            SELECT FeeId, SUM(Amount) AS PaidAmount
            FROM Finance.ActualPayments
            GROUP BY FeeId
        ) p ON p.FeeId = f.Id
        WHERE f.Id = @Id";
            return conn.QueryFirstOrDefault<Fee>(sql, new { Id = id });
        }

        public int AddFee(Fee fee)
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                INSERT INTO Finance.Fees 
                    (CaseId, ClientId, FeeType, TotalAmount, DueDate, Notes, CreatedAt, CreatedBy)
                VALUES 
                    (@CaseId, @ClientId, @FeeType, @TotalAmount, @DueDate, @Notes, GETDATE(), @CreatedBy);
                SELECT SCOPE_IDENTITY();";
            return conn.ExecuteScalar<int>(sql, fee);
        }

        public void UpdateFee(Fee fee)
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                UPDATE Finance.Fees SET
                    FeeType     = @FeeType,
                    TotalAmount = @TotalAmount,
                    DueDate     = @DueDate,
                    Notes       = @Notes
                WHERE Id = @Id";
            conn.Execute(sql, fee);
        }

        public void DeleteFee(int id)
        {
            using var conn = DbHelper.GetConnection();
            conn.Execute("DELETE FROM Finance.Fees WHERE Id = @Id", new { Id = id });
        }

        // ── Actual Payments ───────────────────────────

        public List<ActualPayment> GetPaymentsByFeeId(int feeId)
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                SELECT p.*, u.FullName as ReceivedByName
                FROM Finance.ActualPayments p
                INNER JOIN Core.Users u ON p.ReceivedBy = u.Id
                WHERE p.FeeId = @FeeId
                ORDER BY p.PaymentDate DESC";
            return conn.Query<ActualPayment>(sql, new { FeeId = feeId }).AsList();
        }

        public void AddPayment(ActualPayment payment)
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                INSERT INTO Finance.ActualPayments 
                    (FeeId, Amount, PaymentDate, Method, ReceiptNumber, ReceivedBy, Notes, CreatedAt)
                VALUES 
                    (@FeeId, @Amount, @PaymentDate, @Method, @ReceiptNumber, @ReceivedBy, @Notes, GETDATE())";
            conn.Execute(sql, payment);
        }

        public void DeletePayment(int id)
        {
            using var conn = DbHelper.GetConnection();
            conn.Execute("DELETE FROM Finance.ActualPayments WHERE Id = @Id", new { Id = id });
        }

        // ── Dashboard Stats ───────────────────────────

        public FinanceSummary GetFinanceSummary()
        {
            using var conn = DbHelper.GetConnection();

            string sql = @"
        WITH FeePayments AS (
            SELECT 
                f.Id,
                f.TotalAmount,
                f.DueDate,
                ISNULL(SUM(p.Amount), 0) AS PaidAmount
            FROM Finance.Fees f
            LEFT JOIN Finance.ActualPayments p ON p.FeeId = f.Id
            GROUP BY f.Id, f.TotalAmount, f.DueDate
        )
        SELECT
            SUM(TotalAmount)                                                    AS TotalAmount,
            SUM(PaidAmount)                                                     AS TotalPaid,
            COUNT(CASE WHEN PaidAmount >= TotalAmount THEN 1 END)               AS FullyPaid,
            COUNT(CASE WHEN DueDate < GETDATE() 
                       AND PaidAmount < TotalAmount THEN 1 END)                 AS Overdue
        FROM FeePayments";

            return conn.QueryFirstOrDefault<FinanceSummary>(sql) ?? new FinanceSummary();
        }

        public List<MonthlyRevenue> GetMonthlyRevenue()
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                SELECT 
                    DATENAME(MONTH, PaymentDate) as Month,
                    SUM(Amount) as Amount
                FROM Finance.ActualPayments
                WHERE YEAR(PaymentDate) = YEAR(GETDATE())
                GROUP BY MONTH(PaymentDate), DATENAME(MONTH, PaymentDate)
                ORDER BY MONTH(PaymentDate)";
            return conn.Query<MonthlyRevenue>(sql).AsList();
        }

        // ── Cases & Clients for ComboBox ──────────────

        public List<CaseCombo> GetCasesForCombo()
        {
            using var conn = DbHelper.GetConnection();
            return conn.Query<CaseCombo>(
                "SELECT Id, CaseNumber + ' - ' + Title as DisplayText, ClientId FROM Legal.Cases WHERE IsArchived = 0 ORDER BY CaseNumber"
            ).AsList();
        }
    }

    // ── DTOs ──────────────────────────────────────────
    public class FinanceSummary
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal Remaining => TotalAmount - TotalPaid;
        public int FullyPaid { get; set; }
        public int Overdue { get; set; }
    }

    public class MonthlyRevenue
    {
        public string Month { get; set; }
        public decimal Amount { get; set; }
    }

    public class CaseCombo
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public int ClientId { get; set; }
    }
}
