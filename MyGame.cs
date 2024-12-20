using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using DodgeBlock.data.Jeu;
using System.Text;
using DodgeBlock.data.Enum;
using DodgeBlock.data.Entities;


namespace DodgeBlock;

public class MyGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Joueur _joueur;
    private List<Block> _blocks;
    private List<Pouvoirs> _pouvoirs;

    private Random _random = new Random();
    private Texture2D _backgroundTexture;
    private Texture2D _blockTexture;
    private Texture2D _joueurTexture;
    private Texture2D _speedBoostTexture;
    private Texture2D _doubleScoreTexture;

    private int _score = 0;
    private float _scoreMultiplier = 1.0f;
    private float _timer = 0f;
    private SpriteFont _font;
    
    private float _doubleScoreRespawnTimer = 0f;
    private const float DoubleScoreRespawnInterval = 5.0f;


    private GameState _currentState = GameState.EnJeu;

    private string _playerName = "";
    private bool _isNameEntered;
    

    private StringBuilder _playerNameBuilder = new StringBuilder();
    private KeyboardState _previousKeyboardState;
    
    private int _lastScore = 0;
    private int _highestScore = 0;
    public MyGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1000; // Largeur de la fenêtre
        _graphics.PreferredBackBufferHeight = 800; // Hauteur de la fenêtre
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    
        _currentState = GameState.Menu;
        _previousKeyboardState = Keyboard.GetState();
    
        _joueur = new Joueur(_joueurTexture, GetPositionDepart(), 50);
        _blocks = Block.InitialiseBlocks(_blockTexture);
        _pouvoirs = new List<Pouvoirs>();
    
        var speedBoost = new Pouvoirs(PouvoirsType.SpeedBoost, 10.0f);
        speedBoost.GenererPositionAleatoire(_graphics.PreferredBackBufferWidth - 50, _graphics.PreferredBackBufferHeight / 2, _pouvoirs);
        _pouvoirs.Add(speedBoost);
    
        var doubleScore = new Pouvoirs(PouvoirsType.DoubleScore, 15.0f);
        doubleScore.GenererPositionAleatoire(_graphics.PreferredBackBufferWidth - 50, _graphics.PreferredBackBufferHeight / 2, _pouvoirs);
        _pouvoirs.Add(doubleScore);
    }

    private Vector2 GetPositionDepart()
    {
        int playerX = _graphics.PreferredBackBufferWidth / 2;
        int playerY = _graphics.PreferredBackBufferHeight - 50;
        return new Vector2(playerX, playerY);
    }

    private void HandleMenu()
    {
        var keyboardState = Keyboard.GetState();
        var keys = keyboardState.GetPressedKeys();
    
        foreach (var key in keys)
        {
            if (_previousKeyboardState.IsKeyUp(key))
            {
                if (key == Keys.Enter && _playerNameBuilder.Length > 0)
                {
                    _playerName = _playerNameBuilder.ToString();
                    _isNameEntered = true;
                    _currentState = GameState.EnJeu;
                    LoadPlayerScores();
                }
                else if (key == Keys.Back && _playerNameBuilder.Length > 0)
                {
                    _playerNameBuilder.Length -= 1;
                }
                else if (key != Keys.Enter && key != Keys.Back)
                {
                    _playerNameBuilder.Append(key.ToString());
                }
            }
        }
    
        _previousKeyboardState = keyboardState;
    }

    protected override void LoadContent()
    {
        // Charger les textures
        _backgroundTexture = Content.Load<Texture2D>("images/space");
        _joueurTexture = Content.Load<Texture2D>("images/ship");
        _blockTexture = Content.Load<Texture2D>("images/asteroid");
        _speedBoostTexture = Content.Load<Texture2D>("images/speed_up");
        _doubleScoreTexture = Content.Load<Texture2D>("images/double_score");

        _font = Content.Load<SpriteFont>("fonts/Game_fonts");

        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

   protected override void Update(GameTime gameTime)
{
    if (_currentState == GameState.Menu)
    {
        HandleMenu();
        return;
    }

    if (_currentState == GameState.GameOver)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.R))
        {
            ResetGame();
        }
        else if (keyboardState.IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        
        return;
    }

    _joueur.Update(gameTime);

    _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
    if (_timer >= 1.0f)
    {
        _score += (int)(10 * _scoreMultiplier);
        _timer -= 1.0f;

        foreach (var pouvoir in _pouvoirs)
        {
            pouvoir.MettreAJour((float)gameTime.ElapsedGameTime.TotalSeconds, _joueur, _joueurTexture);
        }
    }

    if (_scoreMultiplier > 1.0f && !_pouvoirs.Exists(p => p.Type == PouvoirsType.DoubleScore && p.Actif))
    {
        _scoreMultiplier = 1.0f;
    }

    foreach (var block in _blocks)
    {
        block.Update(gameTime);

        if (_joueur.Rect.Intersects(block.Rect))
        {
            HandleCollision();
            break;
        }
    }

    for (int i = 0; i < _pouvoirs.Count; i++)
    {
        var pouvoir = _pouvoirs[i];

        if (_joueur.Rect.Intersects(pouvoir.Rect))
        {
            ActiverPouvoir(pouvoir);
            _pouvoirs.RemoveAt(i);
            break;
        }
    }

    // Mettre à jour le timer de respawn du double score
    _doubleScoreRespawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
    if (_doubleScoreRespawnTimer >= DoubleScoreRespawnInterval)
    {
        RespawnDoubleScore();
        _doubleScoreRespawnTimer = 0f;
    }

    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
    {
        Exit();
    }

    base.Update(gameTime);
}

private void RespawnDoubleScore()
{
    var doubleScore = new Pouvoirs(PouvoirsType.DoubleScore, 5.0f);
    doubleScore.GenererPositionAleatoire(_graphics.PreferredBackBufferWidth - 50, _graphics.PreferredBackBufferHeight / 2, _pouvoirs);
    _pouvoirs.Add(doubleScore);
}

    protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _spriteBatch.Begin();
    _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

    if (_currentState == GameState.Menu)
    {
        // Draw a semi-transparent background for the menu text
        Texture2D backgroundTexture = new Texture2D(GraphicsDevice, 1, 1);
        backgroundTexture.SetData(new[] { Color.Black * 0.7f });
        _spriteBatch.Draw(backgroundTexture, new Rectangle(40, 40, 300, 100), Color.White);

        _spriteBatch.DrawString(_font, "Enter your name:", new Vector2(50, 50), Color.White);
        _spriteBatch.DrawString(_font, _playerNameBuilder.ToString(), new Vector2(50, 100), Color.White);
    }
    else if (_currentState == GameState.EnJeu)
    {
        _joueur.Draw(_spriteBatch);
        foreach (var block in _blocks)
        {
            block.Draw(_spriteBatch);
        }
        foreach (var pouvoir in _pouvoirs)
        {
            if (!pouvoir.Actif)
            {
                var texture = pouvoir.Type switch
                {
                    PouvoirsType.SpeedBoost => _speedBoostTexture,
                    PouvoirsType.DoubleScore => _doubleScoreTexture,
                    _ => null
                };
                pouvoir.Draw(_spriteBatch, texture);
            }
        }
        _spriteBatch.DrawString(_font, $"Score : {_score}", new Vector2(50, 50), Color.White);
    }
    else if (_currentState == GameState.GameOver)
    {
        int cadreLargeur = 400;
        int cadreHauteur = 200;
        int cadreX = (_graphics.PreferredBackBufferWidth - cadreLargeur) / 2;
        int cadreY = (_graphics.PreferredBackBufferHeight - cadreHauteur) / 2;

        Texture2D cadreTexture = new Texture2D(GraphicsDevice, 1, 1);
        cadreTexture.SetData(new[] { Color.Black * 0.7f });
        _spriteBatch.Draw(cadreTexture, new Rectangle(cadreX, cadreY, cadreLargeur, cadreHauteur), Color.White);

        Vector2 textPos1 = new Vector2(cadreX + (cadreLargeur / 2) - (_font.MeasureString($"Last Score: {_lastScore}").X / 2), cadreY + 50);
        Vector2 textPos2 = new Vector2(cadreX + (cadreLargeur / 2) - (_font.MeasureString($"Highest Score: {_highestScore}").X / 2), cadreY + 100);
        Vector2 textPos3 = new Vector2(cadreX + 50, cadreY + 130);

        _spriteBatch.DrawString(_font, "GAME OVER", new Vector2(cadreX + 120, cadreY + 20), Color.Red);
        _spriteBatch.DrawString(_font, $"Last Score: {_lastScore}", textPos1, Color.Yellow);
        _spriteBatch.DrawString(_font, $"Highest Score: {_highestScore}", textPos2, Color.Green);
        _spriteBatch.DrawString(_font, "Appuyez sur R pour rejouer", textPos3, Color.White);
        _spriteBatch.DrawString(_font, "Appuyez sur Echap pour quitter", new Vector2(cadreX + 50, cadreY + 160), Color.White);
    }

    _spriteBatch.End();
    base.Draw(gameTime);
}

    private void HandleCollision()
    {
        var speedBoost = _pouvoirs.Find(p => p.Type == PouvoirsType.SpeedBoost && p.Actif);
        if (speedBoost != null)
        {
            speedBoost.DesactiverPouvoir();
            return;
        }

        Console.WriteLine("Game Over!");
        _lastScore = _score;
        if (_lastScore > _highestScore)
        {
            _highestScore = _lastScore;
        }
        _currentState = GameState.GameOver;

        SavePlayerScores();
    }

   private void ActiverPouvoir(Pouvoirs pouvoir)
   {
       pouvoir.ActiverPouvoir();

       switch (pouvoir.Type)
       {
           case PouvoirsType.DoubleScore:
               Console.WriteLine("Double score activé !");
               _scoreMultiplier = 2.0f; // Double the score multiplier
               _score += 20; // Add 20 points to the score
              
               break;

           case PouvoirsType.SpeedBoost:
               _joueur.SpeedAcc *= 2; // Double the player's acceleration
               _joueur.SpeedDec *= 2; // Double the player's deceleration
               pouvoir.Duree = 10.0f; // Reduce the power duration to 10 seconds
               Console.WriteLine("Double speed activé !");
              
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
        _joueur = new Joueur(_joueurTexture, GetPositionDepart(), 50);

        // Réinitialiser les blocs
        _blocks = Block.InitialiseBlocks(_blockTexture);

        // Réinitialiser les pouvoirs
        _pouvoirs = new List<Pouvoirs>();

        var speedBoost = new Pouvoirs(PouvoirsType.SpeedBoost, 10.0f);
        speedBoost.GenererPositionAleatoire(950, 750, _pouvoirs);
        _pouvoirs.Add(speedBoost);

        var doubleScore = new Pouvoirs(PouvoirsType.DoubleScore, 15.0f);
        doubleScore.GenererPositionAleatoire(950, 750, _pouvoirs);
        _pouvoirs.Add(doubleScore);

        Console.WriteLine("Partie réinitialisée !");
    }

    private void LoadPlayerScores()
    {
        var playerData = PlayerDataManager.LoadPlayerData(_playerName);
        _lastScore = playerData.LastScore;
        _highestScore = playerData.HighestScore;
    }
    private void SavePlayerScores()
    {
        var playerData = PlayerDataManager.LoadPlayerData(_playerName);

        // Update the scores
        playerData.LastScore = _lastScore;
        if (_lastScore > playerData.HighestScore)
        {
            playerData.HighestScore = _lastScore;
        }

        PlayerDataManager.SavePlayerData(playerData);
    }
}
