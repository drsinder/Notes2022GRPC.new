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
    public enum LinkAction
    {
        CreateBase,
        CreateResponse,
        Edit,
        Delete
    };

    /// <summary>
    /// This class defines a table in the database.
    /// 
    /// Used to que up items to be linked to remote system.
    /// </summary>
    public class LinkQueue
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public int LinkedFileId { get; set; }

        [Required]
        [StringLength(100)]
        public string? LinkGuid { get; set; }

        [Required]
        public LinkAction Activity { get; set; }

        [Required]
        public string? BaseUri { get; set; }

        public bool Enqueued { get; set; }

        [StringLength(50)]
        public string? Secret { get; set; }


        //
        // Conversions between Db Entity space and gRPC space.
        //
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

        public static List<LinkQueue> GetSequencerList(GLinkQueueList other)
        {
            List<LinkQueue> list = new List<LinkQueue>();
            foreach (GLinkQueue t in other.List)
            {
                list.Add(GetLinkQueue(t));
            }
            return list;
        }

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
