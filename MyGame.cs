using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using DodgeBlock.data.Jeu;

namespace DodgeBlock;

public class MyGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Joueur _ship; 
    private List<Block> _blocks; 
    private List<Pouvoirs> _pouvoirs; 

    private Random _random = new Random();
    private Texture2D _backgroundTexture;
    private Texture2D _blockTexture; 
    private Texture2D _shipTexture;
    private Texture2D _shieldTexture;
    private Texture2D _shipWithShieldTexture;
    
    private int _score = 0;          
    private float _timer = 0f;       
    private SpriteFont _font;      
    
    private GameState _currentState = GameState.EnJeu;
    public enum GameState
    {
        EnJeu,
        GameOver
    }

    public MyGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1000; // Largeur de la fenêtre
        _graphics.PreferredBackBufferHeight = 800; // Hauteur de la fenêtre
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    
    private Vector2 GetPositionDepart()
    {
        int playerX = _graphics.PreferredBackBufferWidth / 2;
        int playerY = _graphics.PreferredBackBufferHeight - 50;
        return new Vector2(playerX, playerY);
    }

    protected override void Initialize()
    { 
        base.Initialize();
        
        _ship = new Joueur(_shipTexture, GetPositionDepart(), 50);
        _blocks = Block.InitialiseBlocks(_blockTexture); 
        _pouvoirs = new List<Pouvoirs>();
        _pouvoirs.Add(new Pouvoirs(PouvoirsType.Bouclier, 60.0f));
        //_pouvoirs.Add(new Pouvoirs(PouvoirsType.Invincibilite, 10.0f));
        //_pouvoirs.Add(new Pouvoirs(PouvoirsType.DoubleScore, 15.0f));
    }

    protected override void LoadContent()
    {
        // Charger les textures  
        _backgroundTexture = Content.Load<Texture2D>("images/space");
        _shipTexture = Content.Load<Texture2D>("images/ship");
        _blockTexture = Content.Load<Texture2D>("images/asteroid");
        _shieldTexture = Content.Load<Texture2D>("images/shield");
        _shipWithShieldTexture = Content.Load<Texture2D>("images/ship_with_shield");
        
        _font = Content.Load<SpriteFont>("fonts/Game_fonts"); 
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);

    }

    protected override void Update(GameTime gameTime)
    {
        // Mise à jour du joueur
        _ship.Update(gameTime);
        
        // Si le jeu est en GameOver, gérer les entrées pour redémarrer ou quitter
        if (_currentState == GameState.GameOver)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.R)) // Rejouer
            {
                ResetGame();
            }
            else if (keyboardState.IsKeyDown(Keys.Escape)) // Quitter le jeu
            {
                Exit();
            }

            return; // Ne pas mettre à jour le reste du jeu
        }

        // Gestion du temps et score
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_timer >= 1.0f)
        {
            _score += 10;
            _timer -= 1.0f;
        }


        // Mise à jour des blocs et collisions
        foreach (var block in _blocks)
        {
            block.Update(gameTime);

            if (_ship.Rect.Intersects(block.Rect))
            {
                HandleCollision();
                break;
            }
        }

        // Vérifier les collisions entre le joueur et les pouvoirs
        for (int i = 0; i < _pouvoirs.Count; i++)
        {
            var pouvoir = _pouvoirs[i];
    
            if (_ship.Rect.Intersects(pouvoir.Rect))
            {
                ActiverPouvoir(pouvoir); 
                _pouvoirs.RemoveAt(i); 
                break; 
            }
        }

        // Gestion des entrées pour quitter (en cours de jeu)
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

        if (_currentState == GameState.EnJeu)
        {
            // Dessiner les pouvoirs non collectés
            foreach (var pouvoir in _pouvoirs)
            {
                if (!pouvoir.Actif)
                {
                    pouvoir.Draw(_spriteBatch, _shieldTexture);
                }
            }
            // Dessiner le joueur, blocs et score
            _ship.Draw(_spriteBatch);
            foreach (var block in _blocks)
            {
                block.Draw(_spriteBatch);
            }
            
        }
        
        else if (_currentState == GameState.GameOver)
        {
            // Définir les dimensions et la position du cadre
            int cadreLargeur = 400;
            int cadreHauteur = 200;
            int cadreX = (_graphics.PreferredBackBufferWidth - cadreLargeur) / 2;
            int cadreY = (_graphics.PreferredBackBufferHeight - cadreHauteur) / 2;

            // Dessiner un fond semi-transparent (cadre)
            Texture2D cadreTexture = new Texture2D(GraphicsDevice, 1, 1);
            cadreTexture.SetData(new[] { Color.Black * 0.7f }); // Noir semi-transparent
            _spriteBatch.Draw(cadreTexture, new Rectangle(cadreX, cadreY, cadreLargeur, cadreHauteur), Color.White);

            // Afficher le texte dans le cadre
            Vector2 textPos1 = new Vector2(cadreX + 50, cadreY + 50); // Position du premier texte
            Vector2 textPos2 = new Vector2(cadreX + 50, cadreY + 100); // Position du deuxième texte
            Vector2 textPos3 = new Vector2(cadreX + 50, cadreY + 130); // Position du troisième texte

            _spriteBatch.DrawString(_font, "GAME OVER", new Vector2(cadreX + 120, cadreY + 20), Color.Red);
            _spriteBatch.DrawString(_font, "Appuyez sur R pour rejouer", textPos2, Color.White);
            _spriteBatch.DrawString(_font, "Appuyez sur Echap pour quitter", textPos3, Color.White);
        }
        
        _spriteBatch.DrawString(_font, $"Score : {_score}", new Vector2(50, 50), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    public void HandleCollision()
    {
        // Vérifie si le joueur a un pouvoir Bouclier actif
        var bouclier = _pouvoirs.Find(p => p.Type == PouvoirsType.Bouclier && p.Actif);
        if (bouclier != null)
        {
            bouclier.DesactiverPouvoir();
            _pouvoirs.Remove(bouclier);
            Console.WriteLine("Collision ignorée grâce au Bouclier !");
            _ship.ChangerApparence(_shipTexture);
            return;
        }
        
        Console.WriteLine("Game Over !");
        _currentState = GameState.GameOver; 
    }    
    private void ActiverPouvoir(Pouvoirs pouvoir)
    {
        pouvoir.ActiverPouvoir();

        switch (pouvoir.Type)
        {
            case PouvoirsType.Bouclier:
                _ship.ChangerApparence(_shipWithShieldTexture);
                Console.WriteLine("Bouclier activé !");
                break;

            case PouvoirsType.Invincibilite:
                Console.WriteLine("Invincibilité activée !");
                break;

            case PouvoirsType.DoubleScore:
                Console.WriteLine("Double score activé !");
                break;

            default:
                Console.WriteLine("Pouvoir inconnu activé !");
                break;
        }
    }
    private void ResetGame()
    {
        _currentState = GameState.EnJeu;
        _score = 0;
        _timer = 0f;

        // Réinitialiser la position du joueur
        _ship = new Joueur(_shipTexture, GetPositionDepart(), 50);

        // Réinitialiser les blocs
        _blocks = Block.InitialiseBlocks(_blockTexture);

        // Réinitialiser les pouvoirs
        _pouvoirs = new List<Pouvoirs>();
        _pouvoirs.Add(new Pouvoirs(PouvoirsType.Bouclier, 0f));
    }
    

    
}
