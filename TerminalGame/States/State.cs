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

        /// <summary>
        /// Initialize the state.
        /// </summary>
        /// <param name="graphics"><c>GraphicsDeviceManager</c> for initializing the <c>Screen</c>.</param>
        /// <param name="screen">The scrren to display in this state.</param>
        /// <param name="game">The current game.</param>
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
        
        /// <summary>
        /// Add a state to the list of available future states.
        /// </summary>
        /// <param name="name">Name of the state, fore reference.</param>
        /// <param name="state">The actual state.</param>
        public void AddState(string name, State state)
        {
            _availableStates.Add(name, state);
        }

        /// <summary>
        /// Will try to fetch the named state from the list of available future states.
        /// </summary>
        /// <param name="state">Name of the state to get.</param>
        /// <param name="outState">State return.</param>
        /// <returns>Returns <c>true</c> if the state exists, <c>false</c> otherwise.</returns>
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
        protected bool IsStateChangeRegistered(Delegate prospectiveHandler)
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
