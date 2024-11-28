```plantuml

class Game {
    player : Player
    screen : Screen
    listePowerUps : ListePowerUps
    listeObstacle : ListeObstacle
    score : int
    isRunning: bool
    void startGame()
    void pauseGame()
    void endGame()
    void updateGame()
    void afficherScore()
    void checkPowerUpCollision()
}

class Player {
    positionX : int
    positionY : int
    speed : int
    void deplace()
    bool isCollidingWithObstacle()
    void updatePosition()
}

class ListeObstacle {
    obstacle : Obstacle[]
}

class Obstacle {
    positionX : int
    positionY : int
    speed : int
    void tombe()
    bool isOutOfScreen()
    void updatePosition()
}

class Screen {
    width : int
    height : int
    void render()
    void clearScreen()
    void drawPlayer(player: Player)
    void drawObstacles(blocks: List<Block> )
    void drawPowerUps(powerUps: List<PowerUps> )
    void drawScore(score: int)
}

class ListePowerUps {
    power_ups : PowerUps[]
}

class PowerUps {
    type : PowerUpsType
    duree : float
    estActive : bool
    positionX : int
    positionY : int
    void activate(player: Player)
    void deactivate(player: Player)
    void update(durationTime: float)
}

enum PowerUpsType {
    bouclier
    invincible
    doubleScore
}

Game *-- Screen
Game *-- Player
Game *-- ListeObstacle
Game *-- ListePowerUps
ListeObstacle *-- Obstacle
ListePowerUps *-- PowerUps
PowerUps *-- PowerUpsType

```