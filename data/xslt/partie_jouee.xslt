<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:player="http://www.univ-grenoble-alpes.fr/l3miage/player">
    <!-- Paramètres pour définir la structure de sortie -->
    <xsl:output method="html" indent="yes" encoding="UTF-8" />

    <!-- Ajout de styles CSS -->
    <xsl:template name="styles">
        <style type="text/css">
            body {
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            color: #333;
            margin: 20px;
            }
            table {
            width: 80%;
            border-collapse: collapse;
            margin: 20px auto;
            }
            th, td {
            border: 1px solid #ccc;
            padding: 10px;
            text-align: left;
            }
            th {
            color: blue;
            background-color: #f2f2f2;
            }
            h1 {
            text-align: center;
            color: #444;
            }
        </style>
    </xsl:template>

    <!-- Template principal qui s'applique à l'élément racine -->
    <xsl:template match="/player:players">
        <html>
            <head>
                <title>Liste des Parties</title>
                <xsl:call-template name="styles" />
            </head>
            <body>
                <h1>Liste des Parties</h1>
                <table>
                    <thead>
                        <tr>
                            <th>Nom</th>
                            <th>Date</th>
                            <th>Score</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Parcours de tous les joueurs et parties -->
                        <xsl:for-each select="player:player/player:parties/player:partie">
                            <tr>
                                <td>
                                    <xsl:value-of select="ancestor::player:player/player:nom" />
                                </td>
                                <td>
                                    <xsl:value-of select="@date" />
                                </td>
                                <td>
                                    <xsl:value-of select="player:score" />
                                </td>
                            </tr>
                        </xsl:for-each>
                    </tbody>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>
