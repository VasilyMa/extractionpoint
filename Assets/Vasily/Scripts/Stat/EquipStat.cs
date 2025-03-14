using System.Collections.Generic;

namespace Stat.EquipStat
{
    public class EquipStat : Stat
    {
        protected List<float> _bonuses;
        public override void Init()
        {
            _bonuses = new List<float>();
            _modifiers = new List<float>();
            _value = _baseValue;
            _maxValue = _baseValue;
        }

        public override float Add(float value)
        {
            return _value += value;
        }

        public override float AddModifier(float modifier)
        {
            _modifiers.Add(modifier);

            _maxValue = GetMaxValue();

            float value = _value;

            return _value += value * modifier;
        }

        public override float Sub(float value)
        {
            return _value -= value;
        }

        public override float SubModifier(float modifier)
        {
            _modifiers.Remove(modifier);

            _maxValue = GetMaxValue();

            float value = _value;

            return _value += value * modifier;
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
        public virtual void Load(float value)
        {
            _baseValue = value;
            _value = value;
            _maxValue = value;
        }
    }
}