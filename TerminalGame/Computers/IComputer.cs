namespace TerminalGame.Computers
{
    public interface IComputer
    {
        bool Connect();
        void Disconnect();
        void Tick();
    }
}
