namespace GUI.Interfaces
{
    /// <summary>
    /// Represents a selectable GUI element for list containers.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Is the item selected in GUI?
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
