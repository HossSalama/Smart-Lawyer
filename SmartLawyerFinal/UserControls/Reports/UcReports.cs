using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using SmartLawyerFinal.DAL.Repositories.ClassRepository;
using System.Drawing.Drawing2D;


namespace SmartLawyerFinal.UserControls.Reports
{
    public partial class UcReports : UserControl
    {
        private ReportsRepository _repo;
        private Panel _pnlScroll;
        private Panel _pnlChartArea;
        private string _activeTab = "monthly";
        public UcReports()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Load += UcReports_Load;
        }
        private void UcReports_Load(object sender, EventArgs e)
        {
            _repo = new ReportsRepository();
            if (this.Width > 100)
                BuildUI();
            else
                this.Resize += (s, re) =>
                {
                    if (this.Width > 100 && this.Controls.Count == 0)
                    {
                        _repo = new ReportsRepository();
                        BuildUI();
                    }
                };
        }
        private void BuildUI()
        {
            this.Controls.Clear();

            var scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(245, 245, 250),
                AutoScrollMinSize = new Size(0, 1400)
            };
            scroll.HorizontalScroll.Enabled = false;
            scroll.HorizontalScroll.Visible = false;
            _pnlScroll = scroll;

            int W = this.Width - 10;
            if (W < 600) W = 800;

            // ── Header ────────────────────────────────
            scroll.Controls.Add(new Label
            {
                Text = "التقارير والإحصائيات",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Location = new Point(W - 310, 15),
                Size = new Size(290, 42),
                TextAlign = ContentAlignment.MiddleRight
            });
            scroll.Controls.Add(new Label
            {
                Text = "عرض وتحليل البيانات والإحصائيات",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(W - 400, 57),
                Size = new Size(380, 24),
                TextAlign = ContentAlignment.MiddleRight
            });

            // Export Buttons
            var btnPdf = new Guna2Button
            {
                Text = "⬇ تصدير PDF",
                Size = new Size(130, 36),
                Location = new Point(160, 25),
                Font = new Font("Segoe UI", 9),
                FillColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };
            var btnExcel = new Guna2Button
            {
                Text = "⬇ تصدير Excel",
                Size = new Size(130, 36),
                Location = new Point(16, 25),
                Font = new Font("Segoe UI", 9),
                FillColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };
            btnPdf.Click += (s, e) => MessageBox.Show("سيتم تصدير PDF قريباً");
            btnExcel.Click += (s, e) => MessageBox.Show("سيتم تصدير Excel قريباً");
            scroll.Controls.AddRange(new Control[] { btnPdf, btnExcel });

            // Report Type Selector
            var pnlSelector = new Panel
            {
                Size = new Size(W - 20, 50),
                Location = new Point(10, 82),
                BackColor = Color.White
            };
            R(pnlSelector, 10);
            var lblType = new Label
            {
                Text = "نوع التقرير:",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(pnlSelector.Width - 110, 13),
                Size = new Size(100, 24),
                TextAlign = ContentAlignment.MiddleRight
            };
            var cmbType = new ComboBox
            {
                Size = new Size(200, 28),
                Location = new Point(pnlSelector.Width - 320, 11),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            new[] { "نظرة عامة", "تقرير القضايا", "تقرير الجلسات", "تقرير مالي" }
                .ForEach(i => cmbType.Items.Add(i));
            cmbType.SelectedIndex = 0;
            pnlSelector.Controls.AddRange(new Control[] { lblType, cmbType });
            scroll.Controls.Add(pnlSelector);

            // ── Stats Cards ───────────────────────────
            var stats = _repo.GetStats();
            int cw = (W - 50) / 4;
            var cards = new[]
            {
                ("إجمالي القضايا",   stats.TotalCases.ToString(),        "12%+ من الشهر الماضي", Color.FromArgb(59,130,246),  Color.FromArgb(230,240,255),"📋"),
                ("العملاء النشطين",  stats.ActiveClients.ToString(),     "8%+ من الشهر الماضي",  Color.FromArgb(34,197,94),   Color.FromArgb(230,255,240),"👥"),
                ("جلسات هذا الشهر",  stats.HearingsThisMonth.ToString(), "15%+ من الشهر الماضي", Color.FromArgb(230,81,0),    Color.FromArgb(255,240,230),"📅"),
                ("معدل النجاح",      $"{stats.SuccessRate}%",            "3%+ من الشهر الماضي",  Color.FromArgb(34,197,94),   Color.FromArgb(230,255,240),"📈"),
            };
            for (int i = 0; i < cards.Length; i++)
            {
                var (t, v, tr, c, bg, ic) = cards[i];
                scroll.Controls.Add(MakeCard(t, v, tr, c, bg, ic,
                    10 + i * (cw + 10), 150, cw, 120));
            }

            // ── Tabs ──────────────────────────────────
            int tabY = 290;
            var pnlTabs = new Panel
            {
                Size = new Size(W - 20, 46),
                Location = new Point(10, tabY),
                BackColor = Color.White
            };
            R(pnlTabs, 10);

            var tabDefs = new[]
            {
                ("monthly",  "الجلسات القادمة"),
                ("status",   "حالة القضايا"),
                ("types",    "أنواع القضايا"),
                ("monthly2", "تقرير شهري"),
            };
            int tx = pnlTabs.Width - 10;
            foreach (var (id, label) in tabDefs)
            {
                string tid = id;
                int tw = 130;
                tx -= tw + 8;
                var tab = new Guna2Button
                {
                    Text = label,
                    Size = new Size(tw, 34),
                    Location = new Point(tx, 6),
                    Font = new Font("Segoe UI", 9),
                    FillColor = id == _activeTab ? Color.FromArgb(230, 81, 0) : Color.FromArgb(245, 245, 250),
                    ForeColor = id == _activeTab ? Color.White : Color.FromArgb(80, 80, 100),
                    BorderRadius = 8,
                    Cursor = Cursors.Hand,
                    Tag = id
                };
                tab.Click += (s, e) =>
                {
                    _activeTab = tid;
                    BuildUI();
                };
                pnlTabs.Controls.Add(tab);
            }
            scroll.Controls.Add(pnlTabs);

            // ── Chart Area ────────────────────────────
            int chartY = tabY + 56;
            BuildChartArea(scroll, W, chartY);

            // ── Bottom Row ────────────────────────────
            int bottomY = chartY + 420;
            BuildTopClients(scroll, (W - 30) / 2, bottomY);
            BuildRecentActivity(scroll, (W - 30) / 2, bottomY, (W - 30) / 2 + 20);

            this.Controls.Add(scroll);
        }

        private void BuildChartArea(Panel scroll, int W, int y)
        {
            var pnl = WPanel(10, y, W - 20, 400);

            if (_activeTab == "monthly" || _activeTab == "monthly2")
            {
                pnl.Controls.Add(new Label
                {
                    Text = "القضايا والجلسات الشهرية",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 30, 50),
                    Location = new Point(10, 14),
                    Size = new Size(pnl.Width - 20, 27),
                    TextAlign = ContentAlignment.MiddleRight
                });

                var data = _repo.GetMonthlyCasesAndHearings();
                if (data.Count > 0) DrawDoubleBar(pnl, data, W - 40);
            }
            else if (_activeTab == "status")
            {
                pnl.Controls.Add(new Label
                {
                    Text = "حالة القضايا",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 30, 50),
                    Location = new Point(10, 14),
                    Size = new Size(pnl.Width - 20, 27),
                    TextAlign = ContentAlignment.MiddleRight
                });
                DrawStatusChart(pnl, W - 40);
            }
            else if (_activeTab == "types")
            {
                pnl.Controls.Add(new Label
                {
                    Text = "أنواع القضايا",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 30, 50),
                    Location = new Point(10, 14),
                    Size = new Size(pnl.Width - 20, 27),
                    TextAlign = ContentAlignment.MiddleRight
                });
                DrawUpcomingHearings(pnl, W - 40);
            }

            scroll.Controls.Add(pnl);
        }

        private void DrawDoubleBar(Panel pnl, System.Collections.Generic.List<MonthlyData> data, int w)
        {
            int maxVal = 30, maxH = 280, barW = 30, gapGroup = 20;
            int groupW = barW * 2 + 10;
            int totalW = data.Count * (groupW + gapGroup);
            int startX = (w - totalW) / 2;

            var colors = new[]
            {
                Color.FromArgb(230, 81,  0),
                Color.FromArgb(59,  130, 246)
            };

            for (int i = 0; i < data.Count; i++)
            {
                int gx = startX + i * (groupW + gapGroup);

                // Cases bar
                int h1 = (int)(data[i].Cases / (float)maxVal * maxH);
                int h2 = (int)(data[i].Hearings / (float)maxVal * maxH);
                if (h1 < 4) h1 = 4;
                if (h2 < 4) h2 = 4;

                var bar1 = new Panel { Size = new Size(barW, h1), Location = new Point(gx, 340 - h1), BackColor = colors[0] };
                var bar2 = new Panel { Size = new Size(barW, h2), Location = new Point(gx + barW + 8, 340 - h2), BackColor = colors[1] };

                var rp1 = new GraphicsPath();
                rp1.AddArc(0, 0, 8, 8, 180, 90); rp1.AddArc(barW - 8, 0, 8, 8, 270, 90);
                rp1.AddLine(barW, h1, 0, h1); rp1.CloseAllFigures();
                bar1.Region = new Region(rp1);

                var rp2 = new GraphicsPath();
                rp2.AddArc(0, 0, 8, 8, 180, 90); rp2.AddArc(barW - 8, 0, 8, 8, 270, 90);
                rp2.AddLine(barW, h2, 0, h2); rp2.CloseAllFigures();
                bar2.Region = new Region(rp2);

                pnl.Controls.AddRange(new Control[] { bar1, bar2,
                    new Label { Text = data[i].MonthName?.Substring(0, Math.Min(4, data[i].MonthName?.Length ?? 0)),
                        Font = new Font("Segoe UI", 8), ForeColor = Color.Gray,
                        Location = new Point(gx, 348), Size = new Size(groupW, 18),
                        TextAlign = ContentAlignment.MiddleCenter }
                });
            }

            // Legend
            AddLegendItem(pnl, colors[0], "القضايا", w - 180, 370);
            AddLegendItem(pnl, colors[1], "الجلسات", w - 100, 370);
        }

        private void DrawStatusChart(Panel pnl, int w)
        {
            var data = _repo.GetCaseStatusDistribution();
            int total = 0;
            data.ForEach(d => total += d.Count);
            if (total == 0) total = 1;

            var statusColors = new[]
            {
                Color.FromArgb(59, 130, 246),
                Color.FromArgb(230, 81,  0),
                Color.FromArgb(34, 197,  94),
                Color.FromArgb(239, 68,  68),
                Color.FromArgb(168, 85, 247),
            };

            int cx = w / 2, cy = 200, radius = 130;
            var pieCtrl = new StatusPieControl(data, total, statusColors)
            {
                Location = new Point(cx - radius, 50),
                Size = new Size(radius * 2, radius * 2)
            };
            pnl.Controls.Add(pieCtrl);

            int ly = 50;
            for (int i = 0; i < data.Count && i < statusColors.Length; i++)
            {
                var dot = new Panel
                { Size = new Size(12, 12), Location = new Point(w - 25, ly + 3), BackColor = statusColors[i] };
                R(dot, 6);
                pnl.Controls.AddRange(new Control[]
                {
                    dot,
                    new Label { Text = $"{data[i].Status}: {data[i].Count}", Font = new Font("Segoe UI", 9),
                        ForeColor = Color.FromArgb(60,60,60), Location = new Point(20, ly),
                        Size = new Size(w - 40, 20), TextAlign = ContentAlignment.MiddleRight }
                });
                ly += 26;
            }
        }

        private void DrawUpcomingHearings(Panel pnl, int w)
        {
            var hearings = _repo.GetUpcomingHearings();
            int y = 52;
            if (hearings.Count == 0)
            {
                pnl.Controls.Add(new Label
                {
                    Text = "لا توجد جلسات قادمة",
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.Gray,
                    Location = new Point(10, 150),
                    Size = new Size(w, 40),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                return;
            }
            foreach (var h in hearings)
            {
                var row = new Panel
                { Size = new Size(w, 50), Location = new Point(10, y), BackColor = Color.FromArgb(250, 250, 252) };
                R(row, 8);
                row.Controls.AddRange(new Control[]
                {
                    new Label { Text = h.CaseNumber, Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        ForeColor = Color.FromArgb(30,30,50), Location = new Point(row.Width-160, 6),
                        Size = new Size(150, 18), TextAlign = ContentAlignment.MiddleRight },
                    new Label { Text = h.ClientName, Font = new Font("Segoe UI", 8),
                        ForeColor = Color.Gray, Location = new Point(row.Width-160, 26),
                        Size = new Size(150, 16), TextAlign = ContentAlignment.MiddleRight },
                    new Label { Text = h.HearingDateTime.ToString("yyyy-MM-dd"), Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        ForeColor = Color.FromArgb(230,81,0), Location = new Point(10, 6),
                        Size = new Size(120, 18), TextAlign = ContentAlignment.MiddleLeft },
                    new Label { Text = h.CourtName, Font = new Font("Segoe UI", 8),
                        ForeColor = Color.Gray, Location = new Point(10, 26),
                        Size = new Size(150, 16), TextAlign = ContentAlignment.MiddleLeft }
                });
                pnl.Controls.Add(row);
                y += 56;
                if (y > 380) break;
            }
        }

        private void BuildTopClients(Panel scroll, int w, int y)
        {
            var pnl = WPanel(10, y, w, 280);
            pnl.Controls.Add(new Label
            {
                Text = "أكثر العملاء نشاطاً",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Location = new Point(10, 14),
                Size = new Size(w - 20, 27),
                TextAlign = ContentAlignment.MiddleRight
            });

            var clients = _repo.GetTopClients();
            int cy = 52;
            foreach (var cl in clients)
            {
                var row = new Panel
                { Size = new Size(w - 20, 40), Location = new Point(10, cy), BackColor = Color.FromArgb(250, 250, 252) };
                R(row, 8);

                var pnlRank = new Panel
                { Size = new Size(30, 30), Location = new Point(row.Width - 38, 5), BackColor = Color.FromArgb(230, 81, 0) };
                R(pnlRank, 15);
                pnlRank.Controls.Add(new Label
                {
                    Text = cl.Rank.ToString(),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Size = new Size(30, 30),
                    TextAlign = ContentAlignment.MiddleCenter
                });

                var badge = new Label
                {
                    Text = $"{cl.CaseCount} قضايا",
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(59, 130, 246),
                    Location = new Point(8, 10),
                    Size = new Size(70, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                row.Controls.AddRange(new Control[]
                {
                    pnlRank, badge,
                    new Label { Text = cl.ClientName, Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        ForeColor = Color.FromArgb(30,30,50), Location = new Point(row.Width-180,10),
                        Size = new Size(135,20), TextAlign = ContentAlignment.MiddleRight }
                });
                pnl.Controls.Add(row);
                cy += 46;
            }
            scroll.Controls.Add(pnl);
        }

        private void BuildRecentActivity(Panel scroll, int w, int y, int x)
        {
            var pnl = WPanel(x, y, w, 280);
            pnl.Controls.Add(new Label
            {
                Text = "النشاط الأخير",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Location = new Point(10, 14),
                Size = new Size(w - 20, 27),
                TextAlign = ContentAlignment.MiddleRight
            });

            var activities = _repo.GetRecentActivity();
            int ay = 52;
            foreach (var a in activities)
            {
                var row = new Panel
                { Size = new Size(w - 20, 44), Location = new Point(10, ay), BackColor = Color.FromArgb(250, 250, 252) };
                R(row, 8);

                var icon = new Panel
                { Size = new Size(32, 32), Location = new Point(row.Width - 40, 6), BackColor = Color.FromArgb(230, 240, 255) };
                R(icon, 8);
                icon.Controls.Add(new Label
                {
                    Text = "📋",
                    Font = new Font("Segoe UI", 13),
                    BackColor = Color.Transparent,
                    Size = new Size(32, 32),
                    TextAlign = ContentAlignment.MiddleCenter
                });

                string timeAgo = GetTimeAgo(a.ActionTime);
                row.Controls.AddRange(new Control[]
                {
                    icon,
                    new Label { Text = a.Action, Font = new Font("Segoe UI",9,FontStyle.Bold),
                        ForeColor = Color.FromArgb(30,30,50), Location = new Point(10,6),
                        Size = new Size(row.Width-55,18), TextAlign = ContentAlignment.MiddleRight },
                    new Label { Text = timeAgo, Font = new Font("Segoe UI",8),
                        ForeColor = Color.Gray, Location = new Point(10,26),
                        Size = new Size(row.Width-55,14), TextAlign = ContentAlignment.MiddleRight }
                });
                pnl.Controls.Add(row);
                ay += 50;
            }
            scroll.Controls.Add(pnl);
        }

        private void AddLegendItem(Panel pnl, Color color, string text, int x, int y)
        {
            var dot = new Panel { Size = new Size(12, 12), Location = new Point(x + 2, y + 4), BackColor = color };
            R(dot, 6);
            pnl.Controls.AddRange(new Control[]
            {
                dot,
                new Label { Text = text, Font = new Font("Segoe UI",9), ForeColor = Color.FromArgb(60,60,60),
                    Location = new Point(x-60,y), Size = new Size(60,20), TextAlign = ContentAlignment.MiddleRight }
            });
        }

        private string GetTimeAgo(DateTime dt)
        {
            var diff = DateTime.Now - dt;
            if (diff.TotalMinutes < 60) return $"منذ {(int)diff.TotalMinutes} دقيقة";
            if (diff.TotalHours < 24) return $"منذ {(int)diff.TotalHours} ساعة";
            return $"منذ {(int)diff.TotalDays} يوم";
        }

        private Panel WPanel(int x, int y, int w, int h)
        {
            var p = new Panel { Location = new Point(x, y), Size = new Size(w, h), BackColor = Color.White };
            R(p, 12);
            return p;
        }

        private Panel MakeCard(string title, string val, string trend,
            Color ic, Color bg, string icon, int x, int y, int w, int h)
        {
            var card = new Panel { Size = new Size(w, h), Location = new Point(x, y), BackColor = Color.White };
            R(card, 12);
            var pi = new Panel { Size = new Size(44, 44), Location = new Point(w - 56, 11), BackColor = bg };
            R(pi, 22);
            pi.Controls.Add(new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 15),
                BackColor = Color.Transparent,
                Size = new Size(44, 44),
                TextAlign = ContentAlignment.MiddleCenter
            });
            card.Controls.AddRange(new Control[]
            {
                pi,
                new Label { Text=val,   Font=new Font("Segoe UI",20,FontStyle.Bold), ForeColor=ic,                    Location=new Point(8,7),  Size=new Size(w-68,36), TextAlign=ContentAlignment.MiddleRight },
                new Label { Text=trend, Font=new Font("Segoe UI",7),                 ForeColor=Color.FromArgb(34,197,94), Location=new Point(8,48), Size=new Size(w-14,15), TextAlign=ContentAlignment.MiddleRight },
                new Label { Text=title, Font=new Font("Segoe UI",8),                 ForeColor=Color.Gray,              Location=new Point(8,65), Size=new Size(w-14,16), TextAlign=ContentAlignment.MiddleRight },
            });
            return card;
        }

        private void R(Control c, int r)
        {
            if (c.Width < r * 2 || c.Height < r * 2) return;
            var p = new GraphicsPath();
            p.AddArc(0, 0, r * 2, r * 2, 180, 90); p.AddArc(c.Width - r * 2, 0, r * 2, r * 2, 270, 90);
            p.AddArc(c.Width - r * 2, c.Height - r * 2, r * 2, r * 2, 0, 90); p.AddArc(0, c.Height - r * 2, r * 2, r * 2, 90, 90);
            p.CloseAllFigures();
            c.Region = new Region(p);
        }
    }

    // ── Pie Chart Control ─────────────────────────────
    public class StatusPieControl : Control
    {
        private readonly System.Collections.Generic.List<StatusData> _data;
        private readonly int _total;
        private readonly Color[] _colors;

        public StatusPieControl(System.Collections.Generic.List<StatusData> data, int total, Color[] colors)
        {
            _data = data; _total = total; _colors = colors;
            this.BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float start = -90f;
            var rect = new RectangleF(5, 5, Width - 10, Height - 10);
            for (int i = 0; i < _data.Count && i < _colors.Length; i++)
            {
                float sw = _data[i].Count / (float)_total * 360f;
                using var b = new SolidBrush(_colors[i]);
                g.FillPie(b, rect.X, rect.Y, rect.Width, rect.Height, start, sw);
                start += sw;
            }
            int ins = 40;
            using var wb = new SolidBrush(Color.White);
            g.FillEllipse(wb, rect.X + ins, rect.Y + ins, rect.Width - ins * 2, rect.Height - ins * 2);
        }
    }
}

static class ArrayExtensions
{
    public static void ForEach<T>(this T[] arr, System.Action<T> action)
    {
        foreach (var item in arr) action(item);
    }
}