/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: LinkedFile.cs
    **
    ** Description:
    **      Represents a linked file
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
    /// 
    /// It is used to link a file to a file on a remote system
    /// such that notes written on one system will be transmitted
    /// to the other.
    /// 
    /// </summary>
    [DataContract]
    public class LinkedFile
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [Required]
        [DataMember(Order = 2)]
        public int HomeFileId { get; set; }

        [Required]
        [StringLength(20)]
        [DataMember(Order = 3)]
        public string? HomeFileName { get; set; }

        [Required]
        [StringLength(20)]
        [DataMember(Order = 4)]
        public string? RemoteFileName { get; set; }

        [Required]
        [StringLength(450)]
        [DataMember(Order = 5)]
        public string? RemoteBaseUri { get; set; }

        [Required]
        [DataMember(Order = 6)]
        public bool AcceptFrom { get; set; }

        [Required]
        [DataMember(Order = 7)]
        public bool SendTo { get; set; }

        [StringLength(50)]
        [DataMember(Order = 8)]
        public string? Secret { get; set; }


        //
        // Conversions between Db Entity space and gRPC space.
        //
        public static LinkedFile GetLinkedFile(GLinkedFile other)
        {
            LinkedFile s = new LinkedFile();
            s.Id = other.Id;
            s.HomeFileId = other.HomeFileId;
            s.HomeFileName = other.HomeFileName;
            s.RemoteFileName = other.RemoteFileName;
            s.RemoteBaseUri = other.RemoteBaseUri;
            s.AcceptFrom = other.AcceptFrom;
            s.SendTo = other.SendTo;
            s.Secret = other.Secret;
            return s;
        }

        public GLinkedFile GetGLinkedFile()
        {
            GLinkedFile s = new GLinkedFile();
            s.Id = this.Id;
            s.HomeFileId=this.HomeFileId;
            s.HomeFileName=this.HomeFileName;
            s.RemoteFileName=this.RemoteFileName;
            s.RemoteBaseUri=this.RemoteBaseUri;
            s.AcceptFrom=this.AcceptFrom;
            s.SendTo=this.SendTo;
            s.Secret=this.Secret;
            return s;
        }

        public static List<LinkedFile> GetSequencerList(GLinkedFileList other)
        {
            List<LinkedFile> list = new List<LinkedFile>();
            foreach (GLinkedFile t in other.List)
            {
                list.Add(GetLinkedFile(t));
            }
            return list;
        }

        public static GLinkedFileList GetGLinkedFileList(List<LinkedFile> other)
        {
            GLinkedFileList list = new GLinkedFileList();
            foreach (LinkedFile t in other)
            {
                list.List.Add(t.GetGLinkedFile());
            }
            return list;
        }

    }
}
