using System;

public static class Easings
{
    private const float PI = 3.1415927f;

    private const float HALFPI = 1.5707964f;


    public static float Interpolate(float p, Functions function)
    {
        switch (function)
        {
            default:
                return Linear(p);
            case Functions.QuadraticEaseIn:
                return QuadraticEaseIn(p);
            case Functions.QuadraticEaseOut:
                return QuadraticEaseOut(p);
            case Functions.QuadraticEaseInOut:
                return QuadraticEaseInOut(p);
            case Functions.CubicEaseIn:
                return CubicEaseIn(p);
            case Functions.CubicEaseOut:
                return CubicEaseOut(p);
            case Functions.CubicEaseInOut:
                return CubicEaseInOut(p);
            case Functions.QuarticEaseIn:
                return QuarticEaseIn(p);
            case Functions.QuarticEaseOut:
                return QuarticEaseOut(p);
            case Functions.QuarticEaseInOut:
                return QuarticEaseInOut(p);
            case Functions.QuinticEaseIn:
                return QuinticEaseIn(p);
            case Functions.QuinticEaseOut:
                return QuinticEaseOut(p);
            case Functions.QuinticEaseInOut:
                return QuinticEaseInOut(p);
            case Functions.SineEaseIn:
                return SineEaseIn(p);
            case Functions.SineEaseOut:
                return SineEaseOut(p);
            case Functions.SineEaseInOut:
                return SineEaseInOut(p);
            case Functions.CircularEaseIn:
                return CircularEaseIn(p);
            case Functions.CircularEaseOut:
                return CircularEaseOut(p);
            case Functions.CircularEaseInOut:
                return CircularEaseInOut(p);
            case Functions.ExponentialEaseIn:
                return ExponentialEaseIn(p);
            case Functions.ExponentialEaseOut:
                return ExponentialEaseOut(p);
            case Functions.ExponentialEaseInOut:
                return ExponentialEaseInOut(p);
            case Functions.ElasticEaseIn:
                return ElasticEaseIn(p);
            case Functions.ElasticEaseOut:
                return ElasticEaseOut(p);
            case Functions.ElasticEaseInOut:
                return ElasticEaseInOut(p);
            case Functions.BackEaseIn:
                return BackEaseIn(p);
            case Functions.BackEaseOut:
                return BackEaseOut(p);
            case Functions.BackEaseInOut:
                return BackEaseInOut(p);
            case Functions.BounceEaseIn:
                return BounceEaseIn(p);
            case Functions.BounceEaseOut:
                return BounceEaseOut(p);
            case Functions.BounceEaseInOut:
                return BounceEaseInOut(p);
        }
    }

    public static float Linear(float p)
    {
        return p;
    }

    public static float QuadraticEaseIn(float p)
    {
        return p * p;
    }

    public static float QuadraticEaseOut(float p)
    {
        return -(p * (p - 2f));
    }

    public static float QuadraticEaseInOut(float p)
    {
        if (p < 0.5f)
        {
            return 2f * p * p;
        }
        return -2f * p * p + 4f * p - 1f;
    }

    public static float CubicEaseIn(float p)
    {
        return p * p * p;
    }

    public static float CubicEaseOut(float p)
    {
        float num = p - 1f;
        return num * num * num + 1f;
    }

    public static float CubicEaseInOut(float p)
    {
        if (p < 0.5f)
        {
            return 4f * p * p * p;
        }
        float num = 2f * p - 2f;
        return 0.5f * num * num * num + 1f;
    }

    public static float QuarticEaseIn(float p)
    {
        return p * p * p * p;
    }

    public static float QuarticEaseOut(float p)
    {
        float num = p - 1f;
        return num * num * num * (1f - p) + 1f;
    }

    public static float QuarticEaseInOut(float p)
    {
        if (p < 0.5f)
        {
            return 8f * p * p * p * p;
        }
        float num = p - 1f;
        return -8f * num * num * num * num + 1f;
    }

    public static float QuinticEaseIn(float p)
    {
        return p * p * p * p * p;
    }

    public static float QuinticEaseOut(float p)
    {
        float num = p - 1f;
        return num * num * num * num * num + 1f;
    }

    public static float QuinticEaseInOut(float p)
    {
        if (p < 0.5f)
        {
            return 16f * p * p * p * p * p;
        }
        float num = 2f * p - 2f;
        return 0.5f * num * num * num * num * num + 1f;
    }

    public static float SineEaseIn(float p)
    {
        return (float)Math.Sin((double)((p - 1f) * 1.5707964f)) + 1f;
    }

    public static float SineEaseOut(float p)
    {
        return (float)Math.Sin((double)(p * 1.5707964f));
    }

    public static float SineEaseInOut(float p)
    {
        return 0.5f * (1f - (float)Math.Cos((double)(p * 3.1415927f)));
    }

    public static float CircularEaseIn(float p)
    {
        return 1f - (float)Math.Sqrt((double)(1f - p * p));
    }

    public static float CircularEaseOut(float p)
    {
        return (float)Math.Sqrt((double)((2f - p) * p));
    }

    public static float CircularEaseInOut(float p)
    {
        if (p < 0.5f)
        {
            return 0.5f * (1f - (float)Math.Sqrt((double)(1f - 4f * (p * p))));
        }
        return 0.5f * ((float)Math.Sqrt((double)(-(double)(2f * p - 3f) * (2f * p - 1f))) + 1f);
    }

    public static float ExponentialEaseIn(float p)
    {
        if (p != 0f)
        {
            return (float)Math.Pow(2.0, (double)(10f * (p - 1f)));
        }
        return p;
    }

    public static float ExponentialEaseOut(float p)
    {
        if (p != 1f)
        {
            return 1f - (float)Math.Pow(2.0, (double)(-10f * p));
        }
        return p;
    }

    public static float ExponentialEaseInOut(float p)
    {
        if ((double)p == 0.0 || (double)p == 1.0)
        {
            return p;
        }
        if (p < 0.5f)
        {
            return 0.5f * (float)Math.Pow(2.0, (double)(20f * p - 10f));
        }
        return -0.5f * (float)Math.Pow(2.0, (double)(-20f * p + 10f)) + 1f;
    }

    public static float ElasticEaseIn(float p)
    {
        return (float)Math.Sin((double)(20.420353f * p)) * (float)Math.Pow(2.0, (double)(10f * (p - 1f)));
    }

    public static float ElasticEaseOut(float p)
    {
        return (float)Math.Sin((double)(-20.420353f * (p + 1f))) * (float)Math.Pow(2.0, (double)(-10f * p)) + 1f;
    }

    public static float ElasticEaseInOut(float p)
    {
        if (p < 0.5f)
        {
            return 0.5f * (float)Math.Sin((double)(20.420353f * (2f * p))) * (float)Math.Pow(2.0, (double)(10f * (2f * p - 1f)));
        }
        return 0.5f * ((float)Math.Sin((double)(-20.420353f * (2f * p - 1f + 1f))) * (float)Math.Pow(2.0, (double)(-10f * (2f * p - 1f))) + 2f);
    }

    public static float BackEaseIn(float p)
    {
        return p * p * p - p * (float)Math.Sin((double)(p * 3.1415927f));
    }

    public static float BackEaseOut(float p)
    {
        float num = 1f - p;
        return 1f - (num * num * num - num * (float)Math.Sin((double)(num * 3.1415927f)));
    }

    public static float BackEaseInOut(float p)
    {
        if (p < 0.5f)
        {
            float num = 2f * p;
            return 0.5f * (num * num * num - num * (float)Math.Sin((double)(num * 3.1415927f)));
        }
        float num2 = 1f - (2f * p - 1f);
        return 0.5f * (1f - (num2 * num2 * num2 - num2 * (float)Math.Sin((double)(num2 * 3.1415927f)))) + 0.5f;
    }

    public static float BounceEaseIn(float p)
    {
        return 1f - BounceEaseOut(1f - p);
    }

    public static float BounceEaseOut(float p)
    {
        if (p < 0.36363637f)
        {
            return 121f * p * p / 16f;
        }
        if (p < 0.72727275f)
        {
            return 9.075f * p * p - 9.9f * p + 3.4f;
        }
        if (p < 0.9f)
        {
            return 12.066482f * p * p - 19.635458f * p + 8.898061f;
        }
        return 10.8f * p * p - 20.52f * p + 10.72f;
    }

    public static float BounceEaseInOut(float p)
    {
        if (p < 0.5f)
        {
            return 0.5f * BounceEaseIn(p * 2f);
        }
        return 0.5f * BounceEaseOut(p * 2f - 1f) + 0.5f;
    }

    public enum Functions
    {
        Linear,
        QuadraticEaseIn,
        QuadraticEaseOut,
        QuadraticEaseInOut,
        CubicEaseIn,
        CubicEaseOut,
        CubicEaseInOut,
        QuarticEaseIn,
        QuarticEaseOut,
        QuarticEaseInOut,
        QuinticEaseIn,
        QuinticEaseOut,
        QuinticEaseInOut,
        SineEaseIn,
        SineEaseOut,
        SineEaseInOut,
        CircularEaseIn,
        CircularEaseOut,
        CircularEaseInOut,
        ExponentialEaseIn,
        ExponentialEaseOut,
        ExponentialEaseInOut,
        ElasticEaseIn,
        ElasticEaseOut,
        ElasticEaseInOut,
        BackEaseIn,
        BackEaseOut,
        BackEaseInOut,
        BounceEaseIn,
        BounceEaseOut,
        BounceEaseInOut
    }
}
