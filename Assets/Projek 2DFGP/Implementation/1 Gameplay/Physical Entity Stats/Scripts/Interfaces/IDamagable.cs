namespace JT.FGP
{
    /// <summary>
    /// Things if can take damage.
    /// </summary>
    public interface IDamagable
    {
        /// <summary>
        /// Flag if this entity is now damageable.
        /// </summary>
        bool IsDamagable { get; set; }
    }
}
