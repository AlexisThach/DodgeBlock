<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
                xmlns:sa="http://www.univ-grenoble-alpes.fr/l3miage/sauvegarde"
                xmlns:db="http://www.univ-grenoble-alpes.fr/l3miage/player"
    <xsl:output method="html" indent="yes"/>

    <!-- Racine -->
    <xsl:template match="/">
        <html>
            <head>
                <title>Profil du joueur et ses parties</title>
                <style>
                    body { font-family: Arial, sans-serif; margin: 20px; }
                    table { border-collapse: collapse; width: 100%; }
                    th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
                    th { background-color: #f4f4f4; }
                </style>
            </head>
            <body>
                <h1>Profil du Joueur</h1>
                <xsl:apply-templates select="//player"/>
                <h2>Parties sauvegardées</h2>
                <xsl:apply-templates select="//sauvegarde"/>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="/">
    
        <html>
            <head>
                <title>Profil ><xsl:value-of select='concat(//db:player/db:firstname, " ", ///db:player/db:lastname)'/></title>
                <link rel="stylesheet" type="text/css" href="../css/pageJoueurPartie.css" />
            </head>
            <body>
                <div class="header">
                    <h1 class="title"><xsl:value-of select='concat(//db:player/db:firstname, " ", //db:player/db:lastname)'/></h1>
                </div>
                <table border="1">
                    <tr>
                        <th>Nom</th>
                        <th>Prénom</th>
                        <th>Partie(s)</th>
                    </tr>
                    <xsl:apply-templates select="//ci:patient"/>
                </table>
            </body>
        </html>
        </xsl:template>

    <!-- Profil du joueur -->
    <xsl:template match="player">
        <div>
            <p><strong>Prénom :</strong> <xsl:value-of select="firstName"/></p>
            <p><strong>Nom :</strong> <xsl:value-of select="lastName"/></p>
            <p><strong>Position X :</strong> <xsl:value-of select="posX"/></p>
            <p><strong>Position Y :</strong> <xsl:value-of select="posY"/></p>
            <p><strong>Vitesse :</strong> <xsl:value-of select="speed"/></p>
        </div>
    </xsl:template>

    <!-- Liste des parties -->
    <xsl:template match="sauvegarde">
        <table>
            <thead>
                <tr>
                    <th>Score</th>
                    <th>Date de sauvegarde</th>
                    <th>Niveau de difficulté</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><xsl:value-of select="general/score"/></td>
                    <td><xsl:value-of select="general/saveDate"/></td>
                    <td><xsl:value-of select="general/difficultyLevel"/></td>
                </tr>
            </tbody>
        </table>
    </xsl:template>
</xsl:stylesheet> 