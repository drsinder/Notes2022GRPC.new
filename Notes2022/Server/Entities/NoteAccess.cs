// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : Dale Sinder
// Created          : 04-19-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 04-19-2022
// ***********************************************************************
// <copyright file="NoteAccess.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteAccess.cs
    **
    ** Description:
    **      Access token for a file
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
using Notes2022.Proto;

namespace Notes2022.Server.Entities
{
    /// <summary>
    /// This class defines a table in the database.
    /// Objects of this class are Access Tokens for a file.
    /// There are a minimum of two for each file:
    /// 1 for the file Owner.
    /// 1 for the unknown "Other" user - if an entry is not
    /// found for a user, this is the fallback.
    /// There COULD be one for each user.  But the Other entry is
    /// usually used for public file and so not too many other entries
    /// are needed.
    /// The fields should be self evident.
    /// </summary>
    [DataContract]
    public class NoteAccess
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        [Column(Order = 0)]
        [StringLength(450)]
        [DataMember(Order = 1)]
        public string? UserID { get; set; }

        /// <summary>
        /// Gets or sets the note file identifier.
        /// </summary>
        /// <value>The note file identifier.</value>
        [Required]
        [Column(Order = 1)]
        [DataMember(Order = 2)]
        public int NoteFileId { get; set; }

        /// <summary>
        /// Gets or sets the archive identifier.
        /// </summary>
        /// <value>The archive identifier.</value>
        [Required]
        [Column(Order = 2)]
        [DataMember(Order = 3)]
        public int ArchiveId { get; set; }

        // Control options

        /// <summary>
        /// Gets or sets a value indicating whether [read access].
        /// </summary>
        /// <value><c>true</c> if [read access]; otherwise, <c>false</c>.</value>
        [Required]
        [Display(Name = "Read")]
        [DataMember(Order = 4)]
        public bool ReadAccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NoteAccess"/> is respond.
        /// </summary>
        /// <value><c>true</c> if respond; otherwise, <c>false</c>.</value>
        [Required]
        [Display(Name = "Respond")]
        [DataMember(Order = 5)]
        public bool Respond { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NoteAccess"/> is write.
        /// </summary>
        /// <value><c>true</c> if write; otherwise, <c>false</c>.</value>
        [Required]
        [Display(Name = "Write")]
        [DataMember(Order = 6)]
        public bool Write { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [set tag].
        /// </summary>
        /// <value><c>true</c> if [set tag]; otherwise, <c>false</c>.</value>
        [Required]
        [Display(Name = "Set Tag")]
        [DataMember(Order = 7)]
        public bool SetTag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [delete edit].
        /// </summary>
        /// <value><c>true</c> if [delete edit]; otherwise, <c>false</c>.</value>
        [Required]
        [Display(Name = "Delete/Edit")]
        [DataMember(Order = 8)]
        public bool DeleteEdit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [view access].
        /// </summary>
        /// <value><c>true</c> if [view access]; otherwise, <c>false</c>.</value>
        [Required]
        [Display(Name = "View Access")]
        [DataMember(Order = 9)]
        public bool ViewAccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [edit access].
        /// </summary>
        /// <value><c>true</c> if [edit access]; otherwise, <c>false</c>.</value>
        [Required]
        [Display(Name = "Edit Access")]
        [DataMember(Order = 10)]
        public bool EditAccess { get; set; }


        //
        // Conversions between Db Entity space and gRPC space.
        //
        /// <summary>
        /// Gets the note access.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>NoteAccess.</returns>
        public static NoteAccess GetNoteAccess(GNoteAccess other)
        {
            NoteAccess a = new NoteAccess();
            a.UserID = other.UserID;
            a.NoteFileId = other.NoteFileId;
            a.ArchiveId = other.ArchiveId;
            a.ReadAccess = other.ReadAccess;
            a.Respond = other.Respond;
            a.Write = other.Write;
            a.SetTag = other.SetTag;
            a.DeleteEdit = other.DeleteEdit;
            a.ViewAccess = other.ViewAccess;
            a.EditAccess = other.EditAccess;
            return a;
        }

        /// <summary>
        /// Gets the g note access.
        /// </summary>
        /// <returns>GNoteAccess.</returns>
        public GNoteAccess GetGNoteAccess()
        {
            GNoteAccess a = new GNoteAccess();
            a.UserID = this.UserID;
            a.NoteFileId = this.NoteFileId;
            a.ArchiveId=this.ArchiveId;
            a.ReadAccess=this.ReadAccess;
            a.Respond  = this.Respond;
            a.Write=this.Write;
            a.SetTag=this.SetTag;
            a.DeleteEdit=this.DeleteEdit;
            a.ViewAccess=this.ViewAccess;
            a.EditAccess=this.EditAccess;
            return a;
        }

        /// <summary>
        /// Gets the note accesses.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>List&lt;NoteAccess&gt;.</returns>
        public static List<NoteAccess> GetNoteAccesses(GNoteAccessList other)
        {
            List<NoteAccess> list = new List<NoteAccess>();
            foreach (GNoteAccess a in other.List)
            {
                list.Add(GetNoteAccess(a));
            }
            return list;
        }

        /// <summary>
        /// Gets the g note access list.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>GNoteAccessList.</returns>
        public static GNoteAccessList GetGNoteAccessList(List<NoteAccess> other)
        {
            GNoteAccessList list = new GNoteAccessList();
            foreach (NoteAccess a in other)
            {
                list.List.Add(a.GetGNoteAccess());
            }
            return list;
        }

    }
}
