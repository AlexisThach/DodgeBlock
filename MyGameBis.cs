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

    private Joueur _ship; // Instance du joueur
    private List<Block> _blocks; // Liste des blocs tombants
    private List<Pouvoirs> _pouvoirs;
    private Random _random = new Random();
    
    private EndGame _endGame; // Fenêtre de fin de jeu
    private Texture2D _blockTexture; // Texture des blocs
    private Texture2D _shipTexture; 
    private Texture2D _buttonTexture; // Texture des boutons
    private SpriteFont _font; // Police d'écriture

    private int _score = 0;
    private bool _isGameOver = false;
    
    public MyGameBis()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 800; // Largeur de la fenêtre
        _graphics.PreferredBackBufferHeight = 800; // Hauteur de la fenêtre
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    
    private Vector2 GetPlayerStartPosition()
    {
        int playerX = _graphics.PreferredBackBufferWidth / 2;
        int playerY = _graphics.PreferredBackBufferHeight - 50;
        return new Vector2(playerX, playerY);
    }

    protected override void Initialize()
    { 
        base.Initialize();
        _ship = new Joueur(_shipTexture, GetPlayerStartPosition(), 50);
        _blocks = Block.InitialiseBlocks(_blockTexture); 
        _pouvoirs = new List<Pouvoirs>();
        _random = new Random();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Charger les textures 
        _shipTexture = Content.Load<Texture2D>("ship2");
        _buttonTexture = Content.Load<Texture2D>("button");
        _blockTexture = Content.Load<Texture2D>("asteroid");
        //_font = Content.Load<SpriteFont>("arial");
        
        // Initialiser la fenêtre de fin de jeu (MARCHE PAS)
        _endGame = new EndGame(_font, _buttonTexture, GraphicsDevice);
        _endGame.RetryClicked += RestartGame;
        _endGame.QuitClicked += Exit;
    }

    protected override void Update(GameTime gameTime)
    {
        if(_isGameOver)
        {
            // gerer la fenetre Game Over
            _endGame.Update(Mouse.GetState());
            return;
        }
        // Mettre à jour le joueur
        _ship.Update(gameTime);
        
        // Mettre à jour chaque bloc
        foreach (var block in _blocks)
        {
            block.Update(gameTime);
            
            // Vérifier la collision entre le joueur et le bloc
            if (_ship.Rect.Intersects(block.Rect))
            {
                // Action en cas de collision
                HandleCollision();
                break; // Arrete de vérifier après une collision
            }
        }

        _score++; 
        
        // Sortir du jeu si la touche Escape ou le bouton Back est pressé
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

        if (!_isGameOver)
        {
            // Dessiner le joueur
            _ship.Draw(_spriteBatch);

            // Dessiner les blocs
            foreach (var block in _blocks)
            {
                block.Draw(_spriteBatch);
            }
        }
        else
        {   
            // Dessiner la fenêtre de fin de jeu
            _endGame.Draw(_spriteBatch, _score);
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    private void HandleCollision()
    {
        Console.WriteLine("Collision détectée !");
        
        // Fermer le jeu
        Exit();
    }

    private void RestartGame()
    {
        // réinitialiser l'état du jeu
        _isGameOver = false;
        _score = 0;
        _endGame.IsVisible = false;
        
        // réinitialiser le joueur
        _ship = new Joueur(_shipTexture, new Vector2(150, 150), 50);
        
        // réinitialiser les blocs
        foreach (var block in _blocks)
        {
            block.ResetPosition(); // méthode dans Block.cs pour réinitialiser la position
        }
    }
    
}
