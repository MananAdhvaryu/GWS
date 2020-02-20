
using static MnM.GWS.Implementation;

namespace MnM.GWS
{
    public static  class FillStyles
    {
        internal static void Initialize()
        {
            if (Factory == null)
                return;
            Default = Factory.newFillStyle(Gradient.Horizontal, Colours.ToColor(255, 255, 255), Colours.ToColor(224, 224, 224));
            Blue = Factory.newFillStyle(Gradient.BackwardDiagonal, Colour.DodgerBlue, Colour.Black);
            Selection = Factory.newFillStyle(Gradient.ForwardDiagonal, Colour.MidnightBlue, Colour.Blue);
            LightSelection = Factory.newFillStyle(Gradient.Horizontal,  Colour.Khaki, Colour.Orange);
            OrangeSelection = Factory.newFillStyle(Gradient.Horizontal, Colour.Gold, Colour.Orange);
            Black = Factory.newFillStyle(Gradient.Horizontal, Colour.Black, Colour.DimGray);
            WindowFrame = Factory.newFillStyle(Gradient.Horizontal, Colour.WindowFrame, Colour.Silver);
            Silver = Factory.newFillStyle(Gradient.Vertical,  Colour.Silver, Colour.White);
            DarkSilver = Factory.newFillStyle(Gradient.Vertical,  Colour.Silver, Colours.ToColor(105, 105, 105));
            ListDarkAlternateStyle = Factory.newFillStyle(Gradient.BackwardDiagonal,  Colour.SeaGreen, Colour.Aqua);

            ListAlternateStyle = Factory.newFillStyle(Gradient.Horizontal,  Colour.Azure, Colour.Silver);
            ListDarkAlternateStyle = Factory.newFillStyle(Gradient.Horizontal,  Colour.Black, Colour.DimGray);
            DropDownButton = Factory.newFillStyle(Gradient.Vertical, Colour.Silver, Colour.White);
            CalendarHighlight = Factory.newFillStyle(Gradient.Horizontal, Colour.Navy, Colour.Blue);
            WhiteWash = Factory.newFillStyle(Gradient.None, Colour.White);
            GridAlternateDefaultCell = Factory.newFillStyle(Gradient.Horizontal, Colour.Black, Colour.Gray);
            GridBackGround = Factory.newFillStyle(Gradient.ForwardDiagonal, Colour.Brown, Colour.Cyan);
            GridSelection = Factory.newFillStyle(Gradient.Vertical, Colour.CornflowerBlue, Colour.Aqua);
            GridColumnHeader = Factory.newFillStyle(Gradient.Vertical, Colour.Black, Colour.Gray);
            GridLastRowHeader = Factory.newFillStyle(Gradient.Vertical, Colour.CornflowerBlue, Colour.SkyBlue);
            Canvas = Factory.newFillStyle(Gradient.Vertical, Colour.CornflowerBlue, Colour.Aqua);
            Hover = Factory.newFillStyle(Gradient.Horizontal, Colour.GradientActiveCaption);
        }

        public static IFillStyle Default { get; private set; }
        public static IFillStyle Blue { get; private set; }
        public static IFillStyle Selection { get; private set; }
        public static IFillStyle LightSelection { get; private set; }
        public static IFillStyle OrangeSelection { get; private set; }
        public static IFillStyle Black { get; private set; }
        public static IFillStyle WindowFrame { get; private set; }
        public static IFillStyle Silver { get; private set; }
        public static IFillStyle DarkSilver { get; private set; }
        public static IFillStyle ListDarkAlternateStyle { get; private set; }
        public static IFillStyle ListAlternateStyle { get; private set; }
        public static IFillStyle ListDarkDefaultStyle { get; private set; }
        public static IFillStyle DropDownButton { get; private set; }
        public static IFillStyle CalendarHighlight { get; private set; }
        public static IFillStyle WhiteWash { get; private set; }
        public static IFillStyle GridAlternateDefaultCell { get; private set; }
        public static IFillStyle GridBackGround { get; private set; }
        public static IFillStyle GridSelection { get; private set; }
        public static IFillStyle GridColumnHeader { get; private set; }
        public static IFillStyle GridLastRowHeader { get; private set; }
        public static IFillStyle Canvas { get; private set; }
        public static IFillStyle Hover { get; private set; }
    }
}
