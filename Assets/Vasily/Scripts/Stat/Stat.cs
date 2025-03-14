using System.Collections.Generic;
namespace Stat
{
    public abstract class Stat
    {
        protected List<float> _modifiers;
        protected float _value;
        protected float _baseValue;
        protected float _maxValue;
        public float BaseValue => _baseValue;
        public float MaxValue { get => _maxValue; }
        public abstract void Init();
        public abstract float Add(float value);
        public abstract float Sub(float value);
        public abstract float AddModifier(float modifier);
        public abstract float SubModifier(float modifier);
        protected abstract float GetMaxValue();
    }
}