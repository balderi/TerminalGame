namespace TerminalGame.Computers
{
    public interface IComputer
    {
        bool Connect();
        void Disconnect(bool forced);
        void Tick();
    }
}
