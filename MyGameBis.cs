using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using DodgeBlock.data.Jeu;

namespace DodgeBlock;

public class MyGameBis : Game
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
    
    private int _score = 0;          
    private float _timer = 0f;       
    private SpriteFont _font;      
    
    public MyGameBis()
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
        _font = Content.Load<SpriteFont>("fonts/textFont");
        _spriteBatch = new SpriteBatch(GraphicsDevice);

    }

    protected override void Update(GameTime gameTime)
    {
        // Gestion du temps
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if(_timer >=1.0f)
        {
            _score+=10;
            _timer -= 1.0f;
        }
        
        // Mise à jour du joueur
        _ship.Update(gameTime);
        
        // Mise à jour des blocs et vérification des collisions
        foreach (var block in _blocks)
        {
            block.Update(gameTime);

            // Collision avec un bloc
            if (_ship.Rect.Intersects(block.Rect))
            {
                HandleCollision(); // Gérer la collision
                break;
            }
        }
        
        foreach (var pouvoir in _pouvoirs)
        {
            pouvoir.MettreAJour((float)gameTime.ElapsedGameTime.TotalSeconds);

            // Si le pouvoir n'est pas actif, vérifier collision avec le joueur
            if (!pouvoir.Actif && new Rectangle(pouvoir.PositionX, pouvoir.PositionY, 50, 50).Intersects(_ship.Rect))
            {
                if (pouvoir.Type == PouvoirsType.Bouclier)
                {
                    pouvoir.ActiverPouvoir();
                    Console.WriteLine("Bouclier collecté !");
                }
            }
        }
        
        // Gestion des entrées pour quitter
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
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
        
        // Dessiner le joueur
        _ship.Draw(_spriteBatch);
        

        // Dessiner les blocs
        foreach (var block in _blocks)
        {
            block.Draw(_spriteBatch);
        }
        
        // Dessiner les pouvoirs actifs
        foreach (var pouvoir in _pouvoirs)
        {
            if (pouvoir.Actif)
            {
                _spriteBatch.DrawString(_font, $"{pouvoir.Type}: {Math.Max(0, (int)pouvoir.Duree)}s", new Vector2(50, 100), Color.White);
            }
        }
        if (_pouvoirs.Exists(p => p.Type == PouvoirsType.Bouclier && p.Actif))
        {
            _spriteBatch.Draw(_shieldTexture, _ship.Rect, Color.Blue * 0.5f); // Aura bleue semi-transparente
        }
        
        // Dessiner le score
        _spriteBatch.DrawString(_font, $"Score : {_score}", new Vector2(50, 50), Color.White);
        
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    public void HandleCollision()
    {
        // Si le joueur a un pouvoir Bouclier actif, on ne fait rien à la collision
        if (_pouvoirs.Exists(p => p.Type == PouvoirsType.Bouclier && p.Actif))
        {
            // On désactive le pouvoir Bouclier
            var bouclier = _pouvoirs.Find(p => p.Type == PouvoirsType.Bouclier && p.Actif);
            if (bouclier != null)
            {
                bouclier.DesactiverPouvoir();
                Console.WriteLine("Collision ignorée grâce au Bouclier !");
            }
        }
        else
        {
            // si aucun bouclier actif, le jeu se termine
            Console.WriteLine("Collision détectée !");
            Exit();
        }
    }
    
}
