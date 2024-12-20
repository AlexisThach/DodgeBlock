using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using DodgeBlock.data.Enum;

namespace DodgeBlock.data.Entities;

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
    
    private int _score;
    
    //invisibilte
    private bool _isInvisible;
    private float _invisibilityDuration;
    private float _invisibilityTimer;

    public bool IsInvisible => _isInvisible;
    
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
        
        // Gestion de la décélération automatique (ralentir progressivement)
        if (!Keyboard.GetState().IsKeyDown(Keys.Up) && !Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            _speed.Y -= Math.Sign(_speed.Y) * SpeedDec;
            if (Math.Abs(_speed.Y) < 0.01f) _speed.Y = 0; // Arrêt complet si la vitesse est faible
        }

        if (!Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _speed.X -= Math.Sign(_speed.X) * SpeedDec;
            if (Math.Abs(_speed.X) < 0.01f) _speed.X = 0; // Arrêt complet si la vitesse est faible
        }
        
        // Gestion des bordures de l'écran
        if (_position.X < 0)
        {
            _position.X = 0; // Empêche de sortir à gauche
            _speed.X = 0;
        }
        if (_position.X > 1000 - _size)
        {
            _position.X = 1000 - _size; // Empêche de sortir à droite
            _speed.X = 0;   
        }
        if (_position.Y < 0)
        {
            _position.Y = 0; // Empêche de sortir en haut
            _speed.Y = 0;
        }
        if (_position.Y > 800 - _size)
        {
            _position.Y = 800 - _size; // Empêche de sortir en bas
            _speed.Y = 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Débug hitbox du joueur
        //spriteBatch.Draw(_texture, Rect, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
        if (!_isInvisible)
        {
            var origin = new Vector2(_texture.Width - _size, _texture.Height - _size);
            spriteBatch.Draw(
                _texture, // Texture2D
                Rect, // Rectangle destination
                null, // Nullable<Rectangle> sourceRectangle
                _color, // Color
                0.0f, // float rotation
                origin, // Vector2 origin
                SpriteEffects.None, // SpriteEffects
                0f // float layerDepth
            );
        }
    }
    public void ChangerApparence(Texture2D nouvelleTexture)
    {
        _texture = nouvelleTexture;
    }
    public int Score
    {
        get => _score;
        private set => _score = value;
    }
    public void IncrementerScore(int points, Pouvoirs pouvoirActif)
    {
        if (pouvoirActif != null && pouvoirActif.Type == PouvoirsType.DoubleScore && pouvoirActif.Actif)
        {
            points *= 2; // Double les points
        }

        _score += points;
    }
    
    public void ActiverInvisibilite(float duree)
    {
        _isInvisible = true;
        _invisibilityDuration = duree;
        _invisibilityTimer = duree;
    }
    public void MettreAJourInvisibilite(float deltaTime)
         {
             if (_isInvisible)
             {
                 _invisibilityTimer -= deltaTime;
                 if (_invisibilityTimer <= 0)
                 {
                     _isInvisible = false;
                     _invisibilityTimer = 0;
                 }
             }
         }
}
