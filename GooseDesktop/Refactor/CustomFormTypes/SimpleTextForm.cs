using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GooseShared;
using SamEngine;

namespace GooseDesktop.Refactor.CustomFormTypes
{
    internal partial class SimpleTextForm : MovableForm
    {
        public SimpleTextForm(GooseEntity ownerGoose)
            : base(ownerGoose)
        {
            base.Width = 200;
            base.Height = 150;
            this.Text = "Goose \"Not-epad\"";
            TextBox textBox = new TextBox();
            textBox.Multiline = true;
            textBox.AcceptsReturn = true;
            try
            {
                string[] files = Directory.GetFiles(messagesRootFolder, "*.txt");
                string text = File.ReadAllText(files[(int)SamMath.RandomRange(0f, (float)files.Length)]);
                textBox.Text = text;
            }
            catch
            {
                textBox.Text = possiblePhrases[textIndices.Next()];
            }
            textBox.Location = new Point(0, 0);
            textBox.Width = base.ClientSize.Width;
            textBox.Height = base.ClientSize.Height - 5;
            textBox.Select(textBox.Text.Length, 0);
            textBox.Font = new Font(textBox.Font.FontFamily, 10f, FontStyle.Regular);
            base.Controls.Add(textBox);
            string text2 = Environment.SystemDirectory + "\\notepad.exe";
            if (File.Exists(text2))
            {
                try
                {
                    base.Icon = Icon.ExtractAssociatedIcon(text2);
                    base.ShowIcon = true;
                }
                catch
                {
                }
            }
        }

        private void ExitWindow(object sender, EventArgs args)
        {
            base.Close();
        }

        private static readonly string messagesRootFolder = Program.GetPathToFileInAssembly("Assets/Text/NotepadMessages/");

        private static string[] possiblePhrases = new string[] { "am goose hjonk", "good work", "nsfdafdsaafsdjl\r\nasdas       sorry\r\nhard to type withh feet", "i cause problems on purpose", "\"peace was never an option\"\r\n   -the goose (me)", "\r\n\r\n  >o) \r\n    (_>" };

        private static Deck textIndices = new Deck(possiblePhrases.Length);
    }
}
