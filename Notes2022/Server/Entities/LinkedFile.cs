﻿// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : sinde
// Created          : 04-19-2022
//
// Last Modified By : sinde
// Last Modified On : 04-20-2022
// ***********************************************************************
// <copyright file="LinkedFile.cs" company="Notes2022.Server">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// It is used to link a file to a file on a remote system
    /// such that notes written on one system will be transmitted
    /// to the other.
    /// </summary>
    [DataContract]
    public class LinkedFile
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the home file identifier.
        /// </summary>
        /// <value>The home file identifier.</value>
        [Required]
        [DataMember(Order = 2)]
        public int HomeFileId { get; set; }

        /// <summary>
        /// Gets or sets the name of the home file.
        /// </summary>
        /// <value>The name of the home file.</value>
        [Required]
        [StringLength(20)]
        [DataMember(Order = 3)]
        public string? HomeFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the remote file.
        /// </summary>
        /// <value>The name of the remote file.</value>
        [Required]
        [StringLength(20)]
        [DataMember(Order = 4)]
        public string? RemoteFileName { get; set; }

        /// <summary>
        /// Gets or sets the remote base URI.
        /// </summary>
        /// <value>The remote base URI.</value>
        [Required]
        [StringLength(450)]
        [DataMember(Order = 5)]
        public string? RemoteBaseUri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [accept from].
        /// </summary>
        /// <value><c>true</c> if [accept from]; otherwise, <c>false</c>.</value>
        [Required]
        [DataMember(Order = 6)]
        public bool AcceptFrom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [send to].
        /// </summary>
        /// <value><c>true</c> if [send to]; otherwise, <c>false</c>.</value>
        [Required]
        [DataMember(Order = 7)]
        public bool SendTo { get; set; }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>The secret.</value>
        [StringLength(50)]
        [DataMember(Order = 8)]
        public string? Secret { get; set; }


        //
        // Conversions between Db Entity space and gRPC space.
        //
        /// <summary>
        /// Gets the linked file.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>LinkedFile.</returns>
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

        /// <summary>
        /// Gets the g linked file.
        /// </summary>
        /// <returns>GLinkedFile.</returns>
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

        /// <summary>
        /// Gets the sequencer list.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>List&lt;LinkedFile&gt;.</returns>
        public static List<LinkedFile> GetSequencerList(GLinkedFileList other)
        {
            List<LinkedFile> list = new List<LinkedFile>();
            foreach (GLinkedFile t in other.List)
            {
                list.Add(GetLinkedFile(t));
            }
            return list;
        }

        /// <summary>
        /// Gets the g linked file list.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>GLinkedFileList.</returns>
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
