using System.Collections.Generic;

using UnityEngine;

namespace Stat.WeaponStat
{
    public class MissileSpeed : WeaponStat
    {
        public override string Name => "Missile speed";
        public MissileSpeed(float value, float resolveMin, float resolveMax)
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
            return Mathf.Clamp(_resolveMin + (_resolveMax - _resolveMin) * (_maxValue / 100), 5, 25);
        }
    }
}