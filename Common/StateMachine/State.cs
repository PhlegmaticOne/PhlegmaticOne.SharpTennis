using System;

namespace PhlegmaticOne.SharpTennis.Game.Common.StateMachine
{
    public readonly struct State : IEquatable<State>
    {
        public static State None => new State("[None]");
        public State(string name) => Name = name;
        public string Name { get; }
        public override string ToString() => Name;
        public bool Equals(State other) => Name == other.Name;
        public override bool Equals(object obj) => obj is State other && Equals(other);
        public override int GetHashCode() => (Name != null ? Name.GetHashCode() : 0);
    }
}