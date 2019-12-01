using System;
using System.Collections.Generic;

namespace VDM.Data.Model
{
    /// <summary>
    /// Represents a preset of a virtual desktop, that can be managed by the user.
    /// </summary>
    public class VirtualDesktopPreset
    {
        /// <summary>
        /// Id of the VDP. Unique in DB.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User given name of the VDP.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Icon of the preset. Might be null. Not necessarily unique.
        /// </summary>
        public byte[] Icon { get; set; }

        /// <summary>
        /// True if the preset should fire on system startup, false otherwise.
        /// </summary>
        public bool IsOpenedOnSystemStart { get; set; }
        
        /// <summary>
        /// List of the processes that should open when this VPD is fired up.
        /// </summary>
        public ICollection<Process> AttachedProcesses { get; set; }
    }
}
