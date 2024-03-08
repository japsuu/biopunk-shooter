namespace Weapons
{
    public interface IDamageCauser
    {
        /// <summary>
        /// Called when this object kills another object.
        /// </summary>
        /// <param name="killed">The object that was killed.</param>
        public void NotifyOfKill(IDamageable killed);
    }
}