<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
                xmlns:db="http://www.univ-grenoble-alpes.fr/l3miage/dodgeblock">
    <xsl:output method="html" indent="yes"/>

    <!-- Template principal -->
    <xsl:template match="/">
        <html>
            <head>
                <title>Leaderboard - Top 10 Best Games</title>
                <link rel="stylesheet" type="text/css" href="../css/pageJoueurPartie.css" />
            </head>
            <body>
                <h1 class="title">Leaderboard - Top 10 Best Games</h1>
                <table border="1">
                    <thead>
                        <tr>
                            <th>Position</th>
                            <th>Nom du Joueur</th>
                            <th>Date</th>
                            <th>Score</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Trier les parties et afficher les meilleurs scores -->
                        <xsl:apply-templates select="//db:partie">
                            <xsl:sort select="db:score" data-type="number" order="descending"/>
                        </xsl:apply-templates>
                    </tbody>
                </table>
            </body>
        </html>
    </xsl:template>

    <!-- Template pour afficher chaque partie et vérifier si c'est le meilleur score -->
    <xsl:template match="db:partie">
        <xsl:variable name="newScore" select="db:score"/>
        <xsl:variable name="player" select="ancestor::db:player"/>

        <!-- Vérifier si le score actuel est supérieur au meilleur score du joueur -->
        <xsl:if test="$newScore &gt; $player/db:meilleur_score">
            <!-- Mettre à jour le meilleur score du joueur -->
            <xsl:value-of select="concat('Nouveau meilleur score pour ', $player/db:nom, ': ', $newScore)" />
            <xsl:variable name="updatedBestScore" select="$newScore"/>
            <!-- Mettre à jour le score dans le joueur -->
            <xsl:value-of select="ancestor::db:player/db:meilleur_score"/>
        </xsl:if>

        <!-- Limiter l'affichage aux 10 meilleurs scores -->
        <xsl:if test="position() &lt;= 10">
            <tr>
                <td><xsl:value-of select="position()"/></td> <!-- Position -->
                <td><xsl:value-of select="ancestor::db:player/db:nom"/></td> <!-- Nom du joueur -->
                <td><xsl:value-of select="@date"/></td> <!-- Date de la partie -->
                <td><xsl:value-of select="db:score"/></td> <!-- Score -->
            </tr>
        </xsl:if>
    </xsl:template>
</xsl:stylesheet>