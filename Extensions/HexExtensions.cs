using System;
using Godot;
public static class HexExtensions
{
    /// <summary>
    /// Takes a value from 0 to 255 and returns a hex value from 00 to FF
    /// </summary>
    /// <param name="value">A value from 0 to 255</param>
    /// <returns>a string representing hex from 00 to FF</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string IntToHexByte(this int value)
    {
        if (value < 0 || value > 255)
        {
            throw new ArgumentOutOfRangeException("Value must be 0 to 255");
        }
        return value.ToString("X2");
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string NormalizedFloatToHexByte(this float value)
    {
        return IntToHexByte(Mathf.RoundToInt(value * 255.0f));
    }


    /// <summary>
    /// Take 00 to FF and returns 0 to 255
    /// </summary>
    /// <param name="hexByte">a single byte in hex as a string 00 to FF</param>
    /// <returns>A number from 0 to 255</returns>
    public static int HexByteToInt(this string hexByte)
    {
        int dec = System.Convert.ToInt32(hexByte, 16);
        return dec;
    }

    /// <summary>
    /// Take 00 to FF and returns 0.0f to 1.0f
    /// </summary>
    /// <param name="hexByte">a single byte in hex as a string 00 to FF</param>
    /// <returns>A number from 0.0f to 1.0f</returns>
    public static float HexByteToNormalizedFloat(this string hexByte)
    {
        return HexByteToInt(hexByte) / 255.0f;
    }

    /// <summary>
    /// Takes a string of hex like "FF80CC" and returns an array of values for each byte 0 - 255
    /// </summary>
    /// <param name="hex"></param>
    /// <returns>An array of ints from 0 to 255 based on the hex</returns>
    public static int[] HexByteToIntArray(this string hex)
    {
        hex = hex.Replace("#", ""); //remove the # if present
        if (hex.Length % 2 != 0)
        {
            throw new Exception("Hex string needs to contain 2 characters per byte, like 'FF' or 'AB'");
        }

        int[] output = new int[hex.Length / 2];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = HexByteToInt(hex.Substring(i * 2, 2));
        }
        return output;
    }

    /// <summary>
    /// Takes a string of hex like "FF80CC" and returns an array of normalized values for each byte 0.0f - 1.0f
    /// </summary>
    /// <param name="hex"></param>
    /// <returns>An array of normalized floats from 0.0f to 1.0f</returns>
    public static float[] HexByteToNormalizedFloatArray(this string hex)
    {
        hex = hex.Replace("#", ""); //remove the # if present
        if(hex.Length % 2 != 0)
        {
            throw new Exception("Hex string needs to contain 2 characters per byte, like 'FF' or 'AB'");
        }

        float[] output = new float[hex.Length/2];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = HexByteToNormalizedFloat(hex.Substring(i * 2, 2));
        }
        return output;
    }

    public static string ColorToHex(this Color color, bool useRGBA = true)
    {
        var r = NormalizedFloatToHexByte(color.R);
        var g = NormalizedFloatToHexByte(color.G);
        var b = NormalizedFloatToHexByte(color.B);
        if (useRGBA)
        {
            var a = NormalizedFloatToHexByte(color.A);
            return $"{r}{g}{b}{a}";
        }
        return $"{r}{g}{b}";
    }

    public static Color HexToColor(this string hexString)
    {
        var values = HexByteToNormalizedFloatArray(hexString);
        if(values.Length == 3){
            return new Color(values[0], values[1], values[2]);
        }
        if(values.Length == 4){
            return new Color(values[0], values[1], values[2], values[3]);
        }
        throw new Exception("Hex to Color conversion needs 6 or 8 characters, like 'FF00AA' or '00FF88FF' ");
    }
}