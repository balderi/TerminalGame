using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TerminalGame.States.Screens;

namespace TerminalGame.States
{
    public partial class State : IState
    {
        #region fields
        public delegate void StateChangeEventHandler(StateChangeEventArgs e);
        public event StateChangeEventHandler StateChange;
        protected StateChangeEventArgs _change;
        protected Dictionary<string, State> _availableStates;
        protected string _name;
        protected Game _game;
        #endregion

        #region properties
        public Screen Screen { get; protected set; }
        #endregion

        public virtual void Initialize(GraphicsDeviceManager graphics, Screen screen, Game game)
        {
            _change = new StateChangeEventArgs();

            // Prevent the eventhandler from subscribing to the same event multiple times
            if(!IsStateChangeRegistered(StateChange))
                StateChange += OnStateChange;

            _availableStates = new Dictionary<string, State>();
            Screen = screen;
            Screen.Initialize(graphics);
            _game = game;
            StateChange?.Invoke(new StateChangeEventArgs());
        }

        public void AddState(string name, State state)
        {
            _availableStates.Add(name, state);
        }

        public bool TryGetNextState(string state, out State outState)
        {
            if (_availableStates.TryGetValue(state, out State retval))
            {
                StateChange?.Invoke(new StateChangeEventArgs());
                outState = retval;
                return true;
            }
            outState = this;
            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            Screen.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            Screen.Draw(gameTime);
        }

        protected virtual void OnStateChange(StateChangeEventArgs e) { Console.WriteLine("StateChange: " + _name); }

        // https://stackoverflow.com/questions/136975/has-an-event-handler-already-been-added/7065771 <3
        public bool IsStateChangeRegistered(Delegate prospectiveHandler)
        {
            if (StateChange != null)
            {
                foreach (Delegate existingHandler in StateChange.GetInvocationList())
                {
                    if (existingHandler == prospectiveHandler)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class StateChangeEventArgs : EventArgs
    {
        // TODO: Specify StateChangeEventArgs content
    }
}
