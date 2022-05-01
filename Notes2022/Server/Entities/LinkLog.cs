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
    /// 
    /// Log of link activity.
    /// 
    /// </summary>
    public class LinkLog
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Event Type")]
        public string? EventType { get; set; }

        [Required]
        [Display(Name = "Event Time")]
        public DateTime EventTime { get; set; }

        [Required]
        [Display(Name = "Event")]
        public string? Event { get; set; }

        //
        // Conversions between Db Entity space and gRPC space.
        //
        public static LinkLog GetLinkLog(GLinkLog other)
        {
            LinkLog s = new LinkLog();
            s.Id = other.Id;
            s.EventType = other.EventType;
            s.EventTime = other.EventTime.ToDateTime();
            s.Event = other.Event;
            return s;
        }

        public GLinkLog GetGLinkLog()
        {
            GLinkLog s = new GLinkLog();
            s.Id=Id;
            s.EventType=EventType;
            s.Event=Event;
            s.EventTime= Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(EventTime.ToUniversalTime());
            return s;
        }

        public static List<LinkLog> GetLinkLogList(GLinkLogList other)
        {
            List<LinkLog> list = new List<LinkLog>();
            foreach (GLinkLog t in other.List)
            {
                list.Add(GetLinkLog(t));
            }
            return list;
        }

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
