using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TerminalGame.Screens;

namespace TerminalGame.States
{
    public partial class State : IState
    {
        #region fields
        public delegate void StateChangeEventHandler(object sender, StateChangeEventArgs e);
        public event StateChangeEventHandler StateChange;
        protected StateChangeEventArgs _change;
        protected Dictionary<string, State> _availableStates;
        protected string _name;
        protected Game _game;
        protected Dictionary<string, Screen> _screens;
        protected GraphicsDeviceManager _graphics;
        protected bool _isInitialized = false;
        #endregion

        #region properties
        public Screen CurrentScreen { get; protected set; }
        #endregion

        /// <summary>
        /// Initialize the state.
        /// </summary>
        /// <param name="graphics"><c>GraphicsDeviceManager</c> for initializing the <c>Screen</c>.</param>
        /// <param name="initialScreen">The scrren to display in this state.</param>
        /// <param name="game">The current game.</param>
        public virtual void Initialize(GraphicsDeviceManager graphics, Screen initialScreen, Game game)
        {
            if (_isInitialized)
                throw new Exception("State is already initialized.");

            _change = new StateChangeEventArgs();

            _graphics = graphics;

            // Prevent the eventhandler from subscribing to the same event multiple times
            if(!IsStateChangeRegistered(StateChange))
                StateChange += OnStateChange;

            _availableStates = new Dictionary<string, State>();
            _screens = new Dictionary<string, Screen>
            {
                { "initial", initialScreen }
            };

            if (!TryChangeScreen("initial"))
                throw new Exception("Something went wrong during initialization of game screen.");
            //CurrentScreen = initialScreen;
            //CurrentScreen.Initialize(_graphics);
            _game = game;
            StateChange?.Invoke(this, new StateChangeEventArgs());
        }
        
        /// <summary>
        /// Add a state to the list of available future states.
        /// </summary>
        /// <param name="name">Name of the state, for reference.</param>
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
                StateChange?.Invoke(this, new StateChangeEventArgs());
                outState = retval;
                return true;
            }
            outState = this;
            return false;
        }

        /// <summary>
        /// Add a screen to the list of available future screens.
        /// </summary>
        /// <param name="name">Name of the screen, for reference.</param>
        /// <param name="screen">The actual screen.</param>
        public void AddScreen(string name, Screen screen)
        {
            _screens.Add(name, screen);
        }

        /// <summary>
        /// Change the active screen.
        /// </summary>
        /// <param name="screen">Name of the screen to change to.</param>
        /// <returns>Returns <c>true</c> if the screen exists, <c>false</c> otherwise.</returns>
        public bool TryChangeScreen(string screen)
        {
            if(_screens.TryGetValue(screen, out Screen retval))
            {
                CurrentScreen = retval;
                CurrentScreen.Initialize(_graphics);
                return true;
            }
            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            CurrentScreen.Draw(gameTime);
        }

        protected virtual void OnStateChange(object sender, StateChangeEventArgs e) { Console.WriteLine("StateChange: " + _name); }

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
