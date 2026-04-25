using System.Drawing;
using System.Windows.Forms;
using GooseShared;

namespace GooseDesktop.Refactor.CustomFormTypes
{
    internal partial class MovableForm : Form
    {
        public readonly GooseEntity ownerGoose;

        public MovableForm(GooseEntity owner)
        {
            this.ownerGoose = owner;
            base.StartPosition = FormStartPosition.Manual;
            base.Width = 400;
            base.Height = 400;
            this.BackColor = Color.DimGray;
            base.Icon = null;
            base.ShowIcon = false;
            this.SetWindowResizableThreadsafe(false);
        }

        public void SetWindowPositionThreadsafe(Point p)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new MethodInvoker(delegate
                {
                    this.Location = p;
                    this.TopMost = true;
                }));
                return;
            }
            base.Location = p;
            base.TopMost = true;
        }

        public void SetWindowResizableThreadsafe(bool canResize)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new MethodInvoker(delegate
                {
                    this.FormBorderStyle = (canResize ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle);
                    this.MaximizeBox = (this.MinimizeBox = canResize);
                }));
                return;
            }
            base.FormBorderStyle = (canResize ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle);
            base.MaximizeBox = (base.MinimizeBox = canResize);
        }

    }
}
