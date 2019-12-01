using System;

namespace VDM.Data.Model
{
    /// <summary>
    /// Represents a process that can be started as part of a VDP. (State tracking could be added in the future.)
    /// </summary>
    public class Process
    {
        /// <summary>
        /// Id of the process. Unique in DB.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the process.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Fully qualified path of the process' executable.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// CLI arguments that should be passed to the given process upon starting it.
        /// </summary>
        public string CommandLineArgs { get; set; }

        public Guid PresetId { get; set; }
        public VirtualDesktopPreset Preset { get; set; }
    }
}
