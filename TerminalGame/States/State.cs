using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TerminalGame.States
{
    public class State : IState
    {
        #region fields
        public delegate void StateChangeEventHandler(StateChangeEventArgs e);
        public event StateChangeEventHandler StateChange;
        protected StateChangeEventArgs _change;
        protected Dictionary<string, State> _availableStates;
        #endregion

        #region properties
        #endregion

        protected static State _instance;

        public static State GetInstance()
        {
            if (_instance == null)
                _instance = new State();
            return _instance;
        }

        protected State()
        {
            _change = new StateChangeEventArgs();
            StateChange += OnStateChange;
            _availableStates = new Dictionary<string, State>();
        }

        public void AddState(string name, State state)
        {
            _availableStates.Add(name, state);
        }

        public State GetNextState(string state)
        {
            if (_availableStates.TryGetValue(state, out State retval))
                return retval;
            return this;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {

        }

        protected void OnStateChange(StateChangeEventArgs e)
        {

        }
    }

    public class StateChangeEventArgs : EventArgs
    {
        // TODO: Specify StateChangeEventArgs content
    }
}
