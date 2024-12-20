```plantuml

package dodgeblock {
    class MyGame {
    -_joueur : Joueur
    -_blocks : ListeBlock
    -_pouvoirs : ListePouvoirs
    -_score : int
    -_timer : float
    -_currentState : GameState
    +MyGame()
    -GetPositionDepart() : void
    -HandleMenu() : void
    -RespawnDoubleScore() : void
    #Initialize() : void
    #LoadContent() : void
    #Update() : void
    #Draw() : void
    -HandleCollision() : void
    -ActiverPouvoir() : void
    -ResetGame() : void
    -LoadPlayerScores() : void
    -SavePlayerScores() : void
    }
    
    class Joueur {
    -_texture : Texture2D
    #_position : Vector2
    -_size : int
    -_speed : Vector2
    -_speedAcc : float
    -_speedDec : float
    +Joueur(texture: Texture2D, position: Vector2, size: int)
    +SpeedAcc : float
    +SpeedDec : float
    +Texture : Texture2D
    +Rect : Rectangle
    +Update(gameTime : GameTime) : void
    +Draw(spriteBatch : SpriteBatch) : void
    +ChangerApparence(nouvelleTexture: Texture2D) : void
    }
    
    class ListeBlock {
    _blocks : Block[]
    }
    
    class Block {
    -_texture : Texture2D
    -_position : Vector2
    -_size : int
    -_speed : float
    +Block(texture: Texture2D, position: Vector2, size: int, speed: float)
    +InitialiseBlocks(blockTexture: Texture2D) : List<Block>
    +Update(gameTime : GameTime) : void
    +Draw(spriteBatch : SpriteBatch) : void 
    +ResetPosition() : void 
    }
    
    class ListePouvoirs {
    _pouvoirs : Pouvoirs[]
    }
    
    class Pouvoirs {
    +Type : PouvoirsType
    +Aduree : float
    +Aactif : bool
    +PositionX : int
    +PositionY : int
    -_tempsRestant : float
    +Pouvoirs(type: PouvoirsType, duree: float)
    +GenererPositionAleatoire(largeurMax : int, hauteurMax : int) : void
    +ActiverPouvoir() : void
    +DesactiverPouvoir() : void 
    +MettreAJour(deltaTime : float, joueur : Joueur, baseTexture : Texture2D) : void
    +Draw(spriteBatch : SpriteBatch, texture : Texture2D) : void 
    }
    
    enum GameState{
    Menu
    EnJeu
    GameOver
    }
    
    enum PouvoirsType {
    SpeedBoost
    SpeedDown
    DoubleScore
    }

    MyGame *-- Joueur
    MyGame *-- ListeBlock
    MyGame *-- ListePouvoirs
    MyGame *-- GameState
    ListeBlock *-- Block
    ListePouvoirs *-- Pouvoirs
    Pouvoirs *-- PouvoirsType
}

```