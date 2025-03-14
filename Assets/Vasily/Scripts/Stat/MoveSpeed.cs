using System.Collections.Generic;

namespace Stat.UnitStat
{
    public class MoveSpeed : UnitStat
    {
        public MoveSpeed(float value, float min, float max)
        {
            _modifiers = new List<float>();
            _bonuses = new List<float>();
            _baseValue = value;
            _value = value;
            _maxValue = value;
            _resolveMax = max;
            _resolveMin = min;
        }

        protected override float GetResolveValue()
        {
            return _resolveMin + (_resolveMax - _resolveMin) * (_maxValue / 100);
        }
        protected override float GetMaxValue()
        {
            return base.GetMaxValue() * 0.1f;
        }
    }
}