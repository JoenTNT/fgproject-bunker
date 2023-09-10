namespace JT.FGP
{
    /// <summary>
    /// Any obstacle or entity that has attack point.
    /// </summary>
    /// <typeparam name="T">Damage attack point data type</typeparam>
    public interface IAttackPoint<T> where T : notnull
    {
        /// <summary>
        /// Attack point damage.
        /// </summary>
        T AttackPointDamage { get; }
    }
}

