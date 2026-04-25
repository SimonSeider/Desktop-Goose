using System.Drawing;
using System.Windows.Forms;
using SamEngine;

namespace GooseDesktop.Refactor
{
    internal class EscToQuitOverlay
    {
        private static float curQuitAlpha = 0f;


        public static void UpdateAndDraw(Graphics g)
        {
            if (Program.GetAsyncKeyState(Keys.Escape) != 0)
            {
                curQuitAlpha += 0.0021666668f;
            }
            else
            {
                curQuitAlpha -= 0.016666668f;
            }
            curQuitAlpha = SamMath.Clamp(curQuitAlpha, 0f, 1f);
            if (curQuitAlpha > 0.05f)
            {
                float num = (curQuitAlpha - 0.1f) / 0.9f;
                int num2 = (int)SamMath.Lerp(-15f, 10f, Easings.ExponentialEaseOut(num * 2f));
                SizeF sizeF = g.MeasureString("Continue Holding ESC to evict goose", showCurQuitFont, int.MaxValue);
                g.FillRectangle(Brushes.LightBlue, new Rectangle(5, num2 - 5, (int)sizeF.Width + 10, (int)sizeF.Height + 10));
                g.FillRectangle(Brushes.LightPink, new Rectangle(5, num2 - 5, (int)SamMath.Lerp(0f, sizeF.Width + 10f, num), (int)sizeF.Height + 10));
                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, (int)(256f * curQuitAlpha), (int)(256f * curQuitAlpha), (int)(256f * curQuitAlpha)));
                g.DrawString("Continue holding ESC to evict goose", showCurQuitFont, solidBrush, 10f, (float)num2);
                solidBrush.Dispose();
            }
            if (curQuitAlpha > 0.99f)
            {
                OSFunctions.ClearCursorClip();
                Application.Exit();
            }
        }

        private static Font showCurQuitFont = new Font("Arial", 12f, FontStyle.Bold);
    }
}
