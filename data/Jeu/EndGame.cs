using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DodgeBlock.data.Jeu;

public class EndGame
{
    private Rectangle _windowRect;
    private SpriteFont _font;
    private Texture2D _backgroundTexture;
    private Button _retryButton;
    private Button _quitButton;

    public bool IsVisible { get; set; } = false; // Indique si la fenêtre est affichée
    public event Action RetryClicked;
    public event Action QuitClicked;

    public EndGame(SpriteFont font, Texture2D buttonTexture, GraphicsDevice graphicsDevice)
    {
        _font = font;
        int screenWidth = graphicsDevice.Viewport.Width;
        int screenHeight = graphicsDevice.Viewport.Height;

        // Définir la taille et la position de la fenêtre
        _windowRect = new Rectangle(screenWidth / 2 - 150, screenHeight / 2 - 100, 300, 200);

        // Créer une texture de fond pour la fenêtre
        _backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
        _backgroundTexture.SetData(new[] { Color.DarkGray });

        // Boutons
        _retryButton = new Button(buttonTexture, new Rectangle(_windowRect.X + 20, _windowRect.Y + 120, 120, 40), "Rejouer", font);
        _quitButton = new Button(buttonTexture, new Rectangle(_windowRect.X + 160, _windowRect.Y + 120, 120, 40), "Quitter", font);
    }

    public void Update(MouseState mouseState)
    {
        if (!IsVisible) return;

        if (_retryButton.IsClicked(mouseState))
        {
            RetryClicked?.Invoke();
        }

        if (_quitButton.IsClicked(mouseState))
        {
            QuitClicked?.Invoke();
        }
    }

    public void Draw(SpriteBatch spriteBatch, int score)
    {
        if (!IsVisible) return;

        // Dessiner le fond de la fenêtre
        spriteBatch.Draw(_backgroundTexture, _windowRect, Color.White);

        // Dessiner le texte du score
        string scoreText = $"Score : {score}";
        Vector2 textSize = _font.MeasureString(scoreText);
        Vector2 textPosition = new Vector2(
            _windowRect.X + (_windowRect.Width - textSize.X) / 2,
            _windowRect.Y + 20
        );
        spriteBatch.DrawString(_font, scoreText, textPosition, Color.Black);

        // Dessiner les boutons
        _retryButton.Draw(spriteBatch);
        _quitButton.Draw(spriteBatch);
    }
}
