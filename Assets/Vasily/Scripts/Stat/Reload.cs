using System.Collections.Generic;

using UnityEngine;

namespace Stat.WeaponStat
{
    public class Reload : WeaponStat
    {
        public override string Name => "Reload";
        public Reload(float value, float resolveMin, float resolveMax)
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
            return Mathf.Clamp(_resolveMax - (_resolveMax - _resolveMin) * (_maxValue / 100), 0.25f, 3f);
        }

    }
}