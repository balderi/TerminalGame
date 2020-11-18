using TerminalGame.Computers;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents.RemoteViewData.Generators
{
    public static class RemoteViewDataGenerator
    {
        public static DefaultView GetDefaultViewData(Computer computer)
        {
            return new DefaultView(computer);
        }
    }
}
