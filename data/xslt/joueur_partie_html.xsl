<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
                xmlns:db="http://www.univ-grenoble-alpes.fr/l3miage/player">
    <xsl:output method="html" indent="yes"/>

    <!-- Template principal -->
    <xsl:template match="/">
        <html>
            <head>
                <title>Profil <xsl:value-of select="//db:player/db:nom"/></title>
                <link rel="stylesheet" type="text/css" href="../css/pageJoueurPartie.css" />
            </head>
            <body>
                <h1 class="title">
                    Profil de <xsl:value-of select="//db:player/db:nom"/>
                </h1>
                <table border="1">
                    <tr>
                        <th>Partie(s)</th>
                    </tr>
                    <!-- Application des templates pour chaque joueur -->
                    <xsl:apply-templates select="//db:players/db:player"/>
                </table>
            </body>
        </html>
    </xsl:template>

    <!-- Template pour chaque joueur -->
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