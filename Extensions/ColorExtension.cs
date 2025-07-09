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
    public static Gradient g;
    public static Color GetColorFromRainbow(float t, float of = 1, Gradient? gradient = null)
    {
        if (g == null)
        {
            g = new Gradient();
            g.AddPoint(0.1f, Colors.Red);
            g.AddPoint(0.2f, Colors.Orange);
            g.AddPoint(0.3f, Colors.Yellow);
            g.AddPoint(0.4f, Colors.Green);
            g.AddPoint(0.5f, Colors.Teal);
            g.AddPoint(0.6f, Colors.Blue);
            g.AddPoint(0.7f, Colors.BlueViolet);
            g.AddPoint(0.8f, Colors.Violet);
            g.AddPoint(0.9f, Colors.Purple);
            g.AddPoint(1.0f, Colors.Magenta);
        }
        return g.Sample(t % 1.0f);
    }
}