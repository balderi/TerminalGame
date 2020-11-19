using TerminalGame.Computers;

namespace TerminalGame.UI.Elements.Modules.ModuleComponents.RemoteViewData.Generators
{
    public static class RemoteViewDataGenerator
    {
        public static RemoteViewData GetDefaultViewData(Computer computer)
        {
            return new RemoteViewData(computer);
        }
    }
}
