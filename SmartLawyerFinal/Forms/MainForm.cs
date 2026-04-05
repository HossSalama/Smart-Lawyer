using Guna.UI2.WinForms;
using SmartLawyerFinal.BLL.Services.ClassSevice;
using SmartLawyerFinal.Forms;
using SmartLawyerFinal.UserControls.Dashboard;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SmartLawyerFinal.UserControls.Finance;
using SmartLawyerFinal.UserControls.Reports;


namespace Smart_Lawyer
{
    public partial class mainForm : Form
    {
        private Panel pnlSidebar;
        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlLogoArea;
        private Panel pnlLogoCircle;
        private Label lblLogoIcon;
        private Label lblAppName;
        private Label lblAppSub;
        private Label lblUserName;
        //private Panel pnlUserAvatar;
        //private Label lblAvatarLetter;
        private Guna2Button btnDashboard;
        private Guna2Button btnClients;
        private Guna2Button btnCases;
        private Guna2Button btnHearings;
        private Guna2Button btnHearingStages;
        private Guna2Button btnInvestigations;
        private Guna2Button btnAppeals;
        private Guna2Button btnPOA;
        private Guna2Button btnMemos;
        private Guna2Button btnDocuments;
        private Guna2Button btnAgenda;
        private Guna2Button btnFees;
        private Guna2Button btnLibrary;
        private Guna2Button btnAlerts;
        private Guna2Button btnReports;
        private Guna2Button btnLogout;
        private Guna2Button _activeBtn;
        public mainForm()
        {
            InitializeComponent();
            BuildUI();
            SetActive(btnDashboard);
        }
        private void BuildUI()
        {
            this.Text = "Smart Lawyer — إدارة القضايا";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.MinimumSize = new Size(1000, 650);
            this.AutoScroll = false;
            this.AutoSize = false;

            // Sidebar
            pnlSidebar = new Panel
            {
                Dock = DockStyle.Right,
                Width = 220,
                BackColor = Color.FromArgb(22, 28, 48),
                AutoScroll = true
            };
            pnlSidebar.HorizontalScroll.Enabled = false;
            pnlSidebar.HorizontalScroll.Visible = false;

            // Logo
            pnlLogoArea = new Panel
            {
                Width = 220,
                Height = 80,
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(22, 28, 48)
            };

            pnlLogoCircle = new Panel
            {
                Size = new Size(40, 40),
                Location = new Point(165, 20),
                BackColor = Color.FromArgb(230, 81, 0)
            };
            var lp = new GraphicsPath();
            lp.AddEllipse(0, 0, 40, 40);
            pnlLogoCircle.Region = new Region(lp);
            lblLogoIcon = new Label
            {
                Text = "⚖",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Size = new Size(40, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlLogoCircle.Controls.Add(lblLogoIcon);

            lblAppName = new Label
            {
                Text = "إدارة القضايا",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(10, 18),
                Size = new Size(145, 22),
                TextAlign = ContentAlignment.MiddleRight
            };
            lblAppSub = new Label
            {
                Text = "Legal System",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(150, 150, 180),
                BackColor = Color.Transparent,
                Location = new Point(10, 40),
                Size = new Size(145, 18),
                TextAlign = ContentAlignment.MiddleRight
            };
            pnlLogoArea.Controls.AddRange(new Control[] { pnlLogoCircle, lblAppName, lblAppSub });
            pnlSidebar.Controls.Add(pnlLogoArea);

            // Buttons
            int by = 90;
            btnDashboard = SBtn("لوحة التحكم", "📊", by); by += 51;
            btnClients = SBtn("العملاء", "👤", by); by += 51;
            btnCases = SBtn("القضايا", "⚖", by); by += 51;
            btnHearings = SBtn("الجلسات", "📅", by); by += 51;
            btnHearingStages = SBtn("مراحل الجلسات", "📋", by); by += 51;
            btnInvestigations = SBtn("التحقيقات", "🔍", by); by += 51;
            btnAppeals = SBtn("الاستئناف", "📈", by); by += 51;
            btnPOA = SBtn("التوكيلات", "📝", by); by += 51;
            btnMemos = SBtn("المذكرات", "📄", by); by += 51;
            btnDocuments = SBtn("المستندات", "📁", by); by += 51;
            btnAgenda = SBtn("الأجندة القضائية", "📆", by); by += 51;
            btnFees = SBtn("الأتعاب", "💰", by); by += 51;
            btnLibrary = SBtn("المكتبة القانونية", "📚", by); by += 51;
            btnAlerts = SBtn("التنبيهات", "🔔", by); by += 51;
            btnReports = SBtn("التقارير", "📊", by); by += 51;
            btnLogout = SBtn("تسجيل الخروج", "🚪", by);
            btnLogout.FillColor = Color.FromArgb(180, 40, 40);
            btnLogout.HoverState.FillColor = Color.FromArgb(200, 50, 50);

            pnlSidebar.Controls.AddRange(new Control[]
            {
                btnDashboard, btnClients, btnCases, btnHearings,
                btnHearingStages, btnInvestigations, btnAppeals,
                btnPOA, btnMemos, btnDocuments,
                btnAgenda, btnFees, btnLibrary, btnAlerts,
                btnReports, btnLogout
            });

            btnDashboard.Click += (s, e) => SetActive(btnDashboard);
            btnClients.Click += (s, e) => SetActive(btnClients);
            btnCases.Click += (s, e) => SetActive(btnCases);
            btnHearings.Click += (s, e) => SetActive(btnHearings);
            btnHearingStages.Click += (s, e) => SetActive(btnHearingStages);
            btnInvestigations.Click += (s, e) => SetActive(btnInvestigations);
            btnAppeals.Click += (s, e) => SetActive(btnAppeals);
            btnPOA.Click += (s, e) => SetActive(btnPOA);
            btnMemos.Click += (s, e) => SetActive(btnMemos);
            btnDocuments.Click += (s, e) => SetActive(btnDocuments);
            btnAgenda.Click += (s, e) => SetActive(btnAgenda);
            btnFees.Click += (s, e) => SetActive(btnFees);
            btnLibrary.Click += (s, e) => SetActive(btnLibrary);
            btnAlerts.Click += (s, e) => SetActive(btnAlerts);
            btnReports.Click += (s, e) => SetActive(btnReports);
            btnLogout.Click += (s, e) =>
            {
                AuthService.Logout();
                new FrmLogin().Show();
                this.Close();
            };

            // Header
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 58,
                BackColor = Color.White,
                Padding = new Padding(0)
            };
            pnlHeader.Paint += (s, e) =>
                e.Graphics.DrawLine(
                    new Pen(Color.FromArgb(230, 230, 235)),
                    0, 57, pnlHeader.Width, 57);

            // Username
            lblUserName = new Label
            {
                Text = $"مرحباً، {AuthService.CurrentUser?.FullName ?? "المستخدم"}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 50),
                Size = new Size(260, 30),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            pnlHeader.Layout += (s, e) =>
                lblUserName.Location = new Point(pnlHeader.Width - 270, 14);

            // Notification
            var pnlNotif = new Panel
            {
                Size = new Size(36, 36),
                Location = new Point(18, 11),
                BackColor = Color.FromArgb(240, 240, 246),
                Cursor = Cursors.Hand
            };
            var np = new GraphicsPath();
            np.AddEllipse(0, 0, 36, 36);
            pnlNotif.Region = new Region(np);
            pnlNotif.Controls.Add(new Label
            {
                Text = "🔔",
                Font = new Font("Segoe UI", 14),
                BackColor = Color.Transparent,
                Size = new Size(36, 36),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // Badge
            var pnlBadge = new Panel
            {
                Size = new Size(17, 17),
                Location = new Point(50, 7),
                BackColor = Color.FromArgb(220, 50, 50)
            };
            var bpp = new GraphicsPath();
            bpp.AddEllipse(0, 0, 17, 17);
            pnlBadge.Region = new Region(bpp);
            pnlBadge.Controls.Add(new Label
            {
                Text = "5",
                Font = new Font("Segoe UI", 7, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Size = new Size(17, 17),
                TextAlign = ContentAlignment.MiddleCenter
            });

            // Avatar
            //pnlUserAvatar = new Panel
            //{
            //    Size = new Size(36, 36),
            //    Location = new Point(62, 11),
            //    BackColor = Color.FromArgb(230, 81, 0)
            //};
            //var ap = new GraphicsPath();
            //ap.AddEllipse(0, 0, 36, 36);
            //pnlUserAvatar.Region = new Region(ap);
            //string fl = AuthService.CurrentUser?.FullName?.Substring(0, 1) ?? "م";
            //lblAvatarLetter = new Label
            //{
            //    Text = fl,
            //    Font = new Font("Segoe UI", 13, FontStyle.Bold),
            //    ForeColor = Color.White,
            //    BackColor = Color.Transparent,
            //    Size = new Size(36, 36),
            //    TextAlign = ContentAlignment.MiddleCenter
            //};
            //pnlUserAvatar.Controls.Add(lblAvatarLetter);

            pnlHeader.Controls.AddRange(new Control[]
                { pnlNotif, pnlBadge, lblUserName });

            // Content 
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 250),
                Padding = new Padding(0),
                AutoScroll = false
            };

            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlSidebar);
        }

        private Guna2Button SBtn(string text, string icon, int y)
        {
            var btn = new Guna2Button
            {
                Text = $"  {icon}  {text}",
                Size = new Size(200, 44),
                Location = new Point(10, y),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(180, 180, 210),
                FillColor = Color.Transparent,
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };
            btn.HoverState.FillColor = Color.FromArgb(40, 45, 70);
            return btn;
        }

        private void SetActive(Guna2Button btn)
        {
            if (_activeBtn != null)
            {
                _activeBtn.FillColor = Color.Transparent;
                _activeBtn.ForeColor = Color.FromArgb(180, 180, 210);
            }
            btn.FillColor = Color.FromArgb(230, 81, 0);
            btn.ForeColor = Color.White;
            _activeBtn = btn;
            LoadPage(btn);
        }

        private void LoadPage(Guna2Button btn)
        {
            pnlContent.Controls.Clear();
            UserControl uc = null;

            if (btn == btnDashboard) uc = new UcDashboard();
            if (btn == btnFees) uc = new UC_Finance();
            if (btn == btnReports) uc = new UcReports();

            if (uc != null)
            {
                uc.Dock = DockStyle.Fill;
                uc.Size = new Size(pnlContent.Width, pnlContent.Height);
                pnlContent.Controls.Add(uc);
            }
        }

        public void NavigateTo(UserControl uc)
        {
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
        }

        

        //private void mainForm_Load(object sender, EventArgs e)
        //{
        //    pnlSideBar.Width = (int)(this.Width * 0.20);
        //}
        //public void UpdateHeader(string title, string description, string buttonText, Action buttonAction)
        //{
        //    lblHeadName.Text = $"<div dir='rtl'>{title}</div>";
        //    lblHeadTxtName.Text = $"<div dir='rtl'>{description}</div>"; ;
        //    BtnAdd.Text = buttonText;

        //    BtnAdd.Click -= null;
        //    BtnAdd.Click += (s, e) => buttonAction();
        //}
        //public void AddControlToMainPanel(UserControl userControl)
        //{
        //    userControl.Dock = DockStyle.Fill;
        //    pnlMainContent.Controls.Clear();
        //    pnlMainContent.Controls.Add(userControl);
        //}

        //private void btnFinance_Click(object sender, EventArgs e)
        //{
        //    UC_Financials financialsPage = new UC_Financials();
        //    AddControlToMainPanel(financialsPage);
        //    UpdateHeader("الأتعاب والدفعات", "إدارة أتعاب القضايا ومتابعة الدفعات", "تسجيل دفعة جديدة", () =>
        //    {
        //        MessageBox.Show("سيتم فتح نافذة إضافة سند مالي جديد");
        //    });
        //}

        //private void guna2Button3_Click(object sender, EventArgs e)
        //{

        //}

        //private void pnlMainContent_Paint(object sender, PaintEventArgs e)
        //{

        //}

        //private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        //{
        //}
    }
}
