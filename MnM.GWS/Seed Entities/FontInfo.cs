namespace MnM.GWS
{
    public struct FontInfo : IFontInfo
    {
        public FontStyle Style { get; set; }
        public float UnitsPerEm { get; set; }
        public bool IntegerPpems { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public float CellAscent { get; set; }
        public float CellDescent { get; set; }
        public float LineHeight { get; set; }
        public float XHeight { get; set; }
        public float UnderlineSize { get; set; }
        public float UnderlinePosition { get; set; }
        public float StrikeoutSize { get; set; }
        public float StrikeoutPosition { get; set; }
    }
}
