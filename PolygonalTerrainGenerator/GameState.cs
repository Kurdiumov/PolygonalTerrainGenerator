namespace ProceduralTerrainGenerator
{
    public enum State { GodMode, FirstPerson, Strategic, Paused, Menu }
    public class GameState
    {
        public GameState(State state = State.GodMode)
        {
            _state = state;
        }

        private State _state;
        private State _previousState;

        public State GetCurrentGameState()
        {
            return _state;
        }

        public State GetPreviousGameState()
        {
            return _previousState;
        }

        public void SetNewGameState(State state)
        {
            if (_state != state)
            {
                _previousState = _state;
                _state = state;
            }
        }
    }
}
