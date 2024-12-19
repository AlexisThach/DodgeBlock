using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace DodgeBlock.data.Jeu;

public class Block
{
    private Texture2D _texture;
    private Vector2 _position;
    private float _speed;
    private int _size;
    private static readonly Random random = new Random();

    
    public Rectangle Rect => new Rectangle((int)_position.X, (int)_position.Y, _size, _size);
    
    public Block(Texture2D texture, Vector2 position, int size, float speed)
    {
        _texture = texture ?? throw new ArgumentNullException(nameof(texture));
        _position = position;
        _size = size > 0 ? size : throw new ArgumentOutOfRangeException(nameof(size));
        _speed = speed > 0 ? speed : throw new ArgumentOutOfRangeException(nameof(speed));
    }
    
    public static List<Block> InitialiseBlocks(Texture2D blockTexture)
    {
        var blocks = new List<Block>();
        for (int i = 0; i <10; i++)
        {
            int x = random.Next(0, 1000 - 50);  // Position aléatoire sur l'axe des abscisses 
            int size = 50;                    // Taille fixe des blocs
            float speed = random.Next(200, 250);  // Vitesse aléatoire
            blocks.Add(new Block(blockTexture, new Vector2(x, -size), size, speed));
        }
        return blocks;
    }
    
    public void Update(GameTime gameTime)
    {
        // Faire tomber le bloc
        _position.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Réinitialiser le bloc en haut de l'écran lorsqu'il sort du bas
        if (_position.Y > 800)
        {
            ResetPosition();
        }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Rect, Color.White);
    }

    public void ResetPosition()
    {
        _position = new Vector2(random.Next(0, 1000 - _size), -_size); 
    }
}
