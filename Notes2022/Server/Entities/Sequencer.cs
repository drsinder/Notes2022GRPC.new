// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : sinde
// Created          : 04-19-2022
//
// Last Modified By : sinde
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="Sequencer.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Sequencer.cs
    **
    ** Description:
    **      Used for tracking recent notes
    **
    ** This program is free software: you can redistribute it and/or modify
    ** it under the terms of the GNU General Public License version 3 as
    ** published by the Free Software Foundation.   
    **
    ** This program is distributed in the hope that it will be useful,
    ** but WITHOUT ANY WARRANTY; without even the implied warranty of
    ** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    ** GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see<http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/



using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Google.Protobuf.WellKnownTypes;
using Notes2022.Proto;

namespace Notes2022.Server.Entities
{
    /// <summary>
    /// This class defines a table in the database.
    /// Object of this class may be associated with a user
    /// and file to be used to find notes written since the
    /// "Recent" function was last invoked.
    /// </summary>
    [DataContract]
    public class Sequencer
    {
        // ID of the user who owns the item
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        [Column(Order = 0)]
        [StringLength(450)]
        [DataMember(Order = 1)]
        public string? UserId { get; set; }

        // ID of target notfile
        /// <summary>
        /// Gets or sets the note file identifier.
        /// </summary>
        /// <value>The note file identifier.</value>
        [Required]
        [Column(Order = 1)]
        [DataMember(Order = 2)]
        public int NoteFileId { get; set; }

        /// <summary>
        /// Gets or sets the ordinal.
        /// </summary>
        /// <value>The ordinal.</value>
        [Required]
        [Display(Name = "Position in List")]
        [DataMember(Order = 3)]
        public int Ordinal { get; set; }

        // Time we last completed a run with this
        /// <summary>
        /// Gets or sets the last time.
        /// </summary>
        /// <value>The last time.</value>
        [Display(Name = "Last Time")]
        [DataMember(Order = 4)]
        public DateTime LastTime { get; set; }

        // Time a run in this file started - will get copied to LastTime when complete
        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>The start time.</value>
        [DataMember(Order = 5)]
        public DateTime StartTime { get; set; }

        // Is this item active now?  Are we sequencing this file
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Sequencer"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        [DataMember(Order = 6)]
        public bool Active { get; set; }

        //[ForeignKey("NoteFileId")]
        //public NoteFile? NoteFile { get; set; }

        //
        // Conversions between Db Entity space and gRPC space.
        //
        /// <summary>
        /// Gets the sequencer.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>Sequencer.</returns>
        public static Sequencer GetSequencer(GSequencer other)
        {
            Sequencer s = new Sequencer();
            s.UserId = other.UserId;
            s.NoteFileId = other.NoteFileId;
            s.Ordinal = other.Ordinal;
            s.LastTime = other.LastTime.ToDateTime();
            s.StartTime = other.StartTime.ToDateTime();
            s.Active = other.Active;
            return s;
        }

        /// <summary>
        /// Gets the g sequencer.
        /// </summary>
        /// <returns>GSequencer.</returns>
        public GSequencer GetGSequencer()
        {
            GSequencer s = new GSequencer();
            s.UserId = this.UserId;
            s.NoteFileId  = this.NoteFileId;
            s.Ordinal = this.Ordinal;
            s.LastTime = Timestamp.FromDateTime(Globals.UTimeBlazor(this.LastTime).ToUniversalTime());
            s.StartTime = Timestamp.FromDateTime(Globals.UTimeBlazor(this.StartTime).ToUniversalTime());
            s.Active=this.Active;
            return s;
        }

        /// <summary>
        /// Gets the sequencer list.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>List&lt;Sequencer&gt;.</returns>
        public static List<Sequencer> GetSequencerList(GSequencerList other)
        {
            List<Sequencer> list = new List<Sequencer>();
            foreach (GSequencer t in other.List)
            {
                list.Add(GetSequencer(t));
            }
            return list;
        }

        /// <summary>
        /// Gets the g sequencer list.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>GSequencerList.</returns>
        public static GSequencerList GetGSequencerList(List<Sequencer> other)
        {
            GSequencerList list = new GSequencerList();
            foreach (Sequencer t in other)
            {
                list.List.Add(t.GetGSequencer());
            }
            return list;
        }

    }
}
