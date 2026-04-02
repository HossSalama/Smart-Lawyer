namespace Smart_Lawyer
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            pnlSideBar.Width = (int)(this.Width * 0.20);
        }
        public void UpdateHeader(string title, string description, string buttonText, Action buttonAction)
        {
            lblHeadName.Text = $"<div dir='rtl'>{title}</div>";
            lblHeadTxtName.Text = $"<div dir='rtl'>{description}</div>"; ;
            BtnAdd.Text = buttonText;

            BtnAdd.Click -= null;
            BtnAdd.Click += (s, e) => buttonAction();
        }
        public void AddControlToMainPanel(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            pnlMainContent.Controls.Clear();
            pnlMainContent.Controls.Add(userControl);
        }

        private void btnFinance_Click(object sender, EventArgs e)
        {
            UC_Financials financialsPage = new UC_Financials();
            AddControlToMainPanel(financialsPage);
            UpdateHeader(" «·√ ⁄«» Ê«·œ›⁄« ", "≈œ«—… « ⁄«» «·ﬁ÷«Ì« Ê„ «»⁄Â «·œ›⁄«  ", "  ”ÃÌ· œ›⁄Â ÃœÌœÂ ", () =>
            {
                MessageBox.Show("”Ì „ › Õ ‰«›–… ≈÷«›… ”‰œ „«·Ì ÃœÌœ");
            });
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }
    }
}
