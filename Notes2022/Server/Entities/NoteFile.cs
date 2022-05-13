// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : Dale Sinder
// Created          : 04-19-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="NoteFile.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteFile.cs
    **
    ** Description:
    **      NoteFile record
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
using Google.Protobuf.WellKnownTypes;

namespace Notes2022.Server.Entities
{
    /// <summary>
    /// This class defines a table in the database.
    /// Objects of this class serve as the highest level
    /// of the hierarchy of the system.  Notes are
    /// thought to be contained in a file, but are
    /// in fact are related to it.  Classes directly
    /// related the a File:
    /// NoteAccess - Access tokens
    /// NoteHeader - descriptor for a note
    /// |-- NoteContent - via a relation to NoteHeader
    /// |-- Tags - via direct relation and via NoteHeader
    /// Subscription - a way to get email for new notes
    /// Sequencer  - a way to keep track of recent notes
    /// Mark       - a way to mark notes for later output
    /// See each of these for more detail.
    /// NoteFiles have a unique integer Id
    /// NoteFiles have a File Name and a File Title
    /// The File Naame is case sensitive.
    /// They are owned by their creator, an Admin.
    /// They also have a count of the number of archives they
    /// have and a LastEdited DateTime
    /// Files can be found by name or Id.
    /// </summary>
    [DataContract]
    public class NoteFile
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// Identity of the file
        /// </summary>
        /// <value>The identifier.</value>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the number archives.
        /// </summary>
        /// <value>The number archives.</value>
        [Required]
        [DataMember(Order = 2)]
        public int NumberArchives { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>The owner identifier.</value>
        [Required]
        [Display(Name = "Owner ID")]
        [StringLength(450)]
        [DataMember(Order = 3)]
        public string? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the note file.
        /// </summary>
        /// <value>The name of the note file.</value>
        [Required]
        [StringLength(20)]
        [Display(Name = "NoteFile Name")]
        [DataMember(Order = 4)]
        public string? NoteFileName { get; set; }

        /// <summary>
        /// Gets or sets the note file title.
        /// </summary>
        /// <value>The note file title.</value>
        [Required]
        [StringLength(200)]
        [Display(Name = "NoteFile Title")]
        [DataMember(Order = 5)]
        public string? NoteFileTitle { get; set; }

        /// <summary>
        /// Gets or sets the last edited.
        /// when anything in the file was last created or edited
        /// </summary>
        /// <value>The last edited.</value>
        [Required]
        [Display(Name = "Last Edited")]
        [DataMember(Order = 6)]
        public DateTime LastEdited { get; set; }

        //
        // Conversions between Db Entity space and gRPC space.
        
        /// <summary>
        /// Gets the note file.
        /// Conversions between Db Entity space and gRPC space.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>NoteFile.</returns>
        public static NoteFile GetNoteFile(GNotefile other)
        {
            NoteFile noteFile = new NoteFile();
            noteFile.Id = other.Id;
            noteFile.NumberArchives = other.NumberArchives;
            noteFile.OwnerId = other.OwnerId;
            noteFile.NoteFileName = other.NoteFileName;
            noteFile.NoteFileTitle = other.NoteFileTitle;
            noteFile.LastEdited = other.LastEdited.ToDateTime();
            return noteFile;
        }

        /// <summary>
        /// Gets the g notefile.
        /// Conversions between Db Entity space and gRPC space.
        /// </summary>
        /// <returns>GNotefile.</returns>
        public GNotefile GetGNotefile()
        {
            GNotefile notefile = new GNotefile();
            notefile.Id = Id;
            notefile.NumberArchives = NumberArchives;
            notefile.OwnerId = OwnerId;
            notefile.NoteFileName = NoteFileName;
            notefile.NoteFileTitle = NoteFileTitle;
            notefile.LastEdited = Timestamp.FromDateTime(Globals.UTimeBlazor(LastEdited).ToUniversalTime());
            return notefile;
        }

        /// <summary>
        /// Gets the note files.
        /// Conversions between Db Entity space and gRPC space.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>List&lt;NoteFile&gt;.</returns>
        public static List<NoteFile> GetNoteFiles(GNotefileList other)
        {
            List<NoteFile> list = new List<NoteFile>();
            foreach (GNotefile notefile in other.List)
            {
                list.Add(GetNoteFile(notefile));
            }
            return list;
        }

        /// <summary>
        /// Gets the g notefile list.  Conversions between Db Entity space and gRPC space.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>GNotefileList.</returns>
        public static GNotefileList GetGNotefileList(List<NoteFile> other)
        {
            GNotefileList list = new GNotefileList();
            foreach (NoteFile notefile in other)
            {
                list.List.Add(notefile.GetGNotefile());
            }
            return list;
        }

    }
}
