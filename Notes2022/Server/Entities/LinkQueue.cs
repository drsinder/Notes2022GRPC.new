// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : sinde
// Created          : 04-19-2022
//
// Last Modified By : sinde
// Last Modified On : 04-20-2022
// ***********************************************************************
// <copyright file="LinkQueue.cs" company="Notes2022.Server">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: LinkQueue.cs
    **
    ** Description:
    **      Queue of linked items to process in background
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
using Notes2022.Proto;

namespace Notes2022.Server.Entities
{
    /// <summary>
    /// Enum LinkAction
    /// </summary>
    public enum LinkAction
    {
        /// <summary>
        /// The create base
        /// </summary>
        CreateBase,
        /// <summary>
        /// The create response
        /// </summary>
        CreateResponse,
        /// <summary>
        /// The edit
        /// </summary>
        Edit,
        /// <summary>
        /// The delete
        /// </summary>
        Delete
    };

    /// <summary>
    /// This class defines a table in the database.
    /// Used to que up items to be linked to remote system.
    /// </summary>
    public class LinkQueue
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the linked file identifier.
        /// </summary>
        /// <value>The linked file identifier.</value>
        [Required]
        public int LinkedFileId { get; set; }

        /// <summary>
        /// Gets or sets the link unique identifier.
        /// </summary>
        /// <value>The link unique identifier.</value>
        [Required]
        [StringLength(100)]
        public string? LinkGuid { get; set; }

        /// <summary>
        /// Gets or sets the activity.
        /// </summary>
        /// <value>The activity.</value>
        [Required]
        public LinkAction Activity { get; set; }

        /// <summary>
        /// Gets or sets the base URI.
        /// </summary>
        /// <value>The base URI.</value>
        [Required]
        public string? BaseUri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LinkQueue"/> is enqueued.
        /// </summary>
        /// <value><c>true</c> if enqueued; otherwise, <c>false</c>.</value>
        public bool Enqueued { get; set; }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>The secret.</value>
        [StringLength(50)]
        public string? Secret { get; set; }


        //
        // Conversions between Db Entity space and gRPC space.
        //
        /// <summary>
        /// Gets the link queue.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>LinkQueue.</returns>
        public static LinkQueue GetLinkQueue(GLinkQueue other)
        {
            LinkQueue s = new LinkQueue();
            s.Id = other.Id;
            s.LinkedFileId = other.LinkedFileId;
            s.LinkGuid = other.LinkGuid;
            s.Activity = (LinkAction)other.Activity;
            s.BaseUri = other.BaseUri;
            s.Enqueued = other.Enqueued;
            s.Secret = other.Secret;
            return s;
        }

        /// <summary>
        /// Gets the g link queue.
        /// </summary>
        /// <returns>GLinkQueue.</returns>
        public GLinkQueue GetGLinkQueue()
        {
            GLinkQueue s = new GLinkQueue();
            s.Id = this.Id;
            s.LinkedFileId=this.LinkedFileId;
            s.LinkGuid=this.LinkGuid;
            s.Activity = (int)this.Activity;
            s.BaseUri=this.BaseUri;
            s.Enqueued=this.Enqueued;
            s.Secret=this.Secret;
            return s;
        }

        /// <summary>
        /// Gets the sequencer list.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>List&lt;LinkQueue&gt;.</returns>
        public static List<LinkQueue> GetSequencerList(GLinkQueueList other)
        {
            List<LinkQueue> list = new List<LinkQueue>();
            foreach (GLinkQueue t in other.List)
            {
                list.Add(GetLinkQueue(t));
            }
            return list;
        }

        /// <summary>
        /// Gets the g sequencer list.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>GLinkQueueList.</returns>
        public static GLinkQueueList GetGSequencerList(List<LinkQueue> other)
        {
            GLinkQueueList list = new GLinkQueueList();
            foreach (LinkQueue t in other)
            {
                list.List.Add(t.GetGLinkQueue());
            }
            return list;
        }

    }
}
