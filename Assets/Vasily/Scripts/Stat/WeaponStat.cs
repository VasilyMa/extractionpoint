using System.Collections.Generic;

namespace Stat.WeaponStat
{
    public abstract class WeaponStat : Stat
    {
        public abstract string Name { get; }
        protected float _resolveMin;
        protected float _resolveMax;
        protected float _resolveValue;
        protected List<float> _bonuses;
        public float ResloveValue { get => _resolveValue; }
        public override void Init()
        {
            _bonuses = new List<float>();
            _modifiers = new List<float>();
            _value = _baseValue;
            _maxValue = GetMaxValue();
            _resolveValue = GetResolveValue();
        }
        public override float Add(float value)
        {
            _bonuses.Add(value);
 
            _maxValue = GetMaxValue();

            _resolveValue = GetResolveValue();

            return _resolveValue;
        }
        public override float Sub(float value)
        {
            if (_bonuses.Contains(value))
            {
                _bonuses.Remove(value);

                _maxValue = GetMaxValue();

                _resolveValue = GetResolveValue();
            }

            return _resolveValue;
        }
        public override float AddModifier(float modifier)
        {
            _modifiers.Add(modifier);

            _maxValue = GetMaxValue();

            _resolveValue = GetResolveValue();

            return _resolveValue;
        }
        public override float SubModifier(float modifier)
        {
            if (_modifiers.Contains(modifier))
            {
                _modifiers.Remove(modifier);

                _maxValue = GetMaxValue();

                _resolveValue = GetResolveValue();

            }
            return _resolveValue;
        }

        protected override float GetMaxValue()
        {
            float maxValue = _baseValue;

            float baseValue = _baseValue;

            if (_bonuses.Count > 0)
            {
                _bonuses.ForEach(x => maxValue += x);
            }

            if (_modifiers.Count > 0)
            {
                _modifiers.ForEach(x => maxValue += baseValue * x);
            }

            return maxValue;
        }

        protected abstract float GetResolveValue();

        public virtual void Load(float value)
        {
            _baseValue = value;
            _value = value;
            _maxValue = value;
        }
    }
}