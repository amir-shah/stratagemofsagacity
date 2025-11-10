using System;
using System.Collections.Generic;
using System.Text;

namespace SoS
{
    class StateMachine
    {
        int state, prevState;

        public StateMachine(int startingState)
        {
            state = startingState;
            prevState = startingState;
        }
        public void changeState(int newState)
        {
            prevState = state;
            state = newState;
        }
        public int getState()
        {
            return state;
        }
        public int getPrevState()
        {
            return prevState;
        }
    }
}
