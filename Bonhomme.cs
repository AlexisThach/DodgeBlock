using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DodgeBlock;

public class Bonhomme
{
    private Texture2D _texture;
    protected Vector2 _position;
    private int _size = 100;
    private static readonly int _sizeMin = 10;
    private static readonly int _sizeMax = 100;
    private Color _color = Color.White;

    // Vitesse et accélération
    private Vector2 _speed; // Vitesse du vaisseau (déplacements X et Y)
    private float _speedAcc = 0.2f; // Accélération (maintenant plus rapide)
    private float _speedDec = 0.1f; // Décélération plus forte (changement important ici)

    // Propriétés pour l'accélération et la décélération, avec des limites
    public float SpeedAcc
    {
        get => _speedAcc;
        set => _speedAcc = MathHelper.Clamp(value, 0.0f, 1.0f); // Limite entre 0.0f et 1.0f
    }

    public float SpeedDec
    {
        get => _speedDec;
        set => _speedDec = MathHelper.Clamp(value, 0.0f, 1.0f); // Limite entre 0.0f et 1.0f
    }

    public Texture2D Texture
    {
        get => _texture;
        init => _texture = value;
    }

    public int Size
    {
        get => _size;
        set => _size = Math.Clamp(value, _sizeMin, _sizeMax);
    }

    public Rectangle Rect
    {
        get => new Rectangle((int)_position.X, (int)_position.Y, _size, _size);
    }

    public Bonhomme(Texture2D texture, Vector2 position, int size)
    {
        Texture = texture;
        _position = position;
        Size = size;
    }

    public void Update(GameTime gameTime)
    {
        // Accélérer dans la direction en fonction des touches pressées
        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            _speed.Y -= SpeedAcc; // Accélérer vers le haut (vers le haut de l'écran)
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            _speed.Y += SpeedAcc; // Accélérer vers le bas (vers le bas de l'écran)
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _speed.X -= SpeedAcc; // Accélérer vers la gauche
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _speed.X += SpeedAcc; // Accélérer vers la droite
        }

        // Appliquer la vitesse (déplacement)
        _position.X += _speed.X;
        _position.Y += _speed.Y;

        // Décélérer rapidement quand aucune touche n'est pressée
        if (!Keyboard.GetState().IsKeyDown(Keys.Up) && _speed.Y < 0)
        {
            _speed.Y += SpeedDec; // Décélérer vers le haut
        }
        if (!Keyboard.GetState().IsKeyDown(Keys.Down) && _speed.Y > 0)
        {
            _speed.Y -= SpeedDec; // Décélérer vers le bas
        }
        if (!Keyboard.GetState().IsKeyDown(Keys.Left) && _speed.X < 0)
        {
            _speed.X += SpeedDec; // Décélérer vers la gauche
        }
        if (!Keyboard.GetState().IsKeyDown(Keys.Right) && _speed.X > 0)
        {
            _speed.X -= SpeedDec; // Décélérer vers la droite
        }

        // Limiter la vitesse à zéro si elle est trop proche de zéro (amortir les petits mouvements)
        if (Math.Abs(_speed.X) < 0.01f) _speed.X = 0;
        if (Math.Abs(_speed.Y) < 0.01f) _speed.Y = 0;
        
        // change size
        if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
        {
            Size -= 1;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
        {
            Size += 1;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);

        spriteBatch.Draw(
            _texture,  // Texture2D
            Rect,      // Rectangle destination
            null,      // Nullable<Rectangle> sourceRectangle
            _color,    // Color
            0.0f,      // float rotation
            origin,    // Vector2 origin
            SpriteEffects.None, // SpriteEffects
            0f         // float layerDepth
        );
    }
}
