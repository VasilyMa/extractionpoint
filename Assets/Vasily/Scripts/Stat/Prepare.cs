using System.Collections.Generic;

namespace Stat.WeaponStat
{
    public class Prepare : WeaponStat
    {
        public override string Name => "Prepare";
        public Prepare(float value, float resolveMin, float resolveMax)
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
            return _resolveMax - (_resolveMax - _resolveMin) * (_maxValue / 100);
        }
    }
}