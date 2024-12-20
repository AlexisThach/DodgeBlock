using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using DodgeBlock.data.Enum;
using System.Threading.Tasks;

namespace DodgeBlock.data.Jeu;

public class Pouvoirs
{
    public PouvoirsType Type { get; private set; }
    public float Duree { get; set; }
    public bool Actif { get; private set; }

    public int PositionX { get; private set; }
    public int PositionY { get; private set; }

    public Rectangle Rect => new Rectangle(PositionX, PositionY, 50, 50); // Taille du pouvoir

    private float tempsRestant;

    private static Random random = new Random();
    public Pouvoirs(PouvoirsType type, float duree)
    {
        Type = type;
        Duree = duree;
        Actif = false;
        tempsRestant = 0;
    }

    public void GenererPositionAleatoire(int largeurMax, int hauteurMax, List<Pouvoirs> pouvoirsExistants)
    {
        bool positionValide;

        do
        {
            PositionX = random.Next(0, largeurMax); // Position X entre 0 et largeurMax
            PositionY = random.Next(0, hauteurMax); // Position Y entre 0 et hauteurMax

            // Vérifie si cette position entre en collision avec d'autres pouvoirs
            positionValide = true;
            foreach (var pouvoir in pouvoirsExistants)
            {
                if (Rect.Intersects(pouvoir.Rect))
                {
                    positionValide = false;
                    break;
                }
            }
        } while (!positionValide);

        Console.WriteLine($"Position aléatoire de {Type} : ({PositionX}, {PositionY})");
    }

    public void ActiverPouvoir()
    {
        if (!Actif)
        {
            Actif = true;
            tempsRestant = Duree;
            Console.WriteLine($"{Type} activé pour {Duree} secondes.");
        }
    }
    public void DesactiverPouvoir()
    {
        if (Actif)
        {
            Actif = false;
            tempsRestant = 0;
            Console.WriteLine($"{Type} désactivé.");
        }
    }

    // Mettre à jour le temps restant
    public void MettreAJour(float deltaTime, Joueur joueur, Texture2D baseTexture)
    {
        if (Actif)
        {
            tempsRestant -= deltaTime;
            if (tempsRestant <= 0)
            {
                DesactiverPouvoir();

                if (Type == PouvoirsType.SpeedBoost)
                {
                    joueur.ChangerApparence(baseTexture); // Réinitialiser l'apparence du joueur
                }
                else if (Type == PouvoirsType.DoubleScore)
                {
                    joueur.SpeedAcc /= 2; // Réinitialiser l'accélération du joueur
                    joueur.SpeedDec /= 2; // Réinitialiser la décélération du joueur
                }
            }
        }
    }
    

    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        if (!Actif)
        {
            spriteBatch.Draw(texture, new Rectangle(PositionX, PositionY, 50, 50), Color.White);
        }
    }
}