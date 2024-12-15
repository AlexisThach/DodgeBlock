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
    
    public Rectangle Rect => new Rectangle((int)_position.X, (int)_position.Y, _size, _size);
    
    public Block(Texture2D texture, Vector2 position, int size, float speed)
    {
        _texture = texture;
        _position = position;
        _size = size;
        _speed = speed;
    }
    
    public static List<Block> InitialiseBlocks(Texture2D blockTexture)
    {
        var blocks = new List<Block>();
        var random = new Random();
        for (int i = 0; i < 12; i++)
        {
            int x = random.Next(0, 800 - 50);  // Position aléatoire sur l'axe des abscisses 
            int size = 50;                    // Taille fixe des blocs
            float speed = random.Next(2, 5);  // Vitesse aléatoire entre 2 et 5
            blocks.Add(new Block(blockTexture, new Vector2(x, -size), size, speed));
        }
        return blocks;
    }

    public void LoadContent(ContentManager content)
    {
        if (_texture == null) // Charger la texture uniquement si elle n'est pas déjà chargée
        {
            _texture = content.Load<Texture2D>("asteroid");
        }
    }

    public void Update(GameTime gameTime)
    {
        // Faire tomber le bloc
        _position.Y += _speed;

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
        var random = new Random();
        _position = new Vector2(random.Next(0, 800 - _size), -_size); 
    }
}
