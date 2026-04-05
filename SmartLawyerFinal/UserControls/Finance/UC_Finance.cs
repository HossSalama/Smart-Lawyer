using SmartLawyerFinal.BLL.Services.ClassSevice;
using SmartLawyerFinal.DAL.Repositories.ClassRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartLawyerFinal.UserControls.Finance
{

    public partial class UC_Finance : UserControl
    {
        private Panel _pnlScroll;
        private FlowLayoutPanel _flowCards;
        private Guna.UI2.WinForms.Guna2Button _btnAdd;
        private FinanceService _financeService; 
        public UC_Finance()
        {
            var context = new LegalManagementContext();
            var repo = new FinanceRepository(context);
            _financeService = new FinanceService(repo);
            InitializeComponent();
            this.Load += UC_Finance_Load;
            SetupLayout();
        }
        private async void UC_Finance_Load(object sender, EventArgs e)
        {
            await LoadDashboardDataAsync();
        }
        private void SetupLayout()
        {
            this.Controls.Clear();

            int W = 1100;

            var pnlHeader = CreateHeader(W,
                "الأتعاب والدفعات",
                "إدارة أتعاب القضايا ومتابعة الدفعات",
                "تسجيل دفعة جديدة +",
                () => ShowAddFeeDialog()
            );
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 140;
            pnlHeader.BackColor = Color.FromArgb(245, 245, 250);
            this.Controls.Add(pnlHeader);
            _pnlScroll= new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(245, 245, 250),
                Padding = new Padding(0, 120, 0, 0)
            };

            this.Controls.Add(_pnlScroll);
         

            pnlHeader.BringToFront();

            _flowCards= new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Height = 180,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10),
                BackColor = Color.Transparent,
                WrapContents = true
            };
            var searchArea = CreateSearchArea();
            _pnlScroll.Controls.Add(searchArea);
            _pnlScroll.Controls.Add(_flowCards);
            _flowCards.SendToBack(); 
            searchArea.BringToFront();
            UpdateCardsScale();
            this.SizeChanged += (s, e) => UpdateCardsScale();
        }

        private Panel CreateSearchArea()
        {
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(25),
                BackColor = Color.Transparent
            };

            var txtSearch = new Guna.UI2.WinForms.Guna2TextBox
            {
                PlaceholderText = "ابحث باسم العميل أو رقم القضية...",
                BorderRadius = 10,
                Font = new Font("Segoe UI", 11),
                TextAlign = HorizontalAlignment.Right,
                FillColor = Color.White,
                BorderColor = Color.FromArgb(210, 210, 220)
            };

            var btnSearch = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "بحث",
                BorderRadius = 10,
                FillColor = Color.FromArgb(30, 30, 50),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };

            void ApplyResponsiveLayout()
            {
                int totalAvailableWidth = pnlSearch.Width - 40;

                txtSearch.Width = (int)(totalAvailableWidth * 0.70);
                btnSearch.Width = (int)(totalAvailableWidth * 0.20);

                txtSearch.Height = 42;
                btnSearch.Height = 42;

                txtSearch.Left = pnlSearch.Width - txtSearch.Width - 20; 

                int gap = (int)(totalAvailableWidth * 0.05);
                btnSearch.Left = txtSearch.Left - btnSearch.Width - gap;

                txtSearch.Top = (pnlSearch.Height - txtSearch.Height) / 2;
                btnSearch.Top = txtSearch.Top;
            }

            pnlSearch.SizeChanged += (s, e) => ApplyResponsiveLayout();

            btnSearch.Click += (s, e) => {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    MessageBox.Show($"جاري البحث عن: {txtSearch.Text}", "البحث");
            };

            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(btnSearch);

            ApplyResponsiveLayout();

            return pnlSearch;
        }
        private async Task LoadDashboardDataAsync()
        {
            try
            {
                var data = await _financeService.GetDashboardSummaryAsync();
                _flowCards.Controls.Clear();

                _flowCards.Controls.Add(CreateSingleCard("إجمالي الأتعاب", data.TotalAmount.ToString("N0"), Color.Teal, "💰"));
                _flowCards.Controls.Add(CreateSingleCard("التحويلات", data.PaidAmount.ToString("N0"), Color.Orange, "💳"));
                _flowCards.Controls.Add(CreateSingleCard("الأقساط المتأخرة", data.overDueAmount.ToString("N0"), Color.Crimson, "⚠️"));
                _flowCards.Controls.Add(CreateSingleCard("دفعات قادمة", data.UpcomingAmount.ToString("N0"), Color.RoyalBlue, "📅"));

                UpdateCardsScale();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل البيانات: {ex.Message}");
            }
        }
     
        private void UpdateCardsScale()
        {
            if (_flowCards == null || _pnlScroll == null || this.IsDisposed) return;
            int availableWidth = _pnlScroll.ClientSize.Width - 40;
            int cardWidth = availableWidth / 4;
            if (cardWidth < 150) cardWidth = 150;
            foreach (Control ctrl in _flowCards.Controls)
            {
                ctrl.Width = cardWidth - 30;
            }
            _flowCards.Width = _pnlScroll.ClientSize.Width;
            _flowCards.PerformLayout();
        }
        private Guna.UI2.WinForms.Guna2ShadowPanel CreateSingleCard(string title, string value, Color accentColor , string emojiIcon)
        {
            var card = new Guna.UI2.WinForms.Guna2ShadowPanel
            {
                Size = new Size(150, 150),
                Radius = 10,
                FillColor = Color.White,
                ShadowColor = Color.Black, 
                ShadowDepth = 50,
                Margin = new Padding(15)
            };
            var lblIcon = new Label
            {
                Text = emojiIcon, 
                Font = new Font("Segoe UI Emoji", 22), 
                Location = new Point(15, 30), 
                Size = new Size(55, 55),
                BackColor = Color.Transparent,
                ForeColor = accentColor, 
                TextAlign = ContentAlignment.MiddleCenter
            };
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                Location = new Point(20, 15),
                Size = new Size(card.Width - 40, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, 
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(10, 50),
                Size = new Size(card.Width - 40, 45),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, // مهم جداً
                TextAlign = ContentAlignment.MiddleRight
            };
            card.Controls.Add(lblIcon);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }
        private Panel CreateHeader(int W, string title, string subTitle, string btnText, Action btnAction)
        {
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 140,
                BackColor = Color.Transparent
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Size = new Size(400, 42),
                Location = new Point(pnlHeader.Width - 420, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleRight
            };

            var lblSub = new Label
            {
                Text = subTitle,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Size = new Size(400, 24),
                Location = new Point(pnlHeader.Width - 420, 70),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleRight
            };

            _btnAdd = new Guna.UI2.WinForms.Guna2Button
            {
                Text = btnText,
                Size = new Size(170, 38),
                Location = new Point(40, 45), 
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FillColor = Color.FromArgb(30, 30, 50),
                ForeColor = Color.White,
                BorderRadius = 8,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left 
            };
            _btnAdd.Click += (s, e) => btnAction?.Invoke();

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblSub, _btnAdd });

            pnlHeader.SizeChanged += (s, e) => {
                lblTitle.Left = pnlHeader.Width - lblTitle.Width - 20;
                lblSub.Left = pnlHeader.Width - lblSub.Width - 20;
            };

            return pnlHeader;
        }
        private void ShowAddFeeDialog()
        {
            MessageBox.Show("شاشة إضافة دفعة");
        }
    }
}
