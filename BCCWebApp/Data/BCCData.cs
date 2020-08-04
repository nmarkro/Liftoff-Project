﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BCCWebApp.Data
{
    // Code modified and based on https://therockmanexezone.com/ncgen/ by Prof9 (https://github.com/Prof9)
    public class Chip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MB { get; set; }
        public string IconUrl
        {
            get
            {
                return string.Format("~/images/ncgen/icon/{0}.png", Id);
            }
        }

        public Chip(int id, string name, int mb)
        {
            Id = id;
            Name = name;
            MB = mb;
        }
    }

    public class Operator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SlotIn { get; set; } 

        public Operator(int id, string name, int slotIn)
        {
            Id = id;
            Name = name;
            SlotIn = slotIn;
        }
    }

    public class BCCData
    {
        public static Dictionary<string, int> CharTable = new Dictionary<string, int>
        {
            {"0", 0x01},
            {"1", 0x02},
            {"2", 0x03},
            {"3", 0x04},
            {"4", 0x05},
            {"5", 0x06},
            {"6", 0x07},
            {"7", 0x08},
            {"8", 0x09},
            {"9", 0x0A},
            {"■", 0x0A},
            {"ア", 0x0B},
            {"イ", 0x0C},
            {"ウ", 0x0D},
            {"エ", 0x0E},
            {"オ", 0x0F},
            {"カ", 0x10},
            {"キ", 0x11},
            {"ク", 0x12},
            {"ケ", 0x13},
            {"コ", 0x14},
            {"サ", 0x15},
            {"シ", 0x16},
            {"ス", 0x17},
            {"セ", 0x18},
            {"ソ", 0x19},
            {"タ", 0x1A},
            {"チ", 0x1B},
            {"ツ", 0x1C},
            {"テ", 0x1D},
            {"ト", 0x1E},
            {"ナ", 0x1F},
            {"ニ", 0x20},
            {"ヌ", 0x21},
            {"ネ", 0x22},
            {"ノ", 0x23},
            {"ハ", 0x24},
            {"ヒ", 0x25},
            {"フ", 0x26},
            {"ヘ", 0x27},
            {"ホ", 0x28},
            {"マ", 0x29},
            {"ミ", 0x2A},
            {"ム", 0x2B},
            {"メ", 0x2C},
            {"モ", 0x2D},
            {"ヤ", 0x2E},
            {"ユ", 0x2F},
            {"ヨ", 0x30},
            {"ラ", 0x31},
            {"リ", 0x32},
            {"ル", 0x33},
            {"レ", 0x34},
            {"ロ", 0x35},
            {"ワ", 0x36},
            {"ヲ", 0x39},
            {"ン", 0x3A},
            {"ガ", 0x3B},
            {"ギ", 0x3C},
            {"グ", 0x3D},
            {"ゲ", 0x3E},
            {"ゴ", 0x3F},
            {"ザ", 0x40},
            {"ジ", 0x41},
            {"ズ", 0x42},
            {"ゼ", 0x43},
            {"ゾ", 0x44},
            {"ダ", 0x45},
            {"ヂ", 0x46},
            {"ヅ", 0x47},
            {"デ", 0x48},
            {"ド", 0x49},
            {"バ", 0x4A},
            {"ビ", 0x4B},
            {"ブ", 0x4C},
            {"ベ", 0x4D},
            {"ボ", 0x4E},
            {"パ", 0x4F},
            {"ピ", 0x50},
            {"プ", 0x51},
            {"ペ", 0x52},
            {"ポ", 0x53},
            {"ァ", 0x54},
            {"ィ", 0x55},
            {"ゥ", 0x56},
            {"ェ", 0x57},
            {"ォ", 0x58},
            {"ッ", 0x59},
            {"ャ", 0x5A},
            {"ュ", 0x5B},
            {"ョ", 0x5C},
            {"A", 0x5E},
            {"B", 0x5F},
            {"C", 0x60},
            {"D", 0x61},
            {"E", 0x62},
            {"F", 0x63},
            {"G", 0x64},
            {"H", 0x65},
            {"I", 0x66},
            {"J", 0x67},
            {"K", 0x68},
            {"L", 0x69},
            {"M", 0x6A},
            {"N", 0x6B},
            {"O", 0x6C},
            {"P", 0x6D},
            {"Q", 0x6E},
            {"R", 0x6F},
            {"S", 0x70},
            {"T", 0x71},
            {"U", 0x72},
            {"V", 0x73},
            {"W", 0x74},
            {"X", 0x75},
            {"Y", 0x76},
            {"Z", 0x77},
            {"ー", 0x78},
            {"-", 0x78},
        };

        public static char[] PassChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static List<Chip> Chips = new List<Chip> {
            new Chip(0x00, "",          0),
            new Chip(0x01, "Cannon",    10),
            new Chip(0x02, "HiCannon",  20),
            new Chip(0x03, "M-Cannon",  40),
            new Chip(0x04, "Shotgun",   10),
            new Chip(0x05, "V-Gun",     20),
            new Chip(0x06, "CrossGun",  30),
            new Chip(0x07, "Spreader",  40),
            new Chip(0x08, "Bubbler",   10),
            new Chip(0x09, "Bub-V",     20),
            new Chip(0x0A, "BubCross",  30),
            new Chip(0x0B, "BubSprd",   40),
            new Chip(0x0C, "HeatShot",  10),
            new Chip(0x0D, "Heat-V",    20),
            new Chip(0x0E, "HeatCros",  30),
            new Chip(0x0F, "HeatSprd",  40),
            new Chip(0x10, "MiniBomb",  10),
            new Chip(0x11, "LilBomb",   20),
            new Chip(0x12, "CrosBomb",  40),
            new Chip(0x13, "BigBomb",   60),
            new Chip(0x14, "TreeBom1",  30),
            new Chip(0x15, "TreeBom2",  40),
            new Chip(0x16, "TreeBom3",  50),
            new Chip(0x17, "Sword",     10),
            new Chip(0x18, "WideSwrd",  20),
            new Chip(0x19, "LongSwrd",  20),
            new Chip(0x1A, "FireSwrd",  30),
            new Chip(0x1B, "AquaSwrd",  30),
            new Chip(0x1C, "ElecSwrd",  30),
            new Chip(0x1D, "FireBlde",  40),
            new Chip(0x1E, "AquaBlde",  40),
            new Chip(0x1F, "ElecBlde",  40),
            new Chip(0x20, "StepSwrd",  40),
            new Chip(0x21, "Kunai1",    10),
            new Chip(0x22, "Kunai2",    20),
            new Chip(0x23, "Kunai3",    30),
            new Chip(0x24, "CustSwrd",  40),
            new Chip(0x25, "Muramasa",  70),
            new Chip(0x26, "VarSwrd",   30),
            new Chip(0x27, "Slasher",   50),
            new Chip(0x28, "Shockwav",  10),
            new Chip(0x29, "Sonicwav",  20),
            new Chip(0x2A, "Dynawave",  40),
            new Chip(0x2B, "Quake1",    40),
            new Chip(0x2C, "Quake2",    50),
            new Chip(0x2D, "Quake3",    60),
            new Chip(0x2E, "GutPunch",  30),
            new Chip(0x2F, "ColdPnch",  30),
            new Chip(0x30, "DashAtk",   40),
            new Chip(0x31, "Wrecker",   20),
            new Chip(0x32, "CannBall",  30),
            new Chip(0x33, "DoubNdl",   10),
            new Chip(0x34, "TripNdl",   20),
            new Chip(0x35, "QuadNdl",   30),
            new Chip(0x36, "Trident",   30),
            new Chip(0x37, "Ratton1",   10),
            new Chip(0x38, "Ratton2",   20),
            new Chip(0x39, "Ratton3",   30),
            new Chip(0x3A, "FireRatn",  60),
            new Chip(0x3B, "Tornado",   40),
            new Chip(0x3C, "Twister",   40),
            new Chip(0x3D, "Blower",    40),
            new Chip(0x3E, "Burner",    50),
            new Chip(0x3F, "ZapRing1",  40),
            new Chip(0x40, "ZapRing2",  40),
            new Chip(0x41, "ZapRing3",  40),
            new Chip(0x42, "Satelit1",  20),
            new Chip(0x43, "Satelit2",  30),
            new Chip(0x44, "Satelit3",  40),
            new Chip(0x45, "Spice1",    20),
            new Chip(0x46, "Spice2",    30),
            new Chip(0x47, "Spice3",    40),
            new Chip(0x48, "MagBomb1",  10),
            new Chip(0x49, "MagBomb2",  20),
            new Chip(0x4A, "MagBomb3",  30),
            new Chip(0x4B, "Yo-Yo1",    30),
            new Chip(0x4C, "Yo-Yo2",    40),
            new Chip(0x4D, "Yo-Yo3",    50),
            new Chip(0x4E, "CrsShld1",  40),
            new Chip(0x4F, "CrsShld2",  50),
            new Chip(0x50, "CrsShld3",  60),
            new Chip(0x51, "BrakHamr",  40),
            new Chip(0x52, "ZeusHamr",  60),
            new Chip(0x53, "SloGauge",  20),
            new Chip(0x54, "BrnzFist",  30),
            new Chip(0x55, "SilvFist",  40),
            new Chip(0x56, "GoldFist",  50),
            new Chip(0x57, "PoisMask",  30),
            new Chip(0x58, "PoisFace",  50),
            new Chip(0x59, "Whirlpl",   30),
            new Chip(0x5A, "Blckhole",  50),
            new Chip(0x5B, "Meteor3",   30),
            new Chip(0x5C, "Meteor4",   40),
            new Chip(0x5D, "Meteor5",   50),
            new Chip(0x5E, "Meteor6",   60),
            new Chip(0x5F, "TimeBom1",  30),
            new Chip(0x60, "TimeBom2",  50),
            new Chip(0x61, "TimeBom3",  60),
            new Chip(0x62, "LilCloud",  20),
            new Chip(0x63, "MedCloud",  30),
            new Chip(0x64, "BigCloud",  40),
            new Chip(0x65, "Mine",      30),
            new Chip(0x66, "FrntSnsr",  50),
            new Chip(0x67, "DblSnsr",   50),
            new Chip(0x68, "Remobit1",  30),
            new Chip(0x69, "Remobit2",  40),
            new Chip(0x6A, "Remobit3",  50),
            new Chip(0x6B, "AquaBall",  30),
            new Chip(0x6C, "ElecBall",  30),
            new Chip(0x6D, "HeatBall",  30),
            new Chip(0x6E, "Geyser",    40),
            new Chip(0x6F, "LavaDrag",  50),
            new Chip(0x70, "GodStone",  50),
            new Chip(0x71, "OldWood",   50),
            new Chip(0x72, "Guard",     20),
            new Chip(0x73, "Catcher",   20),
            new Chip(0x74, "Mindbndr",  10),
            new Chip(0x75, "Recov10",   0),
            new Chip(0x76, "Recov30",   0),
            new Chip(0x77, "Recov50",   10),
            new Chip(0x78, "Recov80",   20),
            new Chip(0x79, "Recov120",  20),
            new Chip(0x7A, "Recov150",  30),
            new Chip(0x7B, "Recov200",  40),
            new Chip(0x7C, "Recov300",  40),
            new Chip(0x7D, "AirShoes",  20),
            new Chip(0x7E, "Candle1",   10),
            new Chip(0x7F, "Candle2",   20),
            new Chip(0x80, "Candle3",   40),
            new Chip(0x81, "RockCube",  10),
            new Chip(0x82, "Prism",     30),
            new Chip(0x83, "Guardian",  70),
            new Chip(0x84, "Wind",      20),
            new Chip(0x85, "Fan",       20),
            new Chip(0x86, "Anubis",    60),
            new Chip(0x87, "Invis1",    20),
            new Chip(0x88, "Invis2",    30),
            new Chip(0x89, "Invis3",    40),
            new Chip(0x8A, "DropDown",  40),
            new Chip(0x8B, "PopUp",     50),
            new Chip(0x8C, "StoneBod",  30),
            new Chip(0x8D, "Shadow1",   40),
            new Chip(0x8E, "Shadow2",   50),
            new Chip(0x8F, "Shadow3",   60),
            new Chip(0x90, "UnderSht",  20),
            new Chip(0x91, "Barrier",   30),
            new Chip(0x92, "BblWrap",   30),
            new Chip(0x93, "LeafShld",  30),
            new Chip(0x94, "AquaAura",  50),
            new Chip(0x95, "FireAura",  50),
            new Chip(0x96, "WoodAura",  50),
            new Chip(0x97, "ElecAura",  50),
            new Chip(0x98, "LifeAur1",  40),
            new Chip(0x99, "LifeAur2",  50),
            new Chip(0x9A, "LifeAur3",  60),
            new Chip(0x9B, "Jealousy",  60),
            new Chip(0x9C, "AntiFire",  50),
            new Chip(0x9D, "AntiElec",  50),
            new Chip(0x9E, "AntiWatr",  50),
            new Chip(0x9F, "AntiDmg",   50),
            new Chip(0xA0, "AntiSwrd",  40),
            new Chip(0xA1, "FstGauge",  20),
            new Chip(0xA2, "AntiRecv",  40),
            new Chip(0xA3, "Atk+10",    10),
            new Chip(0xA4, "Atk+20",    10),
            new Chip(0xA5, "Atk+30",    20),
            new Chip(0xA6, "Fire+40",   20),
            new Chip(0xA7, "Aqua+40",   20),
            new Chip(0xA8, "Wood+40",   20),
            new Chip(0xA9, "Elec+40",   20),
            new Chip(0xAA, "Navi+20",   10),
            new Chip(0xAB, "Navi+40",   20),
            new Chip(0xAC, "BgRedWav",  80),
            new Chip(0xAD, "FreezBom",  40),
            new Chip(0xAE, "Sparker",   70),
            new Chip(0xAF, "GaiaSwrd",  70),
            new Chip(0xB0, "BlkBomb",   50),
            new Chip(0xB1, "FtrSword",  40),
            new Chip(0xB2, "KngtSwrd",  50),
            new Chip(0xB3, "HeroSwrd",  70),
            new Chip(0xB4, "Meteors",   80),
            new Chip(0xB5, "Poltrgst",  50),
            new Chip(0xB6, "PanlGrab",  0),
            new Chip(0xB7, "AreaGrab",  10),
            new Chip(0xB8, "LavaStge",  20),
            new Chip(0xB9, "IceStage",  20),
            new Chip(0xBA, "GrassStg",  20),
            new Chip(0xBB, "AlumiStg",  20),
            new Chip(0xBC, "Geddon",    20),
            new Chip(0xBD, "PanlOut",   20),
            new Chip(0xBE, "Repair",    20)
        };

        public static List<Chip> NaviChips = new List<Chip>
        {
            new Chip(0xC8, "MegaMan",   330),
            new Chip(0xC9, "Roll",      300),
            new Chip(0xCA, "GutsMan",   290),
            new Chip(0xCB, "ProtoMan",  320),
            new Chip(0xCC, "TurboMan",  320),
            new Chip(0xCD, "Ring",      310),
            new Chip(0xCE, "BassGS",    330),
            new Chip(0xCF, "Bass",      300),
            new Chip(0xD0, "IceMan",    290),
            new Chip(0xD1, "FireMan",   290),
            new Chip(0xD2, "ElecMan",   290),
            new Chip(0xD3, "WoodMan",   290),
            new Chip(0xD4, "SkullMan",  310),
            new Chip(0xD5, "NumbrMan",  280),
            new Chip(0xD6, "AirMan",    300),
            new Chip(0xD7, "QuickMan",  310),
            new Chip(0xD8, "ThunMan",   280),
            new Chip(0xD9, "GateMan",   290),
            new Chip(0xDA, "SharkMan",  280),
            new Chip(0xDB, "ShadoMan",  290),
            new Chip(0xDC, "KnightMn",  300),
            new Chip(0xDD, "MagnetMn",  280),
            new Chip(0xDE, "FreezeMn",  280),
            new Chip(0xDF, "SnakeMan",  280),
            new Chip(0xE0, "ToadMan",   290),
            new Chip(0xE1, "HeatMan",   280),
            new Chip(0xE2, "ColorMan",  330),
            new Chip(0xE3, "MagicMan",  340),
            new Chip(0xE4, "FlashMan",  270),
            new Chip(0xE5, "BeastMan",  290),
            new Chip(0xE6, "PlantMan",  270),
            new Chip(0xE7, "FlameMan",  270),
            new Chip(0xE8, "MetalMan",  300),
            new Chip(0xE9, "KingMan",   290),
            new Chip(0xEA, "HubStyl",   510),
            new Chip(0xEB, "HeatGuts",  310),
            new Chip(0xEC, "ElecTeam",  310),
            new Chip(0xED, "WoodShld",  310),
            new Chip(0xEE, "AquaCust",  310),
            new Chip(0xEF, "NormNav1",  330),
            new Chip(0xF0, "NormNav2",  340),
            new Chip(0xF1, "NormNav3",  350),
            new Chip(0xF2, "NormNav4",  360),
            new Chip(0xF3, "NormNav5",  380),
            new Chip(0xF4, "Navi-F",    320),
            new Chip(0xF5, "Navi-A",    320),
            new Chip(0xF6, "Navi-W",    320),
            new Chip(0xF7, "Navi-E",    320),
            new Chip(0xF8, "NormNavX",  370)
        };

        public static List<Operator> Operators = new List<Operator>
        {
            new Operator(0x32, "Lan (MegaMan)", 60),
            new Operator(0x33, "Mayl (Roll)", 70),
            new Operator(0x34, "Dex (GutsMan)", 30),
            new Operator(0x35, "Chaud (ProtoMan)", 70),
            new Operator(0x36, "Kai (TurboMan)", 80),
            new Operator(0x37, "Mary (Ring)", 80),
            new Operator(0x38, "Bass", 50)
        };

        public static Dictionary<string, int> CodeTypes = new Dictionary<string, int>
        {
            { "WonderSwan", 0x00 },
            { "Game Boy Advance", 0x01 },
            { "Promotional", 0x02 },
        };
    }
}