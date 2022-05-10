// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : Dale Sinder
// Created          : 04-19-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 04-14-2022
// ***********************************************************************
// <copyright file="SQLFile.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: SQLFiles.cs
    **
    ** Description:
    **      File info record and data record for uploaded files
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
    public class SQLFile
    {
        /// <summary>
        /// Gets or sets the file identifier.
        /// </summary>
        /// <value>The file identifier.</value>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FileId { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        [Required]
        [StringLength(300)]
        public string? FileName { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        [Required]
        [StringLength(100)]
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets the contributor.
        /// </summary>
        /// <value>The contributor.</value>
        [Required]
        [StringLength(300)]
        public string? Contributor { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public SQLFileContent? Content { get; set; }


        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        [StringLength(1000)]
        public string? Comments { get; set; }

    }

    /// <summary>
    /// This class defines a table in the database.
    /// Not currently in use.
    /// </summary>
    public class SQLFileContent
    {

        /// <summary>
        /// Gets or sets the SQL file identifier.
        /// </summary>
        /// <value>The SQL file identifier.</value>
        [Key]
        public long SQLFileId { get; set; }

        /// <summary>
        /// Gets or sets the SQL file.
        /// </summary>
        /// <value>The SQL file.</value>
        public SQLFile? SQLFile { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        [Required]
        public byte[]? Content { get; set; }
    }
}
