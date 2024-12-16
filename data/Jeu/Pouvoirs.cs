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
    private Texture2D _texture;
    private Vector2 _position;
    private Rectangle _rect;

    public Rectangle Rect => _rect;

    public Pouvoirs(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        _position = position;
        _rect = new Rectangle((int)position.X, (int)position.Y, _texture.Width, _texture.Height);
    }
    private Vector2 GenerateRandomPosition()
    {
        // Génère une position aléatoire dans la fenêtre de jeu
        Random rnd = new Random();
        int x = rnd.Next(0, 800); // Ajuste la taille en fonction de ta fenêtre
        int y = rnd.Next(0, 600);
        return new Vector2(x, y);
    }
    
    public static Pouvoirs GeneratePowerUp(Texture2D texture, int screenWidth, int screenHeight)
    {
        Random rand = new Random();
        Vector2 position = new Vector2(rand.Next(0, screenWidth - texture.Width), rand.Next(0, screenHeight - texture.Height));
        return new Pouvoirs(texture, position);
    }
    
}


