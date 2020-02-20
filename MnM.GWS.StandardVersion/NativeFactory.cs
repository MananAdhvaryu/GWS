namespace MnM.GWS.StandardVersion
{
    public partial class NativeFactory : GwsFactory
    {
        public static readonly IFactory Instance = new NativeFactory();
        protected NativeFactory() { }

        public override IElementCollection newElementCollection(IParent window) =>
            new ElementCollection(window);

        public override IRenderer newRenderer() =>
            new Renderer();
    }
}
