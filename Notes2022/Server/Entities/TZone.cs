// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : sinde
// Created          : 04-19-2022
//
// Last Modified By : sinde
// Last Modified On : 04-14-2022
// ***********************************************************************
// <copyright file="TZone.cs" company="Notes2022.Server">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: TZone.cs
    **
    ** Description:
    **      Time Zones of the world for user selection
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

namespace Notes2022.Server.Entities
{
    /// <summary>
    /// Obsolete with Blazor WASM
    /// </summary>
    [DataContract]
    public class TZone
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // ReSharper disable once InconsistentNaming
        [DataMember(Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        [StringLength(200)]
        [DataMember(Order = 2)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>The abbreviation.</value>
        [Required]
        [StringLength(10)]
        [DataMember(Order = 3)]
        public string? Abbreviation { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
        [Required]
        [DataMember(Order = 4)]
        public string? Offset { get; set; }

        /// <summary>
        /// Gets or sets the offset hours.
        /// </summary>
        /// <value>The offset hours.</value>
        [Required]
        [DataMember(Order = 5)]
        public int OffsetHours { get; set; }

        /// <summary>
        /// Gets or sets the offset minutes.
        /// </summary>
        /// <value>The offset minutes.</value>
        [Required]
        [DataMember(Order = 6)]
        public int OffsetMinutes { get; set; }

        /// <summary>
        /// Locals the specified dt.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>DateTime.</returns>
        public DateTime Local(DateTime dt)
        {
            return dt.AddHours(OffsetHours).AddMinutes(OffsetMinutes);
        }

        //public DateTime LocalBlazor(DateTime dt)
        //{
        //    int OHours = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours;
        //    int OMinutes = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Minutes;

        //    return dt.AddHours(OHours).AddMinutes(OMinutes);
        //}


        /// <summary>
        /// Universals the specified dt.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>DateTime.</returns>
        public DateTime Universal(DateTime dt)
        {
            return dt.AddHours(-OffsetHours).AddMinutes(-OffsetMinutes);
        }
    }

}
