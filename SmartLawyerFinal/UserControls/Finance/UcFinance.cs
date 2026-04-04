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
using SmartLawyerFinal.BLL.Services;
using SmartLawyerFinal.DAL.Repositories;
using SmartLawyerFinal.Models;

using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SmartLawyerFinal.UserControls.Finance
{
    public partial class UcFinance : UserControl
    {
        private FinanceRepository _repo;
        private List<Fee> _fees;
        private Panel _pnlScroll;
        private TextBox _txtSearch;
        public UcFinance()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Load += UcFinance_Load;
        }
        private void UcFinance_Load(object sender, EventArgs e)
        {
            _repo = new FinanceRepository();
            BuildUI();
        }
        private void BuildUI()
        {
            this.Controls.Clear();

            var scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(245, 245, 250),
                AutoScrollMinSize = new Size(0, 1200)
            };
            scroll.HorizontalScroll.Enabled = false;
            scroll.HorizontalScroll.Visible = false;
            _pnlScroll = scroll;

            int W = this.Width - 10;
            if (W < 600) W = 800;

            // ── Header ────────────────────────────────
            var lblTitle = new Label
            {
                Text = "الأتعاب والدفعات",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Location = new Point(W - 310, 15),
                Size = new Size(290, 42),
                TextAlign = ContentAlignment.MiddleRight
            };
            var lblSub = new Label
            {
                Text = "إدارة أتعاب القضايا ومتابعة الدفعات",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(W - 420, 57),
                Size = new Size(400, 24),
                TextAlign = ContentAlignment.MiddleRight
            };

            var btnAdd = new Guna2Button
            {
                Text = "+ تسجيل دفعة جديدة",
                Size = new Size(170, 38),
                Location = new Point(10, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FillColor = Color.FromArgb(30, 30, 50),
                ForeColor = Color.White,
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => ShowAddFeeDialog();

            scroll.Controls.AddRange(new Control[] { lblTitle, lblSub, btnAdd });

            // ── Summary Cards ─────────────────────────
            var summary = _repo.GetFinanceSummary();
            BuildSummaryCards(scroll, summary, W);

            // ── Search ────────────────────────────────
            var pnlSearch = new Panel
            {
                Size = new Size(W - 20, 44),
                Location = new Point(10, 230),
                BackColor = Color.White
            };
            R(pnlSearch, 10);

            _txtSearch = new TextBox
            {
                Size = new Size(pnlSearch.Width - 50, 30),
                Location = new Point(10, 7),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                Text = "البحث برقم القضية أو اسم العميل...",
                ForeColor = Color.Gray
            };
            _txtSearch.GotFocus += (s, e) => { if (_txtSearch.Text.StartsWith("البحث")) { _txtSearch.Text = ""; _txtSearch.ForeColor = Color.Black; } };
            _txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(_txtSearch.Text)) { _txtSearch.Text = "البحث برقم القضية أو اسم العميل..."; _txtSearch.ForeColor = Color.Gray; } };
            _txtSearch.TextChanged += (s, e) => FilterFees();

            var lblSearchIcon = new Label
            {
                Text = "🔍",
                Font = new Font("Segoe UI", 13),
                Size = new Size(35, 35),
                Location = new Point(pnlSearch.Width - 42, 5),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            pnlSearch.Controls.AddRange(new Control[] { _txtSearch, lblSearchIcon });
            scroll.Controls.Add(pnlSearch);

            // ── Fees List ─────────────────────────────
            LoadFees(scroll, W, 290);

            // ── Monthly Revenue ───────────────────────
            BuildMonthlyRevenue(scroll, W);

            this.Controls.Add(scroll);
        }

        private void BuildSummaryCards(Panel scroll, FinanceSummary s, int W)
        {
            int cw = (W - 50) / 4;
            var cards = new[]
            {
                ("إجمالي المحصل",    $"{s.TotalPaid:N0}", "ج.م",   Color.FromArgb(34,197,94),  Color.FromArgb(230,255,240),"$"),
                ("المبالغ المستحقة", $"{s.TotalAmount:N0}","ج.م",  Color.FromArgb(230,81,0),   Color.FromArgb(255,240,230),"🕐"),
                ("مدفوعة بالكامل",  $"{s.FullyPaid}",     "قضية",  Color.FromArgb(59,130,246), Color.FromArgb(230,240,255),"✓"),
                ("متأخرة",           $"{s.Overdue}",       "قضية",  Color.FromArgb(239,68,68),  Color.FromArgb(255,230,230),"⚠"),
            };
            for (int i = 0; i < cards.Length; i++)
            {
                var (title, val, unit, c, bg, ic) = cards[i];
                scroll.Controls.Add(MakeSummaryCard(title, val, unit, c, bg, ic,
                    10 + i * (cw + 10), 92, cw, 115));
            }
        }

        private Panel MakeSummaryCard(string title, string val, string unit,
            Color ic, Color bg, string icon, int x, int y, int w, int h)
        {
            var card = new Panel
            { Size = new Size(w, h), Location = new Point(x, y), BackColor = Color.White };
            R(card, 12);

            var pnlIc = new Panel
            { Size = new Size(46, 46), Location = new Point(w - 58, 12), BackColor = bg };
            R(pnlIc, 23);
            pnlIc.Controls.Add(new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 16),
                BackColor = Color.Transparent,
                Size = new Size(46, 46),
                TextAlign = ContentAlignment.MiddleCenter
            });

            card.Controls.AddRange(new Control[]
            {
                pnlIc,
                new Label { Text=val,   Font=new Font("Segoe UI",20,FontStyle.Bold), ForeColor=ic,                    Location=new Point(8,8),  Size=new Size(w-70,38), TextAlign=ContentAlignment.MiddleRight },
                new Label { Text=unit,  Font=new Font("Segoe UI",9),                 ForeColor=Color.Gray,            Location=new Point(8,46), Size=new Size(w-14,18), TextAlign=ContentAlignment.MiddleRight },
                new Label { Text=title, Font=new Font("Segoe UI",8),                 ForeColor=Color.FromArgb(80,80,80), Location=new Point(8,65), Size=new Size(w-14,18), TextAlign=ContentAlignment.MiddleRight },
            });
            return card;
        }

        private void LoadFees(Panel scroll, int W, int startY)
        {
            _fees = _repo.GetAllFees();
            RenderFees(scroll, W, startY, _fees);
        }

        private void RenderFees(Panel scroll, int W, int startY, List<Fee> fees)
        {
            // امسح الـ fee cards القديمة
            var toRemove = new List<Control>();
            foreach (Control c in scroll.Controls)
                if (c.Tag?.ToString() == "feecard") toRemove.Add(c);
            foreach (var c in toRemove) scroll.Controls.Remove(c);

            int y = startY;
            foreach (var fee in fees)
            {
                var card = BuildFeeCard(fee, W - 20);
                card.Location = new Point(10, y);
                card.Tag = "feecard";
                scroll.Controls.Add(card);
                y += card.Height + 15;
            }
        }

        private Panel BuildFeeCard(Fee fee, int w)
        {
            // حساب ارتفاع الـ card حسب عدد الدفعات
            var payments = _repo.GetPaymentsByFeeId(fee.Id);
            int cardH = 200 + (payments.Count * 44) + 60;

            var card = new Panel
            { Size = new Size(w, cardH), BackColor = Color.White };
            R(card, 12);

            // ── Header Row ────────────────────────────
            var pnlIcon = new Panel
            { Size = new Size(44, 44), Location = new Point(w - 56, 14), BackColor = Color.FromArgb(255, 240, 225) };
            R(pnlIcon, 8);
            pnlIcon.Controls.Add(new Label
            {
                Text = "$",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(230, 81, 0),
                BackColor = Color.Transparent,
                Size = new Size(44, 44),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // Status Badge
            var badgeColor = fee.IsFullyPaid ? Color.FromArgb(34, 197, 94) :
                             fee.IsOverdue ? Color.FromArgb(239, 68, 68) :
                                              Color.FromArgb(59, 130, 246);
            var badge = new Label
            {
                Text = fee.StatusText,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = badgeColor,
                Size = new Size(90, 22),
                Location = new Point(w - 156, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblCase = new Label { Text = $"القضية: {fee.CaseNumber}", Font = new Font("Segoe UI", 13, FontStyle.Bold), ForeColor = Color.FromArgb(30, 30, 50), Location = new Point(w - 310, 14), Size = new Size(290, 26), TextAlign = ContentAlignment.MiddleRight };
            var lblClient = new Label { Text = fee.ClientName, Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(w - 310, 40), Size = new Size(290, 18), TextAlign = ContentAlignment.MiddleRight };

            // Dates
            var lblDates = new Label
            {
                Text = $"📅 البداية: {fee.CreatedAt:yyyy-MM-dd}    📅 الاستحقاق: {fee.DueDate:yyyy-MM-dd}",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Location = new Point(10, 60),
                Size = new Size(w - 20, 18),
                TextAlign = ContentAlignment.MiddleRight
            };

            // Progress
            var lblProgress = new Label
            {
                Text = "تقدم السداد",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(w - 150, 85),
                Size = new Size(140, 18),
                TextAlign = ContentAlignment.MiddleRight
            };
            var lblAmounts = new Label
            {
                Text = $"{fee.TotalPaid:N0} / {fee.TotalAmount:N0} ج.م",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Location = new Point(10, 85),
                Size = new Size(200, 18),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Progress Bar
            var pnlBarBg = new Panel
            { Size = new Size(w - 20, 10), Location = new Point(10, 108), BackColor = Color.FromArgb(230, 230, 235) };
            R(pnlBarBg, 5);
            int fillW = (int)((fee.PaymentPercent / 100.0) * (w - 20));
            if (fillW > 0)
            {
                var pnlFill = new Panel
                { Size = new Size(fillW, 10), Location = new Point(0, 0), BackColor = Color.FromArgb(30, 30, 50) };
                R(pnlFill, 5);
                pnlBarBg.Controls.Add(pnlFill);
            }

            var lblPct = new Label { Text = $"المدفوع: {fee.PaymentPercent}%", Font = new Font("Segoe UI", 8), ForeColor = Color.Gray, Location = new Point(w - 120, 122), Size = new Size(110, 16), TextAlign = ContentAlignment.MiddleRight };
            var lblRemaining = new Label { Text = $"المتبقي: {fee.Remaining:N0} ج.م", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(239, 68, 68), Location = new Point(10, 122), Size = new Size(200, 16), TextAlign = ContentAlignment.MiddleLeft };

            card.Controls.AddRange(new Control[]
                { pnlIcon, badge, lblCase, lblClient, lblDates,
                  lblProgress, lblAmounts, pnlBarBg, lblPct, lblRemaining });

            // ── Payments List ─────────────────────────
            var lblPayTitle = new Label
            {
                Text = $"📋 سجل الدفعات ({payments.Count})",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Location = new Point(w - 200, 148),
                Size = new Size(190, 22),
                TextAlign = ContentAlignment.MiddleRight
            };
            card.Controls.Add(lblPayTitle);

            int py = 175;
            foreach (var p in payments)
            {
                var pRow = new Panel
                { Size = new Size(w - 20, 38), Location = new Point(10, py), BackColor = Color.FromArgb(250, 250, 252) };
                R(pRow, 6);

                var lblRec = new Guna2Button { Text = p.ReceiptNumber ?? "---", Size = new Size(100, 24), Location = new Point(8, 7), Font = new Font("Segoe UI", 7), FillColor = Color.FromArgb(245, 245, 250), ForeColor = Color.FromArgb(80, 80, 100), BorderRadius = 5 };
                var lblAmt = new Label { Text = $"{p.Amount:N0} ج.م", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(34, 197, 94), Location = new Point(pRow.Width - 160, 5), Size = new Size(150, 14), TextAlign = ContentAlignment.MiddleRight };
                var lblInfo = new Label { Text = $"{p.PaymentDate:yyyy-MM-dd} - {p.Method}", Font = new Font("Segoe UI", 7), ForeColor = Color.Gray, Location = new Point(pRow.Width - 160, 20), Size = new Size(150, 14), TextAlign = ContentAlignment.MiddleRight };
                var lblChk = new Label { Text = "✅", Font = new Font("Segoe UI", 12), BackColor = Color.Transparent, Size = new Size(26, 26), Location = new Point(pRow.Width - 28, 6), TextAlign = ContentAlignment.MiddleCenter };

                pRow.Controls.AddRange(new Control[] { lblRec, lblAmt, lblInfo, lblChk });
                card.Controls.Add(pRow);
                py += 44;
            }

            // ── Action Buttons ────────────────────────
            int btnY = py + 8;

            var btnPay = new Guna2Button
            {
                Text = "+ تسجيل دفعة جديدة",
                Size = new Size(155, 32),
                Location = new Point(w - 170, btnY),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FillColor = Color.FromArgb(30, 30, 50),
                ForeColor = Color.White,
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };
            btnPay.Click += (s, e) => ShowAddPaymentDialog(fee);

            var btnStatement = new Guna2Button
            {
                Text = "كشف حساب",
                Size = new Size(100, 32),
                Location = new Point(w - 280, btnY),
                Font = new Font("Segoe UI", 9),
                FillColor = Color.FromArgb(245, 245, 250),
                ForeColor = Color.FromArgb(80, 80, 100),
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };

            var btnDetails = new Guna2Button
            {
                Text = "عرض التفاصيل",
                Size = new Size(110, 32),
                Location = new Point(w - 395, btnY),
                Font = new Font("Segoe UI", 9),
                FillColor = Color.FromArgb(245, 245, 250),
                ForeColor = Color.FromArgb(80, 80, 100),
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };

            card.Controls.AddRange(new Control[] { btnPay, btnStatement, btnDetails });

            return card;
        }

        private void BuildMonthlyRevenue(Panel scroll, int W)
        {
            var revenues = _repo.GetMonthlyRevenue();
            if (revenues.Count == 0) return;

            // تحديد Y
            int lastFeeY = 290;
            foreach (Control c in scroll.Controls)
                if (c.Tag?.ToString() == "feecard")
                    lastFeeY = Math.Max(lastFeeY, c.Bottom + 15);

            var pnl = new Panel
            {
                Size = new Size(W - 20, 160),
                Location = new Point(10, lastFeeY + 10),
                BackColor = Color.White,
                Tag = "revenue"
            };
            R(pnl, 12);

            pnl.Controls.Add(new Label
            {
                Text = "الإيرادات الشهرية",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Location = new Point(10, 14),
                Size = new Size(W - 30, 27),
                TextAlign = ContentAlignment.MiddleRight
            });

            int cw = (W - 60) / revenues.Count;
            for (int i = 0; i < revenues.Count; i++)
            {
                var r = revenues[i];
                int cx = 10 + i * (cw + 10);

                pnl.Controls.AddRange(new Control[]
                {
                    new Label { Text=r.Month,              Font=new Font("Segoe UI",9),               ForeColor=Color.Gray,               Location=new Point(cx,48),  Size=new Size(cw,18), TextAlign=ContentAlignment.MiddleCenter },
                    new Label { Text=$"{r.Amount:N0} ج.م", Font=new Font("Segoe UI",13,FontStyle.Bold),ForeColor=Color.FromArgb(34,197,94), Location=new Point(cx,68),  Size=new Size(cw,28), TextAlign=ContentAlignment.MiddleCenter },
                    new Label { Text="↑ " + (10 + i * 2) + "%+", Font=new Font("Segoe UI",8),        ForeColor=Color.FromArgb(34,197,94), Location=new Point(cx,98),  Size=new Size(cw,16), TextAlign=ContentAlignment.MiddleCenter },
                });
            }

            scroll.Controls.Add(pnl);
        }

        private void FilterFees()
        {
            string q = _txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(q) || q.StartsWith("البحث"))
            {
                RenderFees(_pnlScroll, this.Width - 10, 290, _fees);
                return;
            }
            var filtered = _fees.FindAll(f =>
                (f.CaseNumber?.ToLower().Contains(q) ?? false) ||
                (f.ClientName?.ToLower().Contains(q) ?? false) ||
                (f.CaseTitle?.ToLower().Contains(q) ?? false));
            RenderFees(_pnlScroll, this.Width - 10, 290, filtered);
        }

        // ── Dialogs ───────────────────────────────────

        private void ShowAddFeeDialog()
        {
            var dlg = new Form
            {
                Text = "إضافة أتعاب جديدة",
                Size = new Size(480, 420),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackColor = Color.White,
                RightToLeft = RightToLeft.Yes
            };

            var cases = _repo.GetCasesForCombo();

            int y = 20;
            AddField(dlg, "القضية:", y); y += 28;
            var cmbCase = new ComboBox { Size = new Size(380, 28), Location = new Point(40, y), Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var c in cases) cmbCase.Items.Add(c.DisplayText);
            dlg.Controls.Add(cmbCase); y += 45;

            AddField(dlg, "نوع الأتعاب:", y); y += 28;
            var cmbType = new ComboBox { Size = new Size(380, 28), Location = new Point(40, y), Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var t in new[] { "ثابت", "نسبة", "بالساعة", "مقطوع" }) cmbType.Items.Add(t);
            cmbType.SelectedIndex = 0;
            dlg.Controls.Add(cmbType); y += 45;

            AddField(dlg, "إجمالي الأتعاب:", y); y += 28;
            var txtAmount = new TextBox { Size = new Size(380, 28), Location = new Point(40, y), Font = new Font("Segoe UI", 10) };
            dlg.Controls.Add(txtAmount); y += 45;

            AddField(dlg, "تاريخ الاستحقاق:", y); y += 28;
            var dtpDue = new DateTimePicker { Size = new Size(380, 28), Location = new Point(40, y), Font = new Font("Segoe UI", 10), Format = DateTimePickerFormat.Short };
            dlg.Controls.Add(dtpDue); y += 45;

            AddField(dlg, "ملاحظات:", y); y += 28;
            var txtNotes = new TextBox { Size = new Size(380, 28), Location = new Point(40, y), Font = new Font("Segoe UI", 10) };
            dlg.Controls.Add(txtNotes); y += 50;

            var btnSave = new Guna2Button
            {
                Text = "+  حفظ",
                Size = new Size(120, 36),
                Location = new Point(40, y),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FillColor = Color.FromArgb(30, 30, 50),
                ForeColor = Color.White,
                BorderRadius = 8
            };
            btnSave.Click += (s, e) =>
            {
                if (cmbCase.SelectedIndex < 0 || string.IsNullOrWhiteSpace(txtAmount.Text)) return;
                if (!decimal.TryParse(txtAmount.Text, out decimal amount)) return;

                var selectedCase = cases[cmbCase.SelectedIndex];
                var fee = new Fee
                {
                    CaseId = selectedCase.Id,
                    ClientId = selectedCase.ClientId,
                    FeeType = cmbType.SelectedItem?.ToString(),
                    TotalAmount = amount,
                    DueDate = DateOnly.FromDateTime(dtpDue.Value),
                    Notes = txtNotes.Text,
                    CreatedBy = AuthService.CurrentUser?.Id ?? 1
                };
                _repo.AddFee(fee);
                dlg.Close();
                BuildUI();
            };
            dlg.Controls.Add(btnSave);
            dlg.ShowDialog();
        }

        private void ShowAddPaymentDialog(Fee fee)
        {
            var dlg = new Form
            {
                Text = $"تسجيل دفعة — القضية {fee.CaseNumber}",
                Size = new Size(420, 380),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackColor = Color.White,
                RightToLeft = RightToLeft.Yes
            };

            int y = 20;
            AddField(dlg, $"المتبقي: {fee.Remaining:N0} ج.م", y); y += 45;

            AddField(dlg, "المبلغ:", y); y += 28;
            var txtAmount = new TextBox { Size = new Size(340, 28), Location = new Point(40, y), Font = new Font("Segoe UI", 10) };
            dlg.Controls.Add(txtAmount); y += 45;

            AddField(dlg, "طريقة الدفع:", y); y += 28;
            var cmbMethod = new ComboBox { Size = new Size(340, 28), Location = new Point(40, y), Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var m in new[] { "كاش", "تحويل بنكي", "شيك", "بطاقة", "أخرى" }) cmbMethod.Items.Add(m);
            cmbMethod.SelectedIndex = 0;
            dlg.Controls.Add(cmbMethod); y += 45;

            AddField(dlg, "رقم الإيصال:", y); y += 28;
            var txtReceipt = new TextBox
            {
                Size = new Size(340, 28),
                Location = new Point(40, y),
                Font = new Font("Segoe UI", 10),
                Text = $"REC-{DateTime.Now:yyyy-MM}-{new Random().Next(100, 999)}"
            };
            dlg.Controls.Add(txtReceipt); y += 45;

            AddField(dlg, "ملاحظات:", y); y += 28;
            var txtNotes = new TextBox { Size = new Size(340, 28), Location = new Point(40, y), Font = new Font("Segoe UI", 10) };
            dlg.Controls.Add(txtNotes); y += 50;

            var btnSave = new Guna2Button
            {
                Text = "+  تسجيل الدفعة",
                Size = new Size(150, 36),
                Location = new Point(40, y),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FillColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                BorderRadius = 8
            };
            btnSave.Click += (s, e) =>
            {
                if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0) return;

                var payment = new ActualPayment
                {
                    FeeId = fee.Id,
                    Amount = amount,
                    PaymentDate = DateOnly.FromDateTime(DateTime.Now),
                    Method = cmbMethod.SelectedItem?.ToString(),
                    ReceiptNumber = txtReceipt.Text,
                    ReceivedBy = AuthService.CurrentUser?.Id ?? 1,
                    Notes = txtNotes.Text
                };
                _repo.AddPayment(payment);
                dlg.Close();
                BuildUI();
            };
            dlg.Controls.Add(btnSave);
            dlg.ShowDialog();
        }

        private void AddField(Form form, string label, int y)
        {
            form.Controls.Add(new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(40, y),
                Size = new Size(360, 20),
                TextAlign = ContentAlignment.MiddleRight
            });
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
}

