// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : sinde
// Created          : 04-19-2022
//
// Last Modified By : sinde
// Last Modified On : 04-20-2022
// ***********************************************************************
// <copyright file="LinkLog.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: LinkLog.cs
    **
    ** Description:
    **      Log of link actions
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
using Google.Protobuf.WellKnownTypes;


namespace Notes2022.Server.Entities
{
    /// <summary>
    /// This class defines a table in the database.
    /// Log of link activity.
    /// </summary>
    public class LinkLog
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
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        [Required]
        [StringLength(20)]
        [Display(Name = "Event Type")]
        public string? EventType { get; set; }

        /// <summary>
        /// Gets or sets the event time.
        /// </summary>
        /// <value>The event time.</value>
        [Required]
        [Display(Name = "Event Time")]
        public DateTime EventTime { get; set; }

        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>The event.</value>
        [Required]
        [Display(Name = "Event")]
        public string? Event { get; set; }

        //
        // Conversions between Db Entity space and gRPC space.
        //
        /// <summary>
        /// Gets the link log.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>LinkLog.</returns>
        public static LinkLog GetLinkLog(GLinkLog other)
        {
            LinkLog s = new LinkLog();
            s.Id = other.Id;
            s.EventType = other.EventType;
            s.EventTime = other.EventTime.ToDateTime();
            s.Event = other.Event;
            return s;
        }

        /// <summary>
        /// Gets the g link log.
        /// </summary>
        /// <returns>GLinkLog.</returns>
        public GLinkLog GetGLinkLog()
        {
            GLinkLog s = new GLinkLog();
            s.Id=Id;
            s.EventType=EventType;
            s.Event=Event;
            s.EventTime= Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(EventTime.ToUniversalTime());
            return s;
        }

        /// <summary>
        /// Gets the link log list.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>List&lt;LinkLog&gt;.</returns>
        public static List<LinkLog> GetLinkLogList(GLinkLogList other)
        {
            List<LinkLog> list = new List<LinkLog>();
            foreach (GLinkLog t in other.List)
            {
                list.Add(GetLinkLog(t));
            }
            return list;
        }

        /// <summary>
        /// Gets the g sequencer list.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>GLinkLogList.</returns>
        public static GLinkLogList GetGSequencerList(List<LinkLog> other)
        {
            GLinkLogList list = new GLinkLogList();
            foreach (LinkLog t in other)
            {
                list.List.Add(t.GetGLinkLog());
            }
            return list;
        }

    }
}
