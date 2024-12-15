using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DodgeBlock.data.Jeu;
 
// Énumération pour les types de pouvoirs
public enum PouvoirsType
{
    Bouclier,       // Bouclier : protège contre un impact
    Invincibility,  // Invincibilité : immunité temporaire
    DoubleScore,    // DoubleScore : double le score obtenu pendant un temps limité
}

public class Pouvoirs
{
    public PouvoirsType Type { get; private set; }
    public Vector2 Position { get; private set; }
    public Texture2D Texture { get; private set; }
    public bool IsActive { get; set; } 
    
    public Pouvoirs(PouvoirsType type, Texture2D texture, Vector2 position)
    {
        Type = type;
        Texture = texture;
        Position = position;
        IsActive = true; 
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
    
    public bool CheckCollision(Rectangle playerRect)
    {
        return playerRect.Intersects(new Rectangle(Position.ToPoint(), new Point(Texture.Width, Texture.Height)));
    }
    
    
    
    
    
}


