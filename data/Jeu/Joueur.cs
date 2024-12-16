using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DodgeBlock.data.Jeu;

public class Joueur
{
    private Texture2D _texture;
    protected Vector2 _position;
    private int _size = 50;
    private Color _color = Color.White;
    
    // Vitesse et accélération
    private Vector2 _speed; 
    private float _speedAcc = 0.2f; 
    private float _speedDec = 0.1f; 

    public Joueur(Texture2D texture, Vector2 position, int size)
    {
        Texture = texture;
        _position = position;
        _size = size;
    }
    
    // Propriétés pour l'accélération et la décélération, avec des limites
    public float SpeedAcc
    {
        get => _speedAcc;
        set => _speedAcc = MathHelper.Clamp(value, 0.0f, 1.0f); 
    }

    public float SpeedDec
    {
        get => _speedDec;
        set => _speedDec = MathHelper.Clamp(value, 0.0f, 1.0f); 
    }

    public Texture2D Texture
    {
        get => _texture;
        init => _texture = value;
    }
    
    public Rectangle Rect
    {
        get
        {
            return new Rectangle(
                (int)_position.X, 
                (int)_position.Y,
                _size,
                _size
            );
        }
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
        
        // Gestion des bordures de l'écran
        if (_position.X < 0 || _position.X > 1000 - _size)
        {
            _speed.X = 0;  
        }
        if (_position.Y < 0 || _position.Y > 800 - _size)
        {
            _speed.Y = 0;  
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var origin = new Vector2(_texture.Width - _size, _texture.Height - _size);
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
