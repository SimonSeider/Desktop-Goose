using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GooseDesktop.Refactor.GooseTasks;
using GooseShared;
using SamEngine;

namespace GooseDesktop.Refactor
{
    internal static class GooseFunctions
    {
        public static GooseEntity InitGoose()
        {
            Rectangle desktopBounds = Program.GetDesktopBounds();
            GooseEntity gooseEntity = new GooseEntity(new GooseEntity.TickFunction(TickGoose), new GooseEntity.UpdateRigFunction(UpdateRig), new GooseEntity.RenderFunction(RenderGoose));
            gooseEntity.parameters = new GooseEntity.ParametersTable();
            gooseEntity.position = new Vector2((float)(desktopBounds.Left - 20), (float)(desktopBounds.Top + 120));
            gooseEntity.targetPos = new Vector2((float)(desktopBounds.Left + 100), (float)(desktopBounds.Top + 150));
            SetSpeed(gooseEntity, GooseEntity.SpeedTiers.Walk);
            gooseEntity.rig.feets = new ProceduralFeets();
            gooseEntity.rig.feets.lFootPos = ProceduralFeetFuncs.GetFootHome(gooseEntity.position, gooseEntity.direction, gooseEntity.rig.feets.feetDistanceApart, false);
            gooseEntity.rig.feets.rFootPos = ProceduralFeetFuncs.GetFootHome(gooseEntity.position, gooseEntity.direction, gooseEntity.rig.feets.feetDistanceApart, true);
            gooseEntity.renderData = new GooseRenderData();
            gooseEntity.renderData.shadowBitmap = new Bitmap(2, 2);
            gooseEntity.renderData.shadowBitmap.SetPixel(0, 0, Color.Transparent);
            gooseEntity.renderData.shadowBitmap.SetPixel(1, 1, Color.Transparent);
            gooseEntity.renderData.shadowBitmap.SetPixel(1, 0, Color.Transparent);
            gooseEntity.renderData.shadowBitmap.SetPixel(0, 1, Color.DarkGray);
            gooseEntity.renderData.shadowBrush = new TextureBrush(gooseEntity.renderData.shadowBitmap);
            gooseEntity.renderData.shadowPen = new Pen(gooseEntity.renderData.shadowBrush);
            gooseEntity.renderData.shadowPen.StartCap = (gooseEntity.renderData.shadowPen.EndCap = LineCap.Round);
            gooseEntity.renderData.DrawingPen = new Pen(Brushes.White);
            gooseEntity.renderData.DrawingPen.EndCap = (gooseEntity.renderData.DrawingPen.StartCap = LineCap.Round);
            if (GooseConfig.settings.UseCustomColors)
            {
                gooseEntity.renderData.brushGooseWhite = new SolidBrush(ColorTranslator.FromHtml(GooseConfig.settings.GooseDefaultWhite));
                gooseEntity.renderData.brushGooseOrange = new SolidBrush(ColorTranslator.FromHtml(GooseConfig.settings.GooseDefaultOrange));
                gooseEntity.renderData.brushGooseOutline = new SolidBrush(ColorTranslator.FromHtml(GooseConfig.settings.GooseDefaultOutline));
            }
            else
            {
                gooseEntity.renderData.brushGooseWhite = Brushes.White as SolidBrush;
                gooseEntity.renderData.brushGooseOrange = Brushes.Orange as SolidBrush;
                gooseEntity.renderData.brushGooseOutline = Brushes.LightGray as SolidBrush;
            }
            gooseEntity.tick = new GooseEntity.TickFunction(TickGoose);
            gooseEntity.updateRig = new GooseEntity.UpdateRigFunction(UpdateRig);
            gooseEntity.render = new GooseEntity.RenderFunction(RenderGoose);
            SetTaskByID(gooseEntity, "Wander", true);
            return gooseEntity;
        }

        public static void TickGoose(GooseEntity g)
        {
            if (GooseConfig.settings.Task_CanAttackMouse && Input.leftMouseButton.Clicked && Vector2.Distance(g.position + new Vector2(0f, 14f), new Vector2((float)Input.mouseX, (float)Input.mouseY)) < 30f)
            {
                SetTaskByID(g, "NabMouse", true);
            }
            g.targetDirection = Vector2.Normalize(g.position - g.targetPos);
            g.extendingNeck = false;
            RunAI(g);
            Vector2 vector = Vector2.Lerp(Vector2.GetFromAngleDegrees(g.direction), g.targetDirection, -0.25f);
            g.direction = (float)Math.Atan2((double)vector.y, (double)vector.x) * 57.295776f;
            g.velocity = Vector2.ClampMagnitude(g.velocity, g.currentSpeed);
            if (g.canDecelerateImmediately && Vector2.Distance(g.position, g.targetPos) < g.parameters.StopRadius)
            {
                g.velocity = Vector2.Lerp(g.velocity, Vector2.zero, Vector2.Distance(g.position, g.targetPos) / g.parameters.StopRadius);
            }
            else
            {
                g.velocity += Vector2.Normalize(g.targetPos - g.position) * g.currentAcceleration * 0.008333334f;
            }
            g.position += g.velocity * 0.008333334f;
            ProceduralFeetFuncs.SolveFeets(g.rig.feets, g.position, g.direction, g.stepInterval, g.rig.feets.feetDistanceApart, Time.time < g.trackMudEndTime, g.footMarks, ref g.footMarkIndex);
            Vector2.Magnitude(g.velocity);
            int num = ((g.extendingNeck | (g.currentSpeed >= g.parameters.RunSpeed)) ? 1 : 0);
            g.rig.neckLerpPercent = SamMath.Lerp(g.rig.neckLerpPercent, (float)num, 0.075f);
        }

        public static void RenderGoose(GooseEntity g, Graphics gfx)
        {
            for (int i = 0; i < g.footMarks.Length; i++)
            {
                if (g.footMarks[i].time != 0f)
                {
                    float num = g.footMarks[i].time + 8.5f;
                    float num2 = SamMath.Clamp(Time.time - num, 0f, 1f) / 1f;
                    float num3 = SamMath.Lerp(3f, 0f, num2);
                    RenderFuncs.FillCircleFromCenter(gfx, Brushes.SaddleBrown, g.footMarks[i].position, (int)num3);
                }
            }
            float direction = g.direction;
            int num4 = (int)g.position.x;
            int num5 = (int)g.position.y;
            Vector2 vector = new Vector2((float)num4, (float)num5);
            Vector2 vector2 = new Vector2(1.3f, 0.4f);
            Vector2 fromAngleDegrees = Vector2.GetFromAngleDegrees(direction);
            Vector2 fromAngleDegrees2 = Vector2.GetFromAngleDegrees(direction + 90f);
            Vector2 vector3 = new Vector2(0f, -1f);
            int num6 = 2;
            g.renderData.DrawingPen.Brush = g.renderData.brushGooseWhite;
            RenderFuncs.FillCircleFromCenter(gfx, g.renderData.brushGooseOrange, g.rig.feets.lFootPos, 4);
            RenderFuncs.FillCircleFromCenter(gfx, g.renderData.brushGooseOrange, g.rig.feets.rFootPos, 4);
            RenderFuncs.FillEllipseFromCenter(gfx, g.renderData.shadowBrush, (int)vector.x, (int)vector.y, 20, 15);
            g.renderData.DrawingPen.Color = g.renderData.brushGooseOutline.Color;
            g.renderData.DrawingPen.Width = (float)(22 + num6);
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.bodyCenter + fromAngleDegrees * 11f), RenderFuncs.ToIntPoint(g.rig.bodyCenter - fromAngleDegrees * 11f));
            g.renderData.DrawingPen.Width = (float)(13 + num6);
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.neckBase), RenderFuncs.ToIntPoint(g.rig.neckHeadPoint));
            g.renderData.DrawingPen.Width = (float)(15 + num6);
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.neckHeadPoint), RenderFuncs.ToIntPoint(g.rig.head1EndPoint));
            g.renderData.DrawingPen.Width = (float)(10 + num6);
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.head1EndPoint), RenderFuncs.ToIntPoint(g.rig.head2EndPoint));
            g.renderData.DrawingPen.Color = g.renderData.brushGooseOutline.Color;
            g.renderData.DrawingPen.Width = 15f;
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.underbodyCenter + fromAngleDegrees * 7f), RenderFuncs.ToIntPoint(g.rig.underbodyCenter - fromAngleDegrees * 7f));
            g.renderData.DrawingPen.Color = g.renderData.brushGooseWhite.Color;
            g.renderData.DrawingPen.Width = 22f;
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.bodyCenter + fromAngleDegrees * 11f), RenderFuncs.ToIntPoint(g.rig.bodyCenter - fromAngleDegrees * 11f));
            g.renderData.DrawingPen.Width = 13f;
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.neckBase), RenderFuncs.ToIntPoint(g.rig.neckHeadPoint));
            g.renderData.DrawingPen.Width = 15f;
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.neckHeadPoint), RenderFuncs.ToIntPoint(g.rig.head1EndPoint));
            g.renderData.DrawingPen.Width = 10f;
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.head1EndPoint), RenderFuncs.ToIntPoint(g.rig.head2EndPoint));
            int num7 = 9;
            int num8 = 3;
            g.renderData.DrawingPen.Width = (float)num7;
            g.renderData.DrawingPen.Brush = g.renderData.brushGooseOrange;
            Vector2 vector4 = g.rig.head2EndPoint + fromAngleDegrees * (float)num8;
            gfx.DrawLine(g.renderData.DrawingPen, RenderFuncs.ToIntPoint(g.rig.head2EndPoint), RenderFuncs.ToIntPoint(vector4));
            Vector2 vector5 = g.rig.neckHeadPoint + vector3 * 3f + -fromAngleDegrees2 * vector2 * 5f + fromAngleDegrees * 5f;
            Vector2 vector6 = g.rig.neckHeadPoint + vector3 * 3f + fromAngleDegrees2 * vector2 * 5f + fromAngleDegrees * 5f;
            RenderFuncs.FillCircleFromCenter(gfx, Brushes.Black, vector5, 2);
            RenderFuncs.FillCircleFromCenter(gfx, Brushes.Black, vector6, 2);
        }

        public static void UpdateRig(Rig rig, Vector2 position, float direction)
        {
            int num = (int)position.x;
            int num2 = (int)position.y;
            Vector2 vector = new Vector2((float)num, (float)num2);
            Vector2 fromAngleDegrees = Vector2.GetFromAngleDegrees(direction);
            Vector2 vector3 = new Vector2(0f, -1f);
            rig.underbodyCenter = vector + vector3 * 9f;
            rig.bodyCenter = vector + vector3 * 14f;
            int num3 = (int)SamMath.Lerp(20f, 10f, rig.neckLerpPercent);
            int num4 = (int)SamMath.Lerp(3f, 16f, rig.neckLerpPercent);
            rig.neckCenter = vector + vector3 * (float)(14 + num3);
            rig.neckBase = rig.bodyCenter + fromAngleDegrees * 15f;
            rig.neckHeadPoint = rig.neckBase + fromAngleDegrees * (float)num4 + vector3 * (float)num3;
            rig.head1EndPoint = rig.neckHeadPoint + fromAngleDegrees * 3f - vector3 * 1f;
            rig.head2EndPoint = rig.head1EndPoint + fromAngleDegrees * 5f;
        }

        public static void RunAI(GooseEntity g)
        {
            GooseTaskDatabase.GetTask(g.currentTask).RunTask(g);
        }

        public static void SetTaskByID(GooseEntity g, string id, bool honck = true)
        {
            if (honck)
            {
                Sound.HONCC();
            }
            int taskIndexByID = GooseTaskDatabase.GetTaskIndexByID(id);
            if (taskIndexByID == -1)
            {
                MessageBox.Show("Cannot set task by an ID that's not registered in the database.", "Set Task Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            g.currentTask = taskIndexByID;
            g.currentTaskData = GooseTaskDatabase.GetTask(g.currentTask).GetNewTaskData(g);
        }

        public static void ChooseRandomTask(GooseEntity g)
        {
            g.currentTask = GooseTaskDatabase.GetNextRandomTask();
            g.currentTaskData = GooseTaskDatabase.GetTask(g.currentTask).GetNewTaskData(g);
        }

        public static void SetTaskDefault(GooseEntity g)
        {
            SetTaskByID(g, "Wander", false);
        }

        public static void SetSpeed(GooseEntity g, GooseEntity.SpeedTiers tier)
        {
            switch (tier)
            {
                case GooseEntity.SpeedTiers.Walk:
                    g.currentSpeed = g.parameters.WalkSpeed;
                    g.currentAcceleration = g.parameters.AccelerationNormal;
                    g.stepInterval = g.parameters.StepTimeNormal;
                    return;
                case GooseEntity.SpeedTiers.Run:
                    g.currentSpeed = g.parameters.RunSpeed;
                    g.currentAcceleration = g.parameters.AccelerationNormal;
                    g.stepInterval = g.parameters.StepTimeNormal;
                    return;
                case GooseEntity.SpeedTiers.Charge:
                    g.currentSpeed = g.parameters.ChargeSpeed;
                    g.currentAcceleration = g.parameters.AccelerationCharged;
                    g.stepInterval = g.parameters.StepTimeCharged;
                    return;
                default:
                    return;
            }
        }

        public static ScreenDirection SetTargetOffscreen(GooseEntity g, bool canExitTop = false)
        {
            Rectangle desktopBounds = Program.GetDesktopBounds();
            int num = (int)g.position.x;
            ScreenDirection screenDirection = ScreenDirection.Left;
            float centerX = (float)(desktopBounds.Left + desktopBounds.Width / 2);
            float centerY = (float)(desktopBounds.Top + desktopBounds.Height / 2);
            g.targetPos = new Vector2((float)(desktopBounds.Left - 50), SamMath.Lerp(g.position.y, centerY, 0.4f));
            if (num > desktopBounds.Left + desktopBounds.Width / 2)
            {
                num = desktopBounds.Right - (int)g.position.x;
                screenDirection = ScreenDirection.Right;
                g.targetPos = new Vector2((float)(desktopBounds.Right + 50), SamMath.Lerp(g.position.y, centerY, 0.4f));
            }
            float distanceToTop = g.position.y - (float)desktopBounds.Top;
            if (canExitTop && (float)num > distanceToTop)
            {
                screenDirection = ScreenDirection.Top;
                g.targetPos = new Vector2(SamMath.Lerp(g.position.x, centerX, 0.4f), (float)(desktopBounds.Top - 50));
            }
            return screenDirection;
        }

        public static void AddFootMark(Vector2 markPos, GooseShared.FootMark[] footMarks, ref int footMarkIndex)
        {
            footMarks[footMarkIndex].time = Time.time;
            footMarks[footMarkIndex].position = markPos;
            footMarkIndex++;
            if (footMarkIndex >= footMarks.Length)
            {
                footMarkIndex = 0;
            }
        }

        public static bool IsGooseAtTarget(GooseEntity g, float distanceToTrigger)
        {
            return Vector2.Distance(g.position, g.targetPos) < distanceToTrigger;
        }

        public static float GetDistanceToTarget(GooseEntity g)
        {
            return Vector2.Distance(g.position, g.targetPos);
        }
    }
}
