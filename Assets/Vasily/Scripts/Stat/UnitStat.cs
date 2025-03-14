using System;
using System.Collections.Generic;

namespace Stat.UnitStat
{
    public abstract class UnitStat : Stat
    {
        public Action<float, float> OnValueUpdate;
        protected List<float> _bonuses;
        protected float _resolveMin;
        protected float _resolveMax;
        protected float _resolveValue;
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
            _value += value;

            OnValueUpdate?.Invoke(_value, _maxValue);

            return _value;
        }

        public override float Sub(float value)
        {
            _value -= value;

            OnValueUpdate?.Invoke(_value, _maxValue);

            return _value;
        }

        public virtual float AddBonus(float bonus)
        {
            _bonuses.Add(bonus);

            _value += bonus;

            _maxValue = GetMaxValue();
            OnValueUpdate?.Invoke(_value, _maxValue);

            return _value;
        }

        public virtual float RemoveBonus(float bonus)
        {
            if (_bonuses.Contains(bonus))
            {
                _bonuses.Remove(bonus);

                _value -= bonus;

                _maxValue = GetMaxValue();
                OnValueUpdate?.Invoke(_value, _maxValue);
            }

            return _value;
        }

        public override float AddModifier(float modifier)
        {
            _modifiers.Add(modifier);

            _maxValue = GetMaxValue();

            float val = _value; 
            
            _value += val * modifier;

            OnValueUpdate?.Invoke(_value, _maxValue);

            return _value;
        }

        public override float SubModifier(float modifier)
        {
            if (_modifiers.Contains(modifier))
            {
                _modifiers.Remove(modifier);

                _maxValue = GetMaxValue();

                float val = _value;

                _value -= val * modifier;
            }

            OnValueUpdate?.Invoke(_value, _maxValue);

            return _value;
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

        public virtual float SetActualyCurrentValue(float value)
        {
            return _value = value;
        }
    }
}