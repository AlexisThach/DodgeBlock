using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DodgeBlock.data.Jeu;

public class Button
{
    private Texture2D _texture;
    private Rectangle _rect;
    private string _text;
    private SpriteFont _font;

    public Button(Texture2D texture, Rectangle rect, string text, SpriteFont font)
    {
        _texture = texture;
        _rect = rect;
        _text = text;
        _font = font;
    }

    public bool IsClicked(MouseState mouseState)
    {
        return _rect.Contains(mouseState.Position) &&
               mouseState.LeftButton == ButtonState.Pressed;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Dessine le bouton
        spriteBatch.Draw(_texture, _rect, Color.White);

        // Dessine le texte
        Vector2 textSize = _font.MeasureString(_text);
        Vector2 textPosition = new Vector2(
            _rect.X + (_rect.Width - textSize.X) / 2,
            _rect.Y + (_rect.Height - textSize.Y) / 2
        );
        spriteBatch.DrawString(_font, _text, textPosition, Color.Black);
    }
}
