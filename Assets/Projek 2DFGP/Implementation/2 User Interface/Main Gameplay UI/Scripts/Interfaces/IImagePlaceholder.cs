namespace JT.FGP
{
    /// <summary>
    /// Used to place image data.
    /// </summary>
    public interface IImagePlaceholder<T> where T : class
    {
        /// <summary>
        /// Set current image on placeholder.
        /// </summary>
        /// <param name="image">New image data (to unset use null value)</param>
        void SetImage(T image);
    }
}