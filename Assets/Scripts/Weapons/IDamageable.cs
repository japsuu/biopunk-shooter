namespace Weapons
{
    public interface IDamageable
    {
        public float KillRewardXp { get; }
        
        /// <summary>
        /// Called when the object takes damage.
        /// </summary>
        /// <param name="amount">The amount of damage to take.</param>
        /// <param name="notifyOfKill">The object that caused the damage.</param>
        public void Damage(float amount, IDamageCauser notifyOfKill);
    }
}