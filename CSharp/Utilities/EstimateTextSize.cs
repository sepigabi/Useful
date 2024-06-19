using System.Drawing;

public static SizeF EstimateTextSize(string text, string fontFamily, float fontSize)
{
    using (var bitmap = new Bitmap(1, 1))
    using (var graphics = Graphics.FromImage(bitmap))
    {
        var font = new Font(fontFamily, fontSize);
        SizeF textSize = graphics.MeasureString(text, font);
        return textSize;
    }
}
