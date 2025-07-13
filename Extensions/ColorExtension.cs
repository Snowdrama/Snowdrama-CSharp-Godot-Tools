using Godot;
public static class ColorExtensions
{

    public static Color SetHsv(this Color current, float setHue = 0.0f, float setSaturation = 1.0f, float setValue = 1.0f)
    {
        var newColor = current;
        newColor.H = 1.0f * setHue;
        newColor.S = 1.0f * setSaturation;
        newColor.V = 1.0f * setValue;
        return newColor;
    }
    public static Color ShiftHsv(this Color current, float shiftHue = 0.0f, float shiftSaturation = 0.0f, float shiftValue = 0.0f)
    {
        var newColor = current;
        float h, s, v;
        current.ToHsv(out h, out s, out v);
        h = (h + shiftHue) % 1.0001f;
        s = (s + shiftSaturation) % 1.0001f;
        v = (v + shiftValue) % 1.0001f;
        newColor.H = h;
        newColor.S = s;
        newColor.V = v;
        return newColor;
    }
    public static Gradient g = new Gradient()
    {
        Colors = new Color[]
                {
                    Colors.Red,
                    Colors.Orange,
                    Colors.Yellow,
                    Colors.Green,
                    Colors.Teal,
                    Colors.Blue,
                    Colors.BlueViolet,
                    Colors.Violet,
                    Colors.Purple,
                    Colors.Magenta,
                },
        Offsets = new float[]
                {
                    0.1f,
                    0.2f,
                    0.3f,
                    0.4f,
                    0.5f,
                    0.6f,
                    0.7f,
                    0.8f,
                    0.9f,
                    1.0f,
                },
    };
    public static Color GetColorFromRainbow(float t, Gradient? gradient = null)
    {
        if (gradient != null)
        {
            return gradient.Sample(t % 1.0f);
        }
        return g.Sample(t % 1.0f);
    }
}