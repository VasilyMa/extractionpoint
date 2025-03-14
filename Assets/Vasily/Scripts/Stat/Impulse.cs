
using System.Collections.Generic;

namespace Stat.WeaponStat
{
    public class Impulse : WeaponStat
    {
        public override string Name => "Impulse";
        public Impulse(float value, float resolveMin, float resolveMax)
        {
            _modifiers = new List<float>();
            _baseValue = value;
            _value = value;
            _maxValue = value;
            _resolveMin = resolveMin;
            _resolveMax = resolveMax;
        }
        protected override float GetResolveValue()
        {
            return _resolveMin + (_resolveMax - _resolveMin) * (_maxValue / 100);
        }
    }
}