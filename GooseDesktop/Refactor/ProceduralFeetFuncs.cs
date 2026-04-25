using GooseShared;
using SamEngine;

namespace GooseDesktop.Refactor
{
    public static class ProceduralFeetFuncs
    {
        public static void SolveFeets(ProceduralFeets f, Vector2 centerPos, float forwardDirection, float stepTime, int feetDistanceApart, bool isTrackingMud, GooseShared.FootMark[] footMarksArray, ref int footMarkIndex)
        {
            Vector2.GetFromAngleDegrees(forwardDirection);
            Vector2.GetFromAngleDegrees(forwardDirection + 90f);
            Vector2 footHome = GetFootHome(centerPos, forwardDirection, feetDistanceApart, false);
            Vector2 footHome2 = GetFootHome(centerPos, forwardDirection, feetDistanceApart, true);
            if (f.lFootMoveTimeStart < 0f && f.rFootMoveTimeStart < 0f)
            {
                if (Vector2.Distance(f.lFootPos, footHome) > 5f)
                {
                    f.lFootMoveOrigin = f.lFootPos;
                    f.lFootMoveDir = Vector2.Normalize(footHome - f.lFootPos);
                    f.lFootMoveTimeStart = Time.time;
                    return;
                }
                if (Vector2.Distance(f.rFootPos, footHome2) > 5f)
                {
                    f.rFootMoveOrigin = f.rFootPos;
                    f.rFootMoveDir = Vector2.Normalize(footHome2 - f.rFootPos);
                    f.rFootMoveTimeStart = Time.time;
                    return;
                }
            }
            else if (f.lFootMoveTimeStart > 0f)
            {
                Vector2 vector = footHome + f.lFootMoveDir * 0.4f * 5f;
                if (Time.time <= f.lFootMoveTimeStart + stepTime)
                {
                    float num = (Time.time - f.lFootMoveTimeStart) / stepTime;
                    f.lFootPos = Vector2.Lerp(f.lFootMoveOrigin, vector, Easings.CubicEaseInOut(num));
                    return;
                }
                f.lFootPos = vector;
                f.lFootMoveTimeStart = -1f;
                Sound.PlayPat();
                if (isTrackingMud)
                {
                    GooseFunctions.AddFootMark(f.lFootPos, footMarksArray, ref footMarkIndex);
                    return;
                }
            }
            else if (f.rFootMoveTimeStart > 0f)
            {
                Vector2 vector2 = footHome2 + f.rFootMoveDir * 0.4f * 5f;
                if (Time.time > f.rFootMoveTimeStart + stepTime)
                {
                    f.rFootPos = vector2;
                    f.rFootMoveTimeStart = -1f;
                    Sound.PlayPat();
                    if (isTrackingMud)
                    {
                        GooseFunctions.AddFootMark(f.rFootPos, footMarksArray, ref footMarkIndex);
                        return;
                    }
                }
                else
                {
                    float num2 = (Time.time - f.rFootMoveTimeStart) / stepTime;
                    f.rFootPos = Vector2.Lerp(f.rFootMoveOrigin, vector2, Easings.CubicEaseInOut(num2));
                }
            }
        }

        public static Vector2 GetFootHome(Vector2 centerPosition, float direction, int feetDistanceApart, bool rightFoot)
        {
            float num = (float)(rightFoot ? 1 : 0);
            Vector2 vector = Vector2.GetFromAngleDegrees(direction + 90f) * num;
            return centerPosition + vector * (float)feetDistanceApart;
        }
    }
}
