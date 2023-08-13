namespace JT.InterfaceScripts
{
    /// <summary>
    /// Pagination interface for editor content.
    /// </summary>
    public interface IPagination
    {
        /// <summary>
        /// Page number from 0 to N.
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// Count how many pages in one book.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Limit the content in one page.
        /// </summary>
        int PageContentLimit { get; set; }
    }
}
