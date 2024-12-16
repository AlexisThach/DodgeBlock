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
    }

    protected override void LoadContent()
    {
        // Charger les textures 
        _backgroundTexture = Content.Load<Texture2D>("images/space");
        _shipTexture = Content.Load<Texture2D>("images/ship");
        _blockTexture = Content.Load<Texture2D>("images/asteroid");
       // _font = Content.Load<SpriteFont>("fonts/arial"); // MARCHE PAS SUR MAC COMMENT FAIREEEEEEEEEEEEEEEEEEEEEEEEEEE
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);

    }

    protected override void Update(GameTime gameTime)
    {
        // Gestion du temps
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if(_timer >=1f)
        {
            _score++;
            _timer = 0f;
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
        
        // Dessiner le score
       // _spriteBatch.DrawString(_font, $"Score : {_score}", new Vector2(10, 10), Color.White);

        // Dessiner les blocs
        foreach (var block in _blocks)
        {
            block.Draw(_spriteBatch);
        }
        
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    public void HandleCollision()
    {
        Console.WriteLine("Collision détectée !");
        Exit();
    }
    
}
