```plantuml

package dodgeblock {
    class MyGame {
    -_ship : Joueur
    -_blocks : ListeBlock
    -_pouvoirs : ListePouvoirs
    -_score : int
    -_timer : float
    -_currentState : GameState
    +MyGame()
    -void GetPositionDepart()
    #Initialize() : void
    #LoadContent() : void
    #Update() : void
    #Draw() : void
    +HandleCollision() : void
    -ActiverPouvoir() : void
    -ResetGame() : void
    }
    
    class Joueur {
    -_texture : Texture2D
    #_position : Vector2
    -_size : int
    -_speed : Vector2
    -_speedAcc : float
    -_speedDec : float
    +Joueur(texture: Texture2D, position: Vector2, size: int)
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
    +LoadContent(content: ContentManager) : void
    +Update(gameTime : GameTime) : void
    +Draw(spriteBatch : SpriteBatch) : void 
    +ResetPosition() : void 
    }
    
    class ListePouvoirs {
    _pouvoirs : Pouvoirs[]
    }
    
    class Pouvoirs {
    +Type : PouvoirsType
    -_duree : float
    -_actif : bool
    -_positionX : int
    -_positionY : int
    -_tempsRestant : float
    +Pouvoirs(type: PouvoirsType, duree: float)
    +GenererPositionAleatoire(largeurMax : int, hauteurMax : int) : void
    +ActiverPouvoir() : void
    +DesactiverPouvoir() : void 
    +MettreAJour(deltaTime : float, joueur : Joueur) : void
    +Draw(spriteBatch : SpriteBatch, texture : Texture2D) : void 
    }
    
    enum GameState{
    EnJeu
    GameOver
    }
    
    enum PouvoirsType {
    bouclier
    invincible
    doubleScore
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