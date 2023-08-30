namespace Source.Turn_State_Machine
{
    /// <summary>
    /// Base turn state class.
    /// </summary>
    public abstract class TurnState
    {
        public TurnState NextState { get; protected set; }

        protected TurnState(){}
        protected TurnState(TurnState nextState)
        {
            NextState = nextState;
        }
        
        public virtual void Enter(){}
        public virtual void Exit(){}
        public virtual void LogicUpdate(){}
        public virtual void PhysicsUpdate(){}
    }
}
