using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public struct Colour : IPenContext
    {
        public readonly int Value;

        public Colour(int value)
        {
            Value = value;
        }

        IBufferPen IPenContext.ToPen(int? width, int? height) =>
            ToPen(width ?? 1, height ?? 1);
        public ISolidPen ToPen( int width,  int height) =>
            Factory.newPen(Value, width, height);
        public ISolidPen ToPen() =>
            Factory.newPen(Value, 1000, 1000);

        public static implicit operator int(Colour color) =>
            color.Value;

        public static implicit operator Colour(int value) =>
            new Colour(value);

        #region colors
        public static Colour Empty { get; private set; }
        public static Colour ActiveBorder { get; private set; }
        public static Colour ActiveCaption { get; private set; }
        public static Colour ActiveCaptionText { get; private set; }
        public static Colour AppWorkspace { get; private set; }
        public static Colour Control { get; private set; }
        public static Colour ControlDark { get; private set; }
        public static Colour ControlDarkDark { get; private set; }
        public static Colour ControlLight { get; private set; }
        public static Colour ControlLightLight { get; private set; }
        public static Colour ControlText { get; private set; }
        public static Colour Desktop { get; private set; }
        public static Colour GrayText { get; private set; }
        public static Colour Highlight { get; private set; }
        public static Colour HighlightText { get; private set; }
        public static Colour HotTrack { get; private set; }
        public static Colour InactiveBorder { get; private set; }
        public static Colour InactiveCaption { get; private set; }
        public static Colour InactiveCaptionText { get; private set; }
        public static Colour Info { get; private set; }
        public static Colour InfoText { get; private set; }
        public static Colour Menu { get; private set; }
        public static Colour MenuText { get; private set; }
        public static Colour ScrollBar { get; private set; }
        public static Colour Window { get; private set; }
        public static Colour WindowFrame { get; private set; }
        public static Colour WindowText { get; private set; }
        public static Colour Transparent { get; private set; }
        public static Colour AliceBlue { get; private set; }
        public static Colour AntiqueWhite { get; private set; }
        public static Colour Aqua { get; private set; }
        public static Colour Aquamarine { get; private set; }
        public static Colour Azure { get; private set; }
        public static Colour Beige { get; private set; }
        public static Colour Bisque { get; private set; }
        public static Colour Black { get; private set; }
        public static Colour BlanchedAlmond { get; private set; }
        public static Colour Blue { get; private set; }
        public static Colour BlueViolet { get; private set; }
        public static Colour Brown { get; private set; }
        public static Colour BurlyWood { get; private set; }
        public static Colour CadetBlue { get; private set; }
        public static Colour Chartreuse { get; private set; }
        public static Colour Chocolate { get; private set; }
        public static Colour Coral { get; private set; }
        public static Colour CornflowerBlue { get; private set; }
        public static Colour Cornsilk { get; private set; }
        public static Colour Crimson { get; private set; }
        public static Colour Cyan { get; private set; }
        public static Colour DarkBlue { get; private set; }
        public static Colour DarkCyan { get; private set; }
        public static Colour DarkGoldenrod { get; private set; }
        public static Colour DarkGray { get; private set; }
        public static Colour DarkGreen { get; private set; }
        public static Colour DarkKhaki { get; private set; }
        public static Colour DarkMagenta { get; private set; }
        public static Colour DarkOliveGreen { get; private set; }
        public static Colour DarkOrange { get; private set; }
        public static Colour DarkOrchid { get; private set; }
        public static Colour DarkRed { get; private set; }
        public static Colour DarkSalmon { get; private set; }
        public static Colour DarkSeaGreen { get; private set; }
        public static Colour DarkSlateBlue { get; private set; }
        public static Colour DarkSlateGray { get; private set; }
        public static Colour DarkTurquoise { get; private set; }
        public static Colour DarkViolet { get; private set; }
        public static Colour DeepPink { get; private set; }
        public static Colour DeepSkyBlue { get; private set; }
        public static Colour DimGray { get; private set; }
        public static Colour DodgerBlue { get; private set; }
        public static Colour Firebrick { get; private set; }
        public static Colour FloralWhite { get; private set; }
        public static Colour ForestGreen { get; private set; }
        public static Colour Fuchsia { get; private set; }
        public static Colour Gainsboro { get; private set; }
        public static Colour GhostWhite { get; private set; }
        public static Colour Gold { get; private set; }
        public static Colour Goldenrod { get; private set; }
        public static Colour Gray { get; private set; }
        public static Colour Green { get; private set; }
        public static Colour GreenYellow { get; private set; }
        public static Colour Honeydew { get; private set; }
        public static Colour HotPink { get; private set; }
        public static Colour IndianRed { get; private set; }
        public static Colour Indigo { get; private set; }
        public static Colour Ivory { get; private set; }
        public static Colour Khaki { get; private set; }
        public static Colour Lavender { get; private set; }
        public static Colour LavenderBlush { get; private set; }
        public static Colour LawnGreen { get; private set; }
        public static Colour LemonChiffon { get; private set; }
        public static Colour LightBlue { get; private set; }
        public static Colour LightCoral { get; private set; }
        public static Colour LightCyan { get; private set; }
        public static Colour LightGoldenrodYellow { get; private set; }
        public static Colour LightGray { get; private set; }
        public static Colour LightGreen { get; private set; }
        public static Colour LightPink { get; private set; }
        public static Colour LightSalmon { get; private set; }
        public static Colour LightSeaGreen { get; private set; }
        public static Colour LightSkyBlue { get; private set; }
        public static Colour LightSlateGray { get; private set; }
        public static Colour LightSteelBlue { get; private set; }
        public static Colour LightYellow { get; private set; }
        public static Colour Lime { get; private set; }
        public static Colour LimeGreen { get; private set; }
        public static Colour Linen { get; private set; }
        public static Colour Magenta { get; private set; }
        public static Colour Maroon { get; private set; }
        public static Colour MediumAquamarine { get; private set; }
        public static Colour MediumBlue { get; private set; }
        public static Colour MediumOrchid { get; private set; }
        public static Colour MediumPurple { get; private set; }
        public static Colour MediumSeaGreen { get; private set; }
        public static Colour MediumSlateBlue { get; private set; }
        public static Colour MediumSpringGreen { get; private set; }
        public static Colour MediumTurquoise { get; private set; }
        public static Colour MediumVioletRed { get; private set; }
        public static Colour MidnightBlue { get; private set; }
        public static Colour MintCream { get; private set; }
        public static Colour MistyRose { get; private set; }
        public static Colour Moccasin { get; private set; }
        public static Colour NavajoWhite { get; private set; }
        public static Colour Navy { get; private set; }
        public static Colour OldLace { get; private set; }
        public static Colour Olive { get; private set; }
        public static Colour OliveDrab { get; private set; }
        public static Colour Orange { get; private set; }
        public static Colour OrangeRed { get; private set; }
        public static Colour Orchid { get; private set; }
        public static Colour PaleGoldenrod { get; private set; }
        public static Colour PaleGreen { get; private set; }
        public static Colour PaleTurquoise { get; private set; }
        public static Colour PaleVioletRed { get; private set; }
        public static Colour PapayaWhip { get; private set; }
        public static Colour PeachPuff { get; private set; }
        public static Colour Peru { get; private set; }
        public static Colour Pink { get; private set; }
        public static Colour Plum { get; private set; }
        public static Colour PowderBlue { get; private set; }
        public static Colour Purple { get; private set; }
        public static Colour Red { get; private set; }
        public static Colour RosyBrown { get; private set; }
        public static Colour RoyalBlue { get; private set; }
        public static Colour SaddleBrown { get; private set; }
        public static Colour Salmon { get; private set; }
        public static Colour SandyBrown { get; private set; }
        public static Colour SeaGreen { get; private set; }
        public static Colour SeaShell { get; private set; }
        public static Colour Sienna { get; private set; }
        public static Colour Silver { get; private set; }
        public static Colour SkyBlue { get; private set; }
        public static Colour SlateBlue { get; private set; }
        public static Colour SlateGray { get; private set; }
        public static Colour Snow { get; private set; }
        public static Colour SpringGreen { get; private set; }
        public static Colour SteelBlue { get; private set; }
        public static Colour Tan { get; private set; }
        public static Colour Teal { get; private set; }
        public static Colour Thistle { get; private set; }
        public static Colour Tomato { get; private set; }
        public static Colour Turquoise { get; private set; }
        public static Colour Violet { get; private set; }
        public static Colour Wheat { get; private set; }
        public static Colour White { get; private set; }
        public static Colour WhiteSmoke { get; private set; }
        public static Colour Yellow { get; private set; }
        public static Colour YellowGreen { get; private set; }
        public static Colour ButtonFace { get; private set; }
        public static Colour ButtonHighlight { get; private set; }
        public static Colour ButtonShadow { get; private set; }
        public static Colour GradientActiveCaption { get; private set; }
        public static Colour GradientInactiveCaption { get; private set; }
        public static Colour MenuBar { get; private set; }
        public static Colour MenuHighlight { get; private set; }
        #endregion

        public static implicit operator RGBA(Colour c)=>
            new RGBA(c);
        internal static void Reset()
        {
            Empty = Colours.ToColor(0, 0, 0, 0);
            ActiveBorder = Colours.ToColor(180, 180, 180, 255);
            ActiveCaption = Colours.ToColor(209, 180, 153, 255);
            ActiveCaptionText = Colours.ToColor(0, 0, 0, 255);
            AppWorkspace = Colours.ToColor(171, 171, 171, 255);
            Control = Colours.ToColor(240, 240, 240, 255);
            ControlDark = Colours.ToColor(160, 160, 160, 255);
            ControlDarkDark = Colours.ToColor(105, 105, 105, 255);
            ControlLight = Colours.ToColor(227, 227, 227, 255);
            ControlLightLight = Colours.ToColor(255, 255, 255, 255);
            ControlText = Colours.ToColor(0, 0, 0, 255);
            Desktop = Colours.ToColor(0, 0, 0, 255);
            GrayText = Colours.ToColor(109, 109, 109, 255);
            Highlight = Colours.ToColor(255, 153, 51, 255);
            HighlightText = Colours.ToColor(255, 255, 255, 255);
            HotTrack = Colours.ToColor(204, 102, 0, 255);
            InactiveBorder = Colours.ToColor(252, 247, 244, 255);
            InactiveCaption = Colours.ToColor(219, 205, 191, 255);
            InactiveCaptionText = Colours.ToColor(0, 0, 0, 255);
            Info = Colours.ToColor(225, 255, 255, 255);
            InfoText = Colours.ToColor(0, 0, 0, 255);
            Menu = Colours.ToColor(240, 240, 240, 255);
            MenuText = Colours.ToColor(0, 0, 0, 255);
            ScrollBar = Colours.ToColor(200, 200, 200, 255);
            Window = Colours.ToColor(255, 255, 255, 255);
            WindowFrame = Colours.ToColor(100, 100, 100, 255);
            WindowText = Colours.ToColor(0, 0, 0, 255);
            Transparent = Colours.ToColor(255, 255, 255, 0);
            AliceBlue = Colours.ToColor(255, 248, 240, 255);
            AntiqueWhite = Colours.ToColor(215, 235, 250, 255);
            Aqua = Colours.ToColor(255, 255, 0, 255);
            Aquamarine = Colours.ToColor(212, 255, 127, 255);
            Azure = Colours.ToColor(255, 255, 240, 255);
            Beige = Colours.ToColor(220, 245, 245, 255);
            Bisque = Colours.ToColor(196, 228, 255, 255);
            Black = Colours.ToColor(0, 0, 0, 255);
            BlanchedAlmond = Colours.ToColor(205, 235, 255, 255);
            Blue = Colours.ToColor(255, 0, 0, 255);
            BlueViolet = Colours.ToColor(226, 43, 138, 255);
            Brown = Colours.ToColor(42, 42, 165, 255);
            BurlyWood = Colours.ToColor(135, 184, 222, 255);
            CadetBlue = Colours.ToColor(160, 158, 95, 255);
            Chartreuse = Colours.ToColor(0, 255, 127, 255);
            Chocolate = Colours.ToColor(30, 105, 210, 255);
            Coral = Colours.ToColor(80, 127, 255, 255);
            CornflowerBlue = Colours.ToColor(237, 149, 100, 255);
            Cornsilk = Colours.ToColor(220, 248, 255, 255);
            Crimson = Colours.ToColor(60, 20, 220, 255);
            Cyan = Colours.ToColor(255, 255, 0, 255);
            DarkBlue = Colours.ToColor(139, 0, 0, 255);
            DarkCyan = Colours.ToColor(139, 139, 0, 255);
            DarkGoldenrod = Colours.ToColor(11, 134, 184, 255);
            DarkGray = Colours.ToColor(169, 169, 169, 255);
            DarkGreen = Colours.ToColor(0, 100, 0, 255);
            DarkKhaki = Colours.ToColor(107, 183, 189, 255);
            DarkMagenta = Colours.ToColor(139, 0, 139, 255);
            DarkOliveGreen = Colours.ToColor(47, 107, 85, 255);
            DarkOrange = Colours.ToColor(0, 140, 255, 255);
            DarkOrchid = Colours.ToColor(204, 50, 153, 255);
            DarkRed = Colours.ToColor(0, 0, 139, 255);
            DarkSalmon = Colours.ToColor(122, 150, 233, 255);
            DarkSeaGreen = Colours.ToColor(139, 188, 143, 255);
            DarkSlateBlue = Colours.ToColor(139, 61, 72, 255);
            DarkSlateGray = Colours.ToColor(79, 79, 47, 255);
            DarkTurquoise = Colours.ToColor(209, 206, 0, 255);
            DarkViolet = Colours.ToColor(211, 0, 148, 255);
            DeepPink = Colours.ToColor(147, 20, 255, 255);
            DeepSkyBlue = Colours.ToColor(255, 191, 0, 255);
            DimGray = Colours.ToColor(105, 105, 105, 255);
            DodgerBlue = Colours.ToColor(255, 144, 30, 255);
            Firebrick = Colours.ToColor(34, 34, 178, 255);
            FloralWhite = Colours.ToColor(240, 250, 255, 255);
            ForestGreen = Colours.ToColor(34, 139, 34, 255);
            Fuchsia = Colours.ToColor(255, 0, 255, 255);
            Gainsboro = Colours.ToColor(220, 220, 220, 255);
            GhostWhite = Colours.ToColor(255, 248, 248, 255);
            Gold = Colours.ToColor(0, 215, 255, 255);
            Goldenrod = Colours.ToColor(32, 165, 218, 255);
            Gray = Colours.ToColor(128, 128, 128, 255);
            Green = Colours.ToColor(0, 128, 0, 255);
            GreenYellow = Colours.ToColor(47, 255, 173, 255);
            Honeydew = Colours.ToColor(240, 255, 240, 255);
            HotPink = Colours.ToColor(180, 105, 255, 255);
            IndianRed = Colours.ToColor(92, 92, 205, 255);
            Indigo = Colours.ToColor(130, 0, 75, 255);
            Ivory = Colours.ToColor(240, 255, 255, 255);
            Khaki = Colours.ToColor(140, 230, 240, 255);
            Lavender = Colours.ToColor(250, 230, 230, 255);
            LavenderBlush = Colours.ToColor(245, 240, 255, 255);
            LawnGreen = Colours.ToColor(0, 252, 124, 255);
            LemonChiffon = Colours.ToColor(205, 250, 255, 255);
            LightBlue = Colours.ToColor(230, 216, 173, 255);
            LightCoral = Colours.ToColor(128, 128, 240, 255);
            LightCyan = Colours.ToColor(255, 255, 224, 255);
            LightGoldenrodYellow = Colours.ToColor(210, 250, 250, 255);
            LightGray = Colours.ToColor(211, 211, 211, 255);
            LightGreen = Colours.ToColor(144, 238, 144, 255);
            LightPink = Colours.ToColor(193, 182, 255, 255);
            LightSalmon = Colours.ToColor(122, 160, 255, 255);
            LightSeaGreen = Colours.ToColor(170, 178, 32, 255);
            LightSkyBlue = Colours.ToColor(250, 206, 135, 255);
            LightSlateGray = Colours.ToColor(153, 136, 119, 255);
            LightSteelBlue = Colours.ToColor(222, 196, 176, 255);
            LightYellow = Colours.ToColor(224, 255, 255, 255);
            Lime = Colours.ToColor(0, 255, 0, 255);
            LimeGreen = Colours.ToColor(50, 205, 50, 255);
            Linen = Colours.ToColor(230, 240, 250, 255);
            Magenta = Colours.ToColor(255, 0, 255, 255);
            Maroon = Colours.ToColor(0, 0, 128, 255);
            MediumAquamarine = Colours.ToColor(170, 205, 102, 255);
            MediumBlue = Colours.ToColor(205, 0, 0, 255);
            MediumOrchid = Colours.ToColor(211, 85, 186, 255);
            MediumPurple = Colours.ToColor(219, 112, 147, 255);
            MediumSeaGreen = Colours.ToColor(113, 179, 60, 255);
            MediumSlateBlue = Colours.ToColor(238, 104, 123, 255);
            MediumSpringGreen = Colours.ToColor(154, 250, 0, 255);
            MediumTurquoise = Colours.ToColor(204, 209, 72, 255);
            MediumVioletRed = Colours.ToColor(133, 21, 199, 255);
            MidnightBlue = Colours.ToColor(112, 25, 25, 255);
            MintCream = Colours.ToColor(250, 255, 245, 255);
            MistyRose = Colours.ToColor(225, 228, 255, 255);
            Moccasin = Colours.ToColor(181, 228, 255, 255);
            NavajoWhite = Colours.ToColor(173, 222, 255, 255);
            Navy = Colours.ToColor(128, 0, 0, 255);
            OldLace = Colours.ToColor(230, 245, 253, 255);
            Olive = Colours.ToColor(0, 128, 128, 255);
            OliveDrab = Colours.ToColor(35, 142, 107, 255);
            Orange = Colours.ToColor(0, 165, 255, 255);
            OrangeRed = Colours.ToColor(0, 69, 255, 255);
            Orchid = Colours.ToColor(214, 112, 218, 255);
            PaleGoldenrod = Colours.ToColor(170, 232, 238, 255);
            PaleGreen = Colours.ToColor(152, 251, 152, 255);
            PaleTurquoise = Colours.ToColor(238, 238, 175, 255);
            PaleVioletRed = Colours.ToColor(147, 112, 219, 255);
            PapayaWhip = Colours.ToColor(213, 239, 255, 255);
            PeachPuff = Colours.ToColor(185, 218, 255, 255);
            Peru = Colours.ToColor(63, 133, 205, 255);
            Pink = Colours.ToColor(203, 192, 255, 255);
            Plum = Colours.ToColor(221, 160, 221, 255);
            PowderBlue = Colours.ToColor(230, 224, 176, 255);
            Purple = Colours.ToColor(128, 0, 128, 255);
            Red = Colours.ToColor(0, 0, 255, 255);
            RosyBrown = Colours.ToColor(143, 143, 188, 255);
            RoyalBlue = Colours.ToColor(225, 105, 65, 255);
            SaddleBrown = Colours.ToColor(19, 69, 139, 255);
            Salmon = Colours.ToColor(114, 128, 250, 255);
            SandyBrown = Colours.ToColor(96, 164, 244, 255);
            SeaGreen = Colours.ToColor(87, 139, 46, 255);
            SeaShell = Colours.ToColor(238, 245, 255, 255);
            Sienna = Colours.ToColor(45, 82, 160, 255);
            Silver = Colours.ToColor(192, 192, 192, 255);
            SkyBlue = Colours.ToColor(235, 206, 135, 255);
            SlateBlue = Colours.ToColor(205, 90, 106, 255);
            SlateGray = Colours.ToColor(144, 128, 112, 255);
            Snow = Colours.ToColor(250, 250, 255, 255);
            SpringGreen = Colours.ToColor(127, 255, 0, 255);
            SteelBlue = Colours.ToColor(180, 130, 70, 255);
            Tan = Colours.ToColor(140, 180, 210, 255);
            Teal = Colours.ToColor(128, 128, 0, 255);
            Thistle = Colours.ToColor(216, 191, 216, 255);
            Tomato = Colours.ToColor(71, 99, 255, 255);
            Turquoise = Colours.ToColor(208, 224, 64, 255);
            Violet = Colours.ToColor(238, 130, 238, 255);
            Wheat = Colours.ToColor(179, 222, 245, 255);
            White = Colours.ToColor(255, 255, 255, 255);
            WhiteSmoke = Colours.ToColor(245, 245, 245, 255);
            Yellow = Colours.ToColor(0, 255, 255, 255);
            YellowGreen = Colours.ToColor(50, 205, 154, 255);
            ButtonFace = Colours.ToColor(240, 240, 240, 255);
            ButtonHighlight = Colours.ToColor(255, 255, 255, 255);
            ButtonShadow = Colours.ToColor(160, 160, 160, 255);
            GradientActiveCaption = Colours.ToColor(234, 209, 185, 255);
            GradientInactiveCaption = Colours.ToColor(242, 228, 215, 255);
            MenuBar = Colours.ToColor(240, 240, 240, 255);
            MenuHighlight = Colours.ToColor(255, 153, 51, 255);
        }
    }
}
