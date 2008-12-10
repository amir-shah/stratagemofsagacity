using System;
using System.Collections.Generic;
using System.Text;

namespace SoS
{
    class StateMachine
    {
        int state;

        public StateMachine(int startingState)
        {
            state = startingState;
        }
        public void changeState(int newState)
        {
            state = newState;
        }
        public int getState()
        {
            return state;
        }
    }
}
