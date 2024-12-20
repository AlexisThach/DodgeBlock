```plantuml
title Diagramme de séquence du jeu DodgeBlock 

Joueur -> GameEngine : Entrée pour lancer la partie
GameEngine -> GameController : Initialize()
GameController -> GameController : InitialiseBlock()
GameController -> GameController : Créer les pouvoirs

GameController -> XML : LoadPlayerScores()

loop Partie en cours

    Joueur -> GameController: Entrée clavier 
    GameController -> GameController : Déplacer le vaisseau
    GameController -> GameController : Check les collisions

    alt Collision avec les pouvoirs 

    GameController -> GameController : Augmenter la vitesse de déplacement    
    GameController -> GameController : Doubler le score
    GameController -> XML : Augmenter le score
    else Collision avec les blocs
    GameController -> GameEngine : Game Over
    end
end

alt Game Over

Joueur -> GameEngine : Appuyer sur "R" pour rejouer
GameEngine -> GameController : Réinitialisation du jeu
Joueur -> GameEngine : Appuyer sur "Esc" pour quitter
GameEngine -> GameEngine : Quitter le jeu

end

```