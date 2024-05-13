/*****************************************************************//**
 * \file   Chess.cs
 * \brief  Implementácia generických funkcionalít a šachových pojmov.
 * 
 * \author Martin Šváb
 * \date   Máj 2024
 *********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Táto trieda slúži podobne ako Common trieda.
 */
public class Chess : MonoBehaviour
{
    /**
     * Enumerátor reprezentujúci typ šachovej figúrky.
     */
    public enum PieceType
    {
        /**
         * Pešiak.
         */
        Pawn = 0,
        /**
         * Rytier.
         */
        Knight = 1,
        /**
         * Strelec.
         */
        Bishop = 2,
        /**
         * Veža.
         */
        Rook = 3,
        /**
         * Kráľovná.
         */
        Queen = 4,
        /**
         * Kráľ.
         */
        King = 5
    }

    /**
     * Enumerátor reprezentujúci esenciu šachovej figúrky
     */
    public enum Essence
    {
        /**
         * Klasická esencia.
         */
        Classic = 0,
        /**
         * Červená esencia.
         */
        Red = 1,
        /**
         * Modrá esencia.
         */
        Blue = 2
    }

    /**
     * Farba šachovej figúrky.
     */
    public enum Color
    {
        /**
         * Biela figúrka.
         */
        White = 0,
        /**
         * Čierna figúrka.
         */
        Black = 1,
        /**
         * Náhodná figúrka.
         */
        Random = 2
    }

    /**
     * Enumerátor reprezentujúci typ mobility.
     */
    public enum MovementType
    {
        /**
         * Tento pohyb umožňuje len presunúť sa na prázdnu pozíciu.
         */
        Move = 0,
        /**
         * Tento pohyb umožňuje len napadnúť protivníkovu figúrku.
         */
        Attack = 1,
        /**
         * Tento pohyb umožňuje presunúť sa na prádznu pozíciu a napadnúť protivníkovu figúrku.
         */
        AttackMove = 2
    }

    /**
     * Enumerátor reprezentujúci typ šachovej inteligencie.
     */
    public enum AI
    {
        /**
         * Základná inteligencia využíva na evaluáciu šachovej pozície ručne nastavenú evaluačnú funkciu.
         */
        Basic = 0,
        /**
         * Základná inteligencia využíva na evaluáciu šachovej pozície súbor CNN modelov.
         */
        Ensemble = 1
    }

    /**
     * Trieda reprezentujúca hernú konfiguráciu.
     */
    public class GameSettings
    {
        /**
         * Typ šachovej inteligencie.
         */
        public AI ai;

        /**
         * Farba hráča.
         */
        public Color playerColor;

        /**
         * Farba Malakha.
         */
        public Color malakhColor;

        /**
         * Esencia hráčovho pešiaka.
         */
        public Essence playerPawn;

        /**
         * Esencia hráčovho rytiera.
         */
        public Essence playerKnight;

        /**
         * Esencia hráčovho strelca.
         */
        public Essence playerBishop;

        /**
         * Esencia hráčovej veže.
         */
        public Essence playerRook;

        /**
         * Esencia Malakhovho pešiaka.
         */
        public Essence malakhPawn;

        /**
         * Esencia Malakhovho rytiera.
         */
        public Essence malakhKnight;

        /**
         * Esencia Malakhovho strelca.
         */
        public Essence malakhBishop;

        /**
         * Esencia Malakhovej veže.
         */
        public Essence malakhRook;

        /**
         * Konštruktor hernej konfigurácie.
         * 
         * \param ai Typ šachovej inteligencie.
         * \param playerColor Farba hráča.
         * \param playerPawn Esencia hráčovho pešiaka.
         * \param playerKnight Esencia hráčovho rytiera.
         * \param playerBishop Esencia hráčovho strelca.
         * \param playerRook Esencia hráčovej veže.
         * \param malakhColor Farba Malakha
         * \param malakhPawn Esencia Malakhovho pešiaka.
         * \param malakhKnight Esencia Malakhovho rytiera.
         * \param malakhBishop Esencia Malakhovho strelca.
         * \param malakhRooks Esencia Malakhovej veže.
         */
        public GameSettings(AI ai, Color playerColor, Essence playerPawn, Essence playerKnight, Essence playerBishop, Essence playerRook,
            Color malakhColor, Essence malakhPawn, Essence malakhKnight, Essence malakhBishop, Essence malakhRook)
        {
            this.ai = ai;

            this.playerColor = playerColor;

            this.playerPawn = playerPawn;
            this.playerKnight = playerKnight;
            this.playerBishop = playerBishop;
            this.playerRook = playerRook;

            this.malakhColor = malakhColor;

            this.malakhPawn = malakhPawn;
            this.malakhKnight = malakhKnight;
            this.malakhBishop = malakhBishop;
            this.malakhRook = malakhRook;
        }
    }

    /**
     * Táto metóda načíta obrázok požadovanej šachovej figúrky.
     * 
     * \param type Typ figúrky.
     * \param color Farba figúrky.
     * \param essence Esencia figúrky.
     * \return Obrázok danej figúrky.
     */
    public static Sprite GetPieceImage(PieceType type, Color color, Essence essence)
    {
        return Resources.Load<Sprite>("Pieces/" + type.ToString() + "_" + color.ToString() + "_" + essence.ToString());
    }

    /**
     * Táto metóda načíta obrázok požadovaného pohybu
     * 
     * \param type Typ pohybu.
     * \return Obrázok daného pohybu.
     */
    public static Sprite GetMovementImage(MovementType type)
    {
        return Resources.Load<Sprite>("Movements/" + type.ToString());
    }
}
