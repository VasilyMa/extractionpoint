using System.Collections.Generic;

namespace Stat.UnitStat
{
    public class Health : UnitStat
    {
        public float Current => _value;
        public Health(float value)
        {
            _modifiers = new List<float>();
            _bonuses = new List<float>();
            _baseValue = value;
            _value = value;
            _maxValue = value;
            _resolveMax = 2f;
            _resolveMin = 1f;
        }
        protected override float GetResolveValue()
        {
            return _resolveMin + (_resolveMax - _resolveMin) * (_maxValue / 100);
        }
    }
}
