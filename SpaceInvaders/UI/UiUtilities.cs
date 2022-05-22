using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders;
internal static class UiUtilities
{
  public static void DrawString(
    this SpriteBatch spriteBatch,
    SpriteFont font,
    string text,
    Rectangle bounds,
    Alignment align,
    Color color)
  {
    var size = font.MeasureString(text);
    var pos = bounds.Center.ToVector2();
    var origin = size * 0.5f;

    if (align.HasFlag(Alignment.Left))
      origin.X += (bounds.Width / 2) - (size.X / 2);

    if (align.HasFlag(Alignment.Right))
      origin.X -= (bounds.Width / 2) - (size.X / 2);

    if (align.HasFlag(Alignment.Top))
      origin.Y += (bounds.Height / 2) - (size.Y / 2);

    if (align.HasFlag(Alignment.Bottom))
      origin.Y -= (bounds.Height / 2) - (size.Y / 2);

    spriteBatch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
  }

  public static void DrawStringWithBackground(
    this SpriteBatch spriteBatch,
    Rectangle backgroundRectangle,
    SpriteFont font,
    string text,
    Color textColor,
    Color backgroundColor)
  {
    spriteBatch.FillRectangle(backgroundRectangle, backgroundColor);

    spriteBatch.DrawString(font, text,
      backgroundRectangle,
      Alignment.Center,
      textColor);
  }
}

[Flags]
public enum Alignment
{
  Center = 0,
  Left = 1,
  Right = 2,
  Top = 4,
  Bottom = 8
}
