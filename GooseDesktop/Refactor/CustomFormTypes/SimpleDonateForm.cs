using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using GooseShared;

namespace GooseDesktop.Refactor.CustomFormTypes
{
    internal partial class SimpleDonateForm : MovableForm
    {
        private const string patreonLink = "https://www.patreon.com/bePatron?u=3541875";

        private const string paypalLink = "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=WUKYHY7SZ275Q&currency_code=USD&source=url";

        private const string twitterLink = "https://www.twitter.com/samnchiet";

        private const string discordLink = "https://discord.gg/xZFRmPT";

        private float scale = 1.25f;

        public SimpleDonateForm(GooseEntity ownerGoose)
            : base(ownerGoose)
        {
            new PictureBox();
            base.ClientSize = new Size((int)(250f * this.scale), (int)(300f * this.scale));
            try
            {
                this.BackgroundImage = Image.FromFile(donationGraphicSrc);
            }
            catch
            {
                Label label = new Label();
                label.Text = "Can't find the donation image... are you messing with the game files?\nCheck out my Twitter at twitter.com/samnchiet I guess?";
                label.Location = new Point(0, 0);
                label.Width = base.ClientSize.Width;
                label.Height = base.ClientSize.Height;
                label.BackColor = Color.White;
                label.TextAlign = ContentAlignment.MiddleCenter;
                base.Controls.Add(label);
            }
            this.BackgroundImageLayout = ImageLayout.Stretch;
            base.Controls.Add(this.SetupButton(111, 407, 390, 475, new EventHandler(this.OpenPatreonLink), true));
            base.Controls.Add(this.SetupButton(174, 500, 325, 545, new EventHandler(this.OpenPaypalLink), true));
            base.Controls.Add(this.SetupButton(381, 302, 433, 360, new EventHandler(this.OpenDiscordLink), true));
            base.Controls.Add(this.SetupButton(403, 247, 472, 312, new EventHandler(this.OpenTwitterLink), true));
        }

        private Button SetupButton(int point1X, int point1Y, int point2X, int point2Y, EventHandler handler, bool showHoverClick = true)
        {
            Button button = new Button();
            button.Location = new Point((int)((float)point1X * this.scale) / 2, (int)((float)point1Y * this.scale) / 2);
            button.Size = new Size((int)((float)(point2X - point1X) * this.scale) / 2, (int)((float)(point2Y - point1Y) * this.scale) / 2);
            button.Click += handler;
            button.Cursor = Cursors.Hand;
            button.BackColor = Color.Transparent;
            button.ForeColor = Color.Transparent;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.MouseOverBackColor = (showHoverClick ? Color.FromArgb(40, Color.White) : Color.Transparent);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, Color.White);
            button.FlatAppearance.BorderSize = 0;
            button.TabStop = false;
            return button;
        }

        private void OpenPatreonLink(object sender, EventArgs a)
        {
            Process.Start("https://www.patreon.com/bePatron?u=3541875");
        }

        private void OpenPaypalLink(object sender, EventArgs a)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=WUKYHY7SZ275Q&currency_code=USD&source=url");
        }

        private void OpenTwitterLink(object sender, EventArgs a)
        {
            Process.Start("https://www.twitter.com/samnchiet");
        }

        private void OpenDiscordLink(object sender, EventArgs a)
        {
            Process.Start("https://discord.gg/xZFRmPT");
        }

        private static string donationGraphicSrc = Program.GetPathToFileInAssembly("Assets/Images/OtherGfx/DonatePage.png");

    }
}
