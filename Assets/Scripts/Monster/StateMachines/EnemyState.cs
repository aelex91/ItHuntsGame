namespace Assets.Scripts.StateMachines
{
    public abstract class EnemyState
    {
        protected EnemyAI enemy;
        protected EnemyState(EnemyAI enemy) => this.enemy = enemy;

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}