// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : Dale Sinder
// Created          : 04-19-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 04-14-2022
// ***********************************************************************
// <copyright file="Audit.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright© 2021, Dale Sinder
    **
    ** Name: Audit.cs
    **
    ** Description:
    **      Audit DB record
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

namespace Notes2022.Server.Entities
{
    /// <summary>
    /// This class defines a table in the database.
    /// Not currently in use.
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// Gets or sets the audit identifier.
        /// </summary>
        /// <value>The audit identifier.</value>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AuditID { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        [Required]
        [StringLength(20)]
        [Display(Name = "Event Type")]
        public string? EventType { get; set; }

        // Name of the user did made it
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        [Required]
        [StringLength(256)]
        [Display(Name = "User Name")]
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        [StringLength(450)]
        public string? UserID { get; set; }

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
        [StringLength(1000)]
        [Display(Name = "Event")]
        public string? Event { get; set; }
    }
}
