using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SmartLawyerFinal.DAL.Helpers;

namespace SmartLawyerFinal.DAL.Repositories
{
    public class ReportsRepository
    {
        // ── Overview Stats ────────────────────────────
        public ReportStats GetStats()
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                SELECT
                    (SELECT COUNT(*) FROM Legal.Cases)                          AS TotalCases,
                    (SELECT COUNT(*) FROM Legal.Clients WHERE IsActive = 1)     AS ActiveClients,
                    (SELECT COUNT(*) FROM Legal.Hearings 
                     WHERE MONTH(HearingDateTime) = MONTH(GETDATE())
                     AND YEAR(HearingDateTime) = YEAR(GETDATE()))               AS HearingsThisMonth,
                    (SELECT 
                        CASE WHEN COUNT(*) = 0 THEN 0
                        ELSE CAST(COUNT(CASE WHEN s.StatusName IN (N'كسبنا') 
                                   THEN 1 END) * 100.0 / COUNT(*) AS INT)
                        END
                     FROM Legal.Cases c
                     INNER JOIN Lookup.CaseStatuses s ON c.StatusId = s.Id
                     WHERE s.StatusName IN (N'مغلقة', N'كسبنا', N'خسرنا'))    AS SuccessRate";
            return conn.QueryFirstOrDefault<ReportStats>(sql) ?? new ReportStats();
        }

        // ── Monthly Cases + Hearings ──────────────────
        public List<MonthlyData> GetMonthlyCasesAndHearings()
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                SELECT 
                    m.MonthNum,
                    DATENAME(MONTH, DATEFROMPARTS(YEAR(GETDATE()), m.MonthNum, 1)) AS MonthName,
                    ISNULL(c.CaseCount, 0)    AS Cases,
                    ISNULL(h.HearingCount, 0) AS Hearings
                FROM (VALUES(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) m(MonthNum)
                LEFT JOIN (
                    SELECT MONTH(CreatedAt) AS M, COUNT(*) AS CaseCount
                    FROM Legal.Cases
                    WHERE YEAR(CreatedAt) = YEAR(GETDATE())
                    GROUP BY MONTH(CreatedAt)
                ) c ON c.M = m.MonthNum
                LEFT JOIN (
                    SELECT MONTH(HearingDateTime) AS M, COUNT(*) AS HearingCount
                    FROM Legal.Hearings
                    WHERE YEAR(HearingDateTime) = YEAR(GETDATE())
                    GROUP BY MONTH(HearingDateTime)
                ) h ON h.M = m.MonthNum
                WHERE m.MonthNum <= MONTH(GETDATE())
                ORDER BY m.MonthNum";
            return conn.Query<MonthlyData>(sql).AsList();
        }

        // ── Case Status Distribution ──────────────────
        public List<StatusData> GetCaseStatusDistribution()
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                SELECT 
                    s.StatusName AS Status,
                    s.Color,
                    COUNT(c.Id)  AS Count
                FROM Lookup.CaseStatuses s
                LEFT JOIN Legal.Cases c ON c.StatusId = s.Id
                GROUP BY s.StatusName, s.Color
                ORDER BY Count DESC";
            return conn.Query<StatusData>(sql).AsList();
        }

        // ── Top Active Clients ────────────────────────
        public List<ClientActivity> GetTopClients()
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                SELECT TOP 5
                    cl.FullName AS ClientName,
                    COUNT(c.Id) AS CaseCount,
                    ROW_NUMBER() OVER (ORDER BY COUNT(c.Id) DESC) AS Rank
                FROM Legal.Clients cl
                LEFT JOIN Legal.Cases c ON c.ClientId = cl.Id
                GROUP BY cl.Id, cl.FullName
                ORDER BY CaseCount DESC";
            return conn.Query<ClientActivity>(sql).AsList();
        }

        // ── Recent Activity ───────────────────────────
        public List<ActivityItem> GetRecentActivity()
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                SELECT TOP 5
                    N'إضافة قضية جديدة' AS Action,
                    Title               AS Detail,
                    CreatedAt           AS ActionTime
                FROM Legal.Cases
                UNION ALL
                SELECT TOP 5
                    N'إضافة عميل جديد',
                    FullName,
                    CreatedAt
                FROM Legal.Clients
                ORDER BY ActionTime DESC";
            return conn.Query<ActivityItem>(sql).AsList();
        }

        // ── Upcoming Hearings Report ──────────────────
        public List<HearingReport> GetUpcomingHearings()
        {
            using var conn = DbHelper.GetConnection();
            string sql = @"
                SELECT TOP 10
                    c.CaseNumber,
                    c.Title       AS CaseTitle,
                    cl.FullName   AS ClientName,
                    ct.CourtName,
                    h.HearingDateTime,
                    h.HearingType,
                    h.JudgeName
                FROM Legal.Hearings h
                INNER JOIN Legal.Cases c    ON h.CaseId  = c.Id
                INNER JOIN Legal.Clients cl ON c.ClientId = cl.Id
                INNER JOIN Lookup.Courts ct ON h.CourtId = ct.Id
                WHERE h.HearingDateTime >= GETDATE()
                ORDER BY h.HearingDateTime ASC";
            return conn.Query<HearingReport>(sql).AsList();
        }
    }

    // ── DTOs ──────────────────────────────────────────
    public class ReportStats
    {
        public int TotalCases { get; set; }
        public int ActiveClients { get; set; }
        public int HearingsThisMonth { get; set; }
        public int SuccessRate { get; set; }
    }

    public class MonthlyData
    {
        public string MonthName { get; set; }
        public int Cases { get; set; }
        public int Hearings { get; set; }
    }

    public class StatusData
    {
        public string Status { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
    }

    public class ClientActivity
    {
        public string ClientName { get; set; }
        public int CaseCount { get; set; }
        public int Rank { get; set; }
    }

    public class ActivityItem
    {
        public string Action { get; set; }
        public string Detail { get; set; }
        public DateTime ActionTime { get; set; }
    }

    public class HearingReport
    {
        public string CaseNumber { get; set; }
        public string CaseTitle { get; set; }
        public string ClientName { get; set; }
        public string CourtName { get; set; }
        public DateTime HearingDateTime { get; set; }
        public string HearingType { get; set; }
        public string JudgeName { get; set; }
    }
}
