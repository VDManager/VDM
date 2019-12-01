namespace VDM.Data.Model.Enums
{
    /// <summary>
    /// Represents the state of a managed process on a VD.
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// Default state for .NET initialization purposes.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The process is running on the corresponding VD.
        /// </summary>
        Running = 1,

        /// <summary>
        /// The process doesn't seem to be running on the corresponding VD.
        /// </summary>
        Stopped = -1
    }
}
