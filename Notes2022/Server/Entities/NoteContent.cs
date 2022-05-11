// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : Dale Sinder
// Created          : 04-19-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 05-08-2022
// ***********************************************************************
// <copyright file="NoteContent.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: NoteContent.cs
    **
    ** Description:
    **      Note body and director message for note
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
using System.Runtime.Serialization;
using Notes2022.Proto;

namespace Notes2022.Server.Entities
{
    /// <summary>
    /// This class defines a table in the database.
    /// Each NoteContent object is associated with one NoteHeader
    /// It contains the "Body" of the note.
    /// </summary>
    [DataContract]
    public class NoteContent
    {
        /// <summary>
        /// Gets or sets the note header identifier.
        /// </summary>
        /// <value>The note header identifier.</value>
        [Required]
        [Key]
        [DataMember(Order = 1)]
        public long NoteHeaderId { get; set; }


        /// <summary>
        /// Gets or sets the note body.
        /// The Body or content of the note
        /// </summary>
        /// <value>The note body.</value>
        [Required]
        [StringLength(100000)]
        [Display(Name = "Note")]
        [DataMember(Order = 2)]
        public string? NoteBody { get; set; }

        /// <summary>
        /// Clones for link.
        /// </summary>
        /// <returns>NoteContent.</returns>
        public NoteContent CloneForLink()
        {
            NoteContent nc = new NoteContent()
            {
                NoteBody = NoteBody,
                //DirectorMessage = DirectorMessage
            };

            return nc;
        }

        /// <summary>
        /// Gets the content of the note.
        /// Conversions between Db Entity space and gRPC space.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>NoteContent.</returns>
        public static NoteContent GetNoteContent(GNoteContent other)
        {
            NoteContent c = new NoteContent();
            c.NoteHeaderId = other.NoteHeaderId;
            c.NoteBody = other.NoteBody;
            return c;
        }

        /// <summary>
        /// Gets the content of the g note.
        /// Conversions between Db Entity space and gRPC space.
        /// </summary>
        /// <returns>GNoteContent.</returns>
        public GNoteContent GetGNoteContent()
        {
            GNoteContent nc = new GNoteContent();
            nc.NoteHeaderId = this.NoteHeaderId;
            nc.NoteBody = this.NoteBody;
            return nc;
        }

        /// <summary>
        /// Gets the note contents.
        /// Conversions between Db Entity space and gRPC space.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>List&lt;NoteContent&gt;.</returns>
        public static List<NoteContent> GetNoteContents(GNoteContentList other)
        {
            List<NoteContent> list = new List<NoteContent>();
            foreach (GNoteContent c in other.List)
            {
                list.Add(GetNoteContent(c));
            }
            return list;
        }

        /// <summary>
        /// Gets the g note content list.
        /// Conversions between Db Entity space and gRPC space.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>GNoteContentList.</returns>
        public static GNoteContentList GetGNoteContentList(List<NoteContent> other)
        {
            GNoteContentList list = new GNoteContentList();
            foreach (NoteContent c in other)
            {
                list.List.Add(c.GetGNoteContent());
            }
            return list;
        }

    }
}
