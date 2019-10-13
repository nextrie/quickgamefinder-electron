using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOA_DEVServer.Traitement
{
    public class Game
    {
        public static string GetNameByID(string name)
        {
            string toreturn = "";

            switch (name)
            {
                case "AR3":
                    toreturn = "ARMA 3";
                    break;
                case "BF4":
                    toreturn = "BATTLEFIELD 4";
                    break;
                case "BF5":
                    toreturn = "BATTLEFIELD 5";
                    break;
                case "BO2":
                    toreturn = "BLACK OPS 2";
                    break;
                case "BO3":
                    toreturn = "BLACK OPS 3";
                    break;
                case "BO4":
                    toreturn = "BLACK OPS 4";
                    break;
                case "BST": // business tour
                    toreturn = "BUSINESS TOUR";
                    break;
                case "CGO": // CSGO
                    toreturn = "COUNTER STRIKE: GLOBAL OFFENSIVE";
                    break;
                case "DS2": // DESTINY 2
                    toreturn = "DESTINY 2";
                    break;
                case "DB3": // Diablo 3
                    toreturn = "DIABLO 3";
                    break;
                case "DOO": // DOOM
                    toreturn = "DOOM";
                    break;
                case "FL4":
                    toreturn = "FALLOUT4";
                    break;
                case "F18":
                    toreturn = "FIFA 18";
                    break;
                case "F19":
                    toreturn = "FIFA 19";
                    break;
                case "FRT":
                    toreturn = "FORTNITE";
                    break;
                case "GMD":
                    toreturn = "GARRY'S MOD";
                    break;
                case "GT5":
                    toreturn = "GRAND THEFT AUTO 5";
                    break;
                case "LOL":
                    toreturn = "LEAGUE OF LEGEND";
                    break;
                case "MCR":
                    toreturn = "MINECRAFT";
                    break;
                case "NBA":
                    toreturn = "NBA 2K18";
                    break;
                case "OVW":
                    toreturn = "OVERWATCH";
                    break;
                case "PD2":
                    toreturn = "PAYDAY 2";
                    break;
                case "PBG":
                    toreturn = "PLAYER UNKNOWN'S BATLLEGROUNDS";
                    break;
                case "RD2":
                    toreturn = "RED DEAD REDEMPTION 2";
                    break;
                case "RLG":
                    toreturn = "ROCKET LEAGUE";
                    break;
                case "SPR":
                    toreturn = "SPEEDRUNNERS";
                    break;
                case "TC2":
                    toreturn = "THE CREW 2";
                    break;
                case "RB6":
                    toreturn = "TOM CLANCY'S RAINBOW SIX SIEGE";
                    break;
                case "WOW":
                    toreturn = "WORLD OF WARCRAFT";
                    break;
            }
            return toreturn;
        }
        public static string GetIDByGame(string name)
        {
            string toreturn = "";

            switch (name)
            {
                case "ARMA 3":
                    toreturn = "AR3";
                    break;
                case "BATTLEFIELD 4":
                    toreturn = "BF4";
                    break;
                case "BATTLEFIELD 5":
                    toreturn = "BF5";
                    break;
                case "BLACK OPS 2":
                    toreturn = "BO2";
                    break;
                case "BLACK OPS 3":
                    toreturn = "BO3";
                    break;
                case "BLACK OPS 4":
                    toreturn = "BO4";
                    break;
                case "BUSINESS TOUR": // business tour
                    toreturn = "BST";
                    break;
                case "COUNTER STRIKE: GLOBAL OFFENSIVE": // CSGO
                    toreturn = "CGO";
                    break;
                case "DESTINY 2": // DESTINY 2
                    toreturn = "DS2";
                    break;
                case "DIABLO 3": // Diablo 3
                    toreturn = "DB3";
                    break;
                case "DOOM": // DOOM
                    toreturn = "DOO";
                    break;
                case "FALLOUT4":
                    toreturn = "FL4";
                    break;
                case "FIFA 18":
                    toreturn = "F18";
                    break;
                case "FIFA 19":
                    toreturn = "F19";
                    break;
                case "FORTNITE":
                    toreturn = "FRT";
                    break;
                case "GARRY'S MOD":
                    toreturn = "GMD";
                    break;
                case "GRAND THEFT AUTO 5":
                    toreturn = "GT5";
                    break;
                case "LEAGUE OF LEGEND":
                    toreturn = "LOL";
                    break;
                case "MINECRAFT":
                    toreturn = "MCR";
                    break;
                case "NBA 2K18":
                    toreturn = "NBA";
                    break;
                case "OVERWATCH":
                    toreturn = "OVW";
                    break;
                case "PAYDAY 2":
                    toreturn = "PD2";
                    break;
                case "PLAYER UNKNOWN'S BATLLEGROUNDS":
                    toreturn = "PBG";
                    break;
                case "RED DEAD REDEMPTION 2":
                    toreturn = "RD2";
                    break;
                case "ROCKET LEAGUE":
                    toreturn = "RLG";
                    break;
                case "SPEEDRUNNERS":
                    toreturn = "SPR";
                    break;
                case "THE CREW 2":
                    toreturn = "TC2";
                    break;
                case "TOM CLANCY'S RAINBOW SIX SIEGE":
                    toreturn = "R86";
                    break;
                case "WORLD OF WARCRAFT":
                    toreturn = "WOW";
                    break;
            }
            return toreturn;
        }
    }
}
