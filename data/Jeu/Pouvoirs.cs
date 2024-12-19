using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DodgeBlock.data.Jeu;
 
// Énumération pour les types de pouvoirs
public enum PouvoirsType
{
    Bouclier,       // Bouclier : protège contre un impact
    Invincibilite,  // Invincibilité : immunité temporaire
    DoubleScore,    // DoubleScore : double le score obtenu pendant un temps limité
}

public class Pouvoirs
{   
    public PouvoirsType Type { get; private set; }
    public float Duree { get; private set; }
    public bool Actif { get; private set; }

    public int PositionX { get; private set; }
    public int PositionY { get; private set; }

    private float tempsRestant;

    private static Random random = new Random();
    public Pouvoirs(PouvoirsType type, float duree)
    {
        Type = type;
        Duree = duree;
        Actif = false;
        tempsRestant = 0;
        GenererPositionAleatoire(950, 750); 
    }
    public void GenererPositionAleatoire(int largeurMax, int hauteurMax)
    {
        PositionX = random.Next(0, largeurMax); // Position X entre 0 et largeurMax
        PositionY = random.Next(0, hauteurMax); // Position Y entre 0 et hauteurMax
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
    public void MettreAJour(float deltaTime)
    {
        if (Actif)
        {
            tempsRestant -= deltaTime;
            if (tempsRestant <= 0)
            {
                DesactiverPouvoir();
            }
        }
    }
    
}


