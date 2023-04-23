namespace PhlegmaticOne.SharpTennis.Game.Common.StateMachine
{
    public class States<T> where T : IStateCollection, new()
    {
        private static T _state;
        public static T Get
        {
            get
            {
                if (_state == null)
                {
                    _state = new T();
                }
                return _state;
            }
        }
    }
}