using UnityEngine;

namespace HordeEvolution
{
    public sealed class SkillButton : MonoBehaviour
    {
        [SerializeField] AutoWeapon weapon;
        public void Press()=>weapon.ActivateSkill();
    }
}
