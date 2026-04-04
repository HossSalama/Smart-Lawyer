using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SmartLawyerFinal.UserControls.Dashboard
{
    public partial class UcDashboard : UserControl
    {
        public UcDashboard()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Dock = DockStyle.Fill;
            this.Load += (s, e) => BuildDashboard();
        }
        private void BuildDashboard()
        {
            this.Controls.Clear();

            int W = this.ClientSize.Width;

            var scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20, 10, 20, 20),
                BackColor = Color.FromArgb(245, 245, 250)
            };
            scroll.HorizontalScroll.Enabled = false;
            scroll.HorizontalScroll.Visible = false;

            //  Title 
            scroll.Controls.Add(new Label
            {
                Text = "لوحة التحكم",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Location = new Point(W - 305, 15),
                Size = new Size(285, 42),
                TextAlign = ContentAlignment.MiddleRight
            });
            scroll.Controls.Add(new Label
            {
                Text = "نظرة عامة على جميع الأنشطة والإحصائيات",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(W - 470, 57),
                Size = new Size(450, 24),
                TextAlign = ContentAlignment.MiddleRight
            });

            // 4 Cards
            int cw = (W - 40) / 4;
            var cards = new[]
            {
                ("إجمالي القضايا",     "147","12%+",Color.FromArgb(230,81,0),  Color.FromArgb(255,240,230),"⚖"),
                ("العملاء النشطين",    "89", "8%+", Color.FromArgb(34,197,94), Color.FromArgb(230,255,240),"👥"),
                ("جلسات هذا الأسبوع", "23", "5+",  Color.FromArgb(230,81,0),  Color.FromArgb(255,240,230),"📅"),
                ("تنبيهات عاجلة",      "7",  "عاجل",Color.FromArgb(239,68,68), Color.FromArgb(255,230,230),"🔔"),
            };
            for (int i = 0; i < cards.Length; i++)
            {
                var (t, v, tr, c, bg, ic) = cards[i];
                scroll.Controls.Add(MakeCard(t, v, tr, c, bg, ic,
                    10 + i * (cw + 10), 92, cw, 112));
            }

            // Charts
            int cy = 224, hw = (W - 28) / 2;
            var pnlPie = WPanel(10, cy, hw, 285, "حالة القضايا");
            var pnlBar = WPanel(18 + hw, cy, hw, 285, "القضايا الشهرية");

            pnlPie.Controls.Add(new PieChartControl
            { Location = new Point(hw / 2 - 78, 50), Size = new Size(156, 156) });
            AddLegend(pnlPie, hw);
            AddBars(pnlBar, hw);

            // ── Row 3 ─────────────────────────────────
            int r3y = cy + 300, lw = (int)(W * 0.60), rw2 = W - lw - 26;
            var pnlH = WPanel(10, r3y, lw, 290, "الجلسات القادمة");
            var pnlA = WPanel(18 + lw, r3y, rw2, 290, "إجراءات سريعة");

            AddHearBtn(pnlH);
            AddHearRows(pnlH, lw);
            AddActions(pnlA, rw2);

            scroll.Controls.AddRange(new Control[]
                { pnlPie, pnlBar, pnlH, pnlA });

            this.Controls.Add(scroll);
        }

        private Panel MakeCard(string title, string val, string trend,
            Color ic, Color bg, string icon, int x, int y, int w, int h)
        {
            var c = new Panel
            { Size = new Size(w, h), Location = new Point(x, y), BackColor = Color.White };
            R(c, 12);
            var pi = new Panel
            { Size = new Size(44, 44), Location = new Point(w - 56, 11), BackColor = bg };
            R(pi, 22);
            pi.Controls.Add(new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 15),
                BackColor = Color.Transparent,
                Size = new Size(44, 44),
                TextAlign = ContentAlignment.MiddleCenter
            });
            c.Controls.AddRange(new Control[]
            {
                pi,
                new Label { Text=val,          Font=new Font("Segoe UI",22,FontStyle.Bold), ForeColor=Color.FromArgb(30,30,50), Location=new Point(8,7),  Size=new Size(w-68,38), TextAlign=ContentAlignment.MiddleRight },
                new Label { Text=title,        Font=new Font("Segoe UI",8),                 ForeColor=Color.Gray,              Location=new Point(8,48), Size=new Size(w-14,17), TextAlign=ContentAlignment.MiddleRight },
                new Label { Text=$"↑ {trend}", Font=new Font("Segoe UI",8,FontStyle.Bold),  ForeColor=Color.FromArgb(34,197,94),Location=new Point(8,67), Size=new Size(w-14,17), TextAlign=ContentAlignment.MiddleRight },
            });
            return c;
        }

        private Panel WPanel(int x, int y, int w, int h, string title)
        {
            var p = new Panel
            { Location = new Point(x, y), Size = new Size(w, h), BackColor = Color.White };
            R(p, 12);
            p.Controls.Add(new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Location = new Point(10, 13),
                Size = new Size(w - 20, 27),
                TextAlign = ContentAlignment.MiddleRight
            });
            return p;
        }

        private void AddLegend(Panel pnl, int w)
        {
            var items = new[]
            {
                ("جارية: 59%",  Color.FromArgb(59,130,246)),
                ("مكتملة: 31%", Color.FromArgb(34,197,94)),
                ("معلقة: 10%",  Color.FromArgb(230,81,0))
            };
            int y = 222;
            foreach (var (txt, col) in items)
            {
                var dot = new Panel
                { Size = new Size(10, 10), Location = new Point(w - 20, y + 4), BackColor = col };
                R(dot, 5);
                pnl.Controls.AddRange(new Control[]
                {
                    dot,
                    new Label { Text=txt, Font=new Font("Segoe UI",9), ForeColor=Color.FromArgb(60,60,60),
                        Location=new Point(14,y), Size=new Size(w-36,18), TextAlign=ContentAlignment.MiddleRight }
                });
                y += 21;
            }
        }

        private void AddBars(Panel pnl, int w)
        {
            var months = new[] { "يناير", "فبراير", "مارس", "أبريل" };
            var vals = new[] { 11, 15, 19, 8 };
            int bw = 48, maxH = 165, maxV = 20;
            int gap = (w - 40 - months.Length * bw) / (months.Length + 1);
            int sx = 20 + gap;

            for (int i = 0; i < months.Length; i++)
            {
                int bh = (int)(vals[i] / (float)maxV * maxH);
                int x = sx + i * (bw + gap);
                var bar = new Panel
                { Size = new Size(bw, bh), Location = new Point(x, 228 - bh), BackColor = Color.FromArgb(230, 81, 0) };
                var bp = new GraphicsPath();
                bp.AddArc(0, 0, 10, 10, 180, 90); bp.AddArc(bw - 10, 0, 10, 10, 270, 90);
                bp.AddLine(bw, bh, 0, bh); bp.CloseAllFigures();
                bar.Region = new Region(bp);
                pnl.Controls.AddRange(new Control[]
                {
                    bar,
                    new Label { Text=months[i], Font=new Font("Segoe UI",8), ForeColor=Color.Gray,
                        Location=new Point(x,236), Size=new Size(bw,18), TextAlign=ContentAlignment.MiddleCenter }
                });
            }
        }

        private void AddHearBtn(Panel pnl)
        {
            pnl.Controls.Add(new Guna2Button
            {
                Text = "عرض الكل",
                Size = new Size(78, 26),
                Location = new Point(11, 14),
                Font = new Font("Segoe UI", 8),
                FillColor = Color.FromArgb(245, 245, 250),
                ForeColor = Color.FromArgb(80, 80, 100),
                BorderRadius = 8,
                Cursor = Cursors.Hand
            });
        }

        private void AddHearRows(Panel pnl, int w)
        {
            var data = new[]
            {
                ("قضية رقم 2024/156","أحمد محمود", "محكمة القاهرة الابتدائية","2026-04-01","10:00 ص",false),
                ("قضية رقم 2024/142","سارة علي",   "محكمة الجيزة الابتدائية", "2026-04-02","11:30 ص",false),
                ("قضية رقم 2024/189","محمد حسن",   "محكمة الاستئناف",          "2026-04-03","09:00 ص",true),
            };
            int y = 50, rw = w - 26;
            foreach (var (cn, cl, ct, dt, tm, urg) in data)
            {
                var row = new Panel
                { Size = new Size(rw, 68), Location = new Point(13, y), BackColor = Color.FromArgb(250, 250, 252) };
                R(row, 8);
                var cal = new Panel
                { Size = new Size(36, 36), Location = new Point(rw - 48, 16), BackColor = Color.FromArgb(255, 240, 225) };
                R(cal, 8);
                cal.Controls.Add(new Label
                {
                    Text = "📅",
                    Font = new Font("Segoe UI", 13),
                    BackColor = Color.Transparent,
                    Size = new Size(36, 36),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                int tw = rw - 160;
                var lblCN = new Label { Text = cn, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(30, 30, 50), Location = new Point(96, 6), Size = new Size(tw, 17), TextAlign = ContentAlignment.MiddleRight };
                row.Controls.AddRange(new Control[]
                {
                    cal, lblCN,
                    new Label { Text=cl, Font=new Font("Segoe UI",8), ForeColor=Color.Gray, Location=new Point(96,23), Size=new Size(tw,15), TextAlign=ContentAlignment.MiddleRight },
                    new Label { Text=ct, Font=new Font("Segoe UI",8), ForeColor=Color.Gray, Location=new Point(96,38), Size=new Size(tw,15), TextAlign=ContentAlignment.MiddleRight },
                    new Label { Text=dt, Font=new Font("Segoe UI",8,FontStyle.Bold), ForeColor=Color.FromArgb(30,30,50), Location=new Point(7,7),  Size=new Size(86,15), TextAlign=ContentAlignment.MiddleLeft },
                    new Label { Text=$"🕐 {tm}", Font=new Font("Segoe UI",8), ForeColor=Color.Gray, Location=new Point(7,24), Size=new Size(86,15), TextAlign=ContentAlignment.MiddleLeft },
                    new Guna2Button { Text="التفاصيل", Size=new Size(65,21), Location=new Point(7,43), Font=new Font("Segoe UI",7), FillColor=Color.FromArgb(245,245,250), ForeColor=Color.FromArgb(80,80,100), BorderRadius=5, Cursor=Cursors.Hand }
                });
                if (urg)
                    row.Controls.Add(new Label
                    {
                        Text = "عاجل",
                        Font = new Font("Segoe UI", 7, FontStyle.Bold),
                        ForeColor = Color.White,
                        BackColor = Color.FromArgb(239, 68, 68),
                        Location = new Point(lblCN.Left - 52, 10),
                        Size = new Size(42, 17),
                        TextAlign = ContentAlignment.MiddleCenter
                    });
                pnl.Controls.Add(row);
                y += 74;
            }
        }

        private void AddActions(Panel pnl, int w)
        {
            var actions = new[]
            {
                ("إضافة قضية جديدة","ابدأ قضية جديدة",    "⚖", Color.FromArgb(255,240,225)),
                ("إضافة عميل جديد", "تسجيل عميل جديد",   "👥", Color.FromArgb(230,255,240)),
                ("جدولة جلسة",      "حدد موعد جلسة جديد","📅", Color.FromArgb(225,235,255)),
            };
            int y = 50, rw = w - 26;
            foreach (var (t, s, ic, bg) in actions)
            {
                var row = new Panel
                { Size = new Size(rw, 64), Location = new Point(13, y), BackColor = bg, Cursor = Cursors.Hand };
                R(row, 10);
                var pi = new Panel
                { Size = new Size(36, 36), Location = new Point(rw - 48, 14), BackColor = Color.White };
                R(pi, 8);
                pi.Controls.Add(new Label
                {
                    Text = ic,
                    Font = new Font("Segoe UI", 15),
                    BackColor = Color.Transparent,
                    Size = new Size(36, 36),
                    TextAlign = ContentAlignment.MiddleCenter
                });
                row.Controls.AddRange(new Control[]
                {
                    pi,
                    new Label { Text=t, Font=new Font("Segoe UI",9,FontStyle.Bold), ForeColor=Color.FromArgb(30,30,50), Location=new Point(7,10), Size=new Size(rw-56,19), TextAlign=ContentAlignment.MiddleRight },
                    new Label { Text=s, Font=new Font("Segoe UI",8), ForeColor=Color.Gray, Location=new Point(7,31), Size=new Size(rw-56,17), TextAlign=ContentAlignment.MiddleRight }
                });
                pnl.Controls.Add(row);
                y += 74;
            }
        }

        private void R(Control c, int r)
        {
            if (c.Width < r * 2 || c.Height < r * 2) return;
            var p = new GraphicsPath();
            p.AddArc(0, 0, r * 2, r * 2, 180, 90);
            p.AddArc(c.Width - r * 2, 0, r * 2, r * 2, 270, 90);
            p.AddArc(c.Width - r * 2, c.Height - r * 2, r * 2, r * 2, 0, 90);
            p.AddArc(0, c.Height - r * 2, r * 2, r * 2, 90, 90);
            p.CloseAllFigures();
            c.Region = new Region(p);
        }
    }

    public class PieChartControl : Control
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var slices = new[]
            {
                (59f,Color.FromArgb(59,130,246)),
                (31f,Color.FromArgb(34,197,94)),
                (10f,Color.FromArgb(230,81,0))
            };
            float start = -90f;
            var rect = new RectangleF(5, 5, Width - 10, Height - 10);
            foreach (var (pct, col) in slices)
            {
                float sw = pct / 100f * 360f;
                using var b = new SolidBrush(col);
                g.FillPie(b, rect.X, rect.Y, rect.Width, rect.Height, start, sw);
                start += sw;
            }
            int ins = 36;
            using var wb = new SolidBrush(Color.White);
            g.FillEllipse(wb, rect.X + ins, rect.Y + ins, rect.Width - ins * 2, rect.Height - ins * 2);
        }



    }

   

   
}
