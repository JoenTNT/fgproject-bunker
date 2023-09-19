namespace JT.FGP
{
    /// <summary>
    /// Type of light in game.
    /// </summary>
    [System.Flags]
    public enum InGameLightingType : short
    {
        Unknown = 0,
        Global = 1,
        Primary = 2,
        Secondary = 4,
        Tertiary = 8,
        Quartenary = 16,
        Quinary = 32,
        Senary = 64,
        Septenary = 128,
        Octonary = 256,
        Nonary = 512,
        Denary = 1024,
    }
}
