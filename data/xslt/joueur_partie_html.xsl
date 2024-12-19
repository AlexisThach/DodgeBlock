<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
                xmlns:db="http://www.univ-grenoble-alpes.fr/l3miage/dodgeblock">
    <xsl:output method="html" indent="yes"/>

    <!-- Paramètre pour identifier le joueur par son nom -->
    <xsl:param name="nomJoueur" select="'Thyd'"/>
    
    <!-- Template principal -->
    <xsl:template match="/">
        <html>
            <head>
                <title>Profil de <xsl:value-of select="//db:player[db:nom = $nomJoueur]/db:nom"/></title>
                <link rel="stylesheet" type="text/css" href="../css/pageJoueurPartie.css" />
            </head>
            <body>
                <h1 class="title">
                    Profil de <xsl:value-of select="//db:player[db:nom = $nomJoueur]/db:nom"/>
                </h1>
                <table border="1">
                    <tr>
                        <th>Partie(s)</th>
                    </tr>
                    <!-- Application des templates pour le joueur spécifique -->
                    <xsl:apply-templates select="//db:player[db:nom = $nomJoueur]"/>
                </table>
            </body>
        </html>
    </xsl:template>

    <!-- Template pour le joueur -->
    <xsl:template match="db:player">
        <tr>
            <!-- Affichage des parties -->
            <td>
                <xsl:for-each select="db:parties/db:partie">
                    <p>
                        <!-- Affichage de la date et du score de chaque partie -->
                        Date: <xsl:value-of select="@date"/> - Score:
                        <xsl:value-of select="db:score"/>
                    </p>
                </xsl:for-each>
            </td>
        </tr>
    </xsl:template>
</xsl:stylesheet>