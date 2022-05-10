// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : sinde
// Created          : 04-19-2022
//
// Last Modified By : sinde
// Last Modified On : 04-14-2022
// ***********************************************************************
// <copyright file="HomePageMessage.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: HomePageMessage.cs
    **
    ** Description:
    **      HOme Page MEssage DB record
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
    /// Obsolete
    /// </summary>
    public class HomePageMessage
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [Required]
        [StringLength(1000)]
        public string? Message { get; set; }
        /// <summary>
        /// Gets or sets the posted.
        /// </summary>
        /// <value>The posted.</value>
        [Required]
        public DateTime Posted { get; set; }
    }
}
