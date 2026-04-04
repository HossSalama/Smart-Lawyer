using Guna.UI2.WinForms;
using Smart_Lawyer;
using SmartLawyerFinal.BLL.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartLawyerFinal.Forms
{
    public partial class FrmLogin : Form
    {
        private readonly AuthService _auth;

        // Controls
        private Panel? pnlBackground;
        private Panel pnlCard;
        private Panel pnlLogo;
        private Label lblLogoIcon;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblEmail;
        private Label lblPassword;
        private Guna2TextBox txtEmail;
        private Guna2TextBox txtPassword;
        private Guna2Button btnLogin;
        private Label lblHint;

        public FrmLogin()
        {
            InitializeComponent();
            _auth = new AuthService();
            BuildUI();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
        private void BuildUI()
        {
            // ── Form ──────────────────────────────────────
            this.Text = "Smart Lawyer — تسجيل الدخول";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(18, 32, 55);

            // ── Background Panel ──────────────────────────
            pnlBackground = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(18, 32, 55)
            };

            // ── Card ──────────────────────────────────────
            pnlCard = new Panel
            {
                Size = new Size(440, 520),
                BackColor = Color.White,
                Location = new Point(330, 80)
            };
            // Rounded Card
            var region = new System.Drawing.Drawing2D.GraphicsPath();
            region.AddArc(0, 0, 20, 20, 180, 90);
            region.AddArc(pnlCard.Width - 20, 0, 20, 20, 270, 90);
            region.AddArc(pnlCard.Width - 20, pnlCard.Height - 20, 20, 20, 0, 90);
            region.AddArc(0, pnlCard.Height - 20, 20, 20, 90, 90);
            region.CloseAllFigures();
            pnlCard.Region = new Region(region);

            // ── Logo Circle ───────────────────────────────
            pnlLogo = new Panel
            {
                Size = new Size(72, 72),
                Location = new Point(184, 30),
                BackColor = Color.FromArgb(230, 81, 0)
            };
            var logoRegion = new System.Drawing.Drawing2D.GraphicsPath();
            logoRegion.AddEllipse(0, 0, 72, 72);
            pnlLogo.Region = new Region(logoRegion);

            lblLogoIcon = new Label
            {
                Text = "⚖",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(12, 12),
                Size = new Size(48, 48),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlLogo.Controls.Add(lblLogoIcon);

            // ── Title ─────────────────────────────────────
            lblTitle = new Label
            {
                Text = "نظام إدارة القضايا",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 20, 40),
                Location = new Point(60, 118),
                Size = new Size(320, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ── Subtitle ──────────────────────────────────
            lblSubtitle = new Label
            {
                Text = "Legal Management System",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(110, 158),
                Size = new Size(220, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ── Email Label ───────────────────────────────
            lblEmail = new Label
            {
                Text = "البريد الإلكتروني",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(30, 205),
                Size = new Size(380, 22),
                TextAlign = ContentAlignment.MiddleRight
            };

            // ── Email TextBox (Guna) ──────────────────────
            txtEmail = new Guna2TextBox
            {
                Location = new Point(30, 230),
                Size = new Size(380, 42),
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "example@law.com",
                BorderRadius = 8,
                BorderColor = Color.FromArgb(200, 200, 200),
                FillColor = Color.FromArgb(248, 248, 250)
            };

            // ── Password Label ────────────────────────────
            lblPassword = new Label
            {
                Text = "كلمة المرور",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(30, 285),
                Size = new Size(380, 22),
                TextAlign = ContentAlignment.MiddleRight
            };

            // ── Password TextBox (Guna) ───────────────────
            txtPassword = new Guna2TextBox
            {
                Location = new Point(30, 310),
                Size = new Size(380, 42),
                Font = new Font("Segoe UI", 11),
                PlaceholderText = "••••••••",
                UseSystemPasswordChar = true,
                BorderRadius = 8,
                BorderColor = Color.FromArgb(200, 200, 200),
                FillColor = Color.FromArgb(248, 248, 250)
            };

            // ── Login Button (Guna) ───────────────────────
            btnLogin = new Guna2Button
            {
                Text = "تسجيل الدخول",
                Location = new Point(30, 375),
                Size = new Size(380, 48),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                FillColor = Color.FromArgb(230, 81, 0),
                ForeColor = Color.White,
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };
            btnLogin.Click += BtnLogin_Click;

            // ── Hint ──────────────────────────────────────
            lblHint = new Label
            {
                Text = "للتجربة: admin@law.com / password",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(30, 440),
                Size = new Size(380, 22),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ── Enter Key ─────────────────────────────────
            txtPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) btnLogin.PerformClick();
            };

            // ── Add to Card ───────────────────────────────
            pnlCard.Controls.AddRange(new Control[]
            {
                pnlLogo, lblTitle, lblSubtitle,
                lblEmail, txtEmail,
                lblPassword, txtPassword,
                btnLogin, lblHint
            });

            // ── Add to Form ───────────────────────────────
            this.Controls.Add(pnlCard);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            btnLogin.Text = "جاري التحقق...";

            bool success = _auth.Login(
                txtEmail.Text.Trim(),
                txtPassword.Text,
                out string message);

            if (success)
            {
                var main = new mainForm();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(message, "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnLogin.Enabled = true;
                btnLogin.Text = "تسجيل الدخول";
            }
        }
    }
}
