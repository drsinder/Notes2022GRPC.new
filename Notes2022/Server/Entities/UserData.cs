// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : sinde
// Created          : 04-19-2022
//
// Last Modified By : sinde
// Last Modified On : 11-29-2021
// ***********************************************************************
// <copyright file="UserData.cs" company="Notes2022.Server">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: UserData.cs
    **
    ** Description:
    **      User Preferences
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
using System.Runtime.Serialization;

namespace Notes2022.Shared
{
    /// <summary>
    /// This class does NOT define a table in the database!
    /// It is a local mirror of the extra data fields added to the
    /// ApplicationUser.
    /// </summary>
    [DataContract]
    public class UserData
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        [Key]
        [StringLength(450)]
        [DataMember(Order = 1)]
        public string UserId { get; set; }

        //[Display(Name = "Display Name")]
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        [StringLength(50)]
        [DataMember(Order = 2)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the display name2.
        /// </summary>
        /// <value>The display name2.</value>
        public string DisplayName2
        {
            get { return DisplayName.Replace(" ", "_"); }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [StringLength(150)]
        [DataMember(Order = 3)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the time zone identifier.
        /// </summary>
        /// <value>The time zone identifier.</value>
        [DataMember(Order = 4)]
        public int TimeZoneID { get; set; }

        /// <summary>
        /// Gets or sets the ipref0.
        /// </summary>
        /// <value>The ipref0.</value>
        [DataMember(Order = 5)]
        public int Ipref0 { get; set; }

        /// <summary>
        /// Gets or sets the ipref1.
        /// </summary>
        /// <value>The ipref1.</value>
        [DataMember(Order = 6)]
        public int Ipref1 { get; set; }

        /// <summary>
        /// Gets or sets the ipref2.
        /// </summary>
        /// <value>The ipref2.</value>
        [DataMember(Order = 7)]
        public int Ipref2 { get; set; } // user choosen page size

        /// <summary>
        /// Gets or sets the ipref3.
        /// </summary>
        /// <value>The ipref3.</value>
        [DataMember(Order = 8)]
        public int Ipref3 { get; set; }

        /// <summary>
        /// Gets or sets the ipref4.
        /// </summary>
        /// <value>The ipref4.</value>
        [DataMember(Order = 9)]
        public int Ipref4 { get; set; }

        /// <summary>
        /// Gets or sets the ipref5.
        /// </summary>
        /// <value>The ipref5.</value>
        [DataMember(Order = 10)]
        public int Ipref5 { get; set; }

        /// <summary>
        /// Gets or sets the ipref6.
        /// </summary>
        /// <value>The ipref6.</value>
        [DataMember(Order = 11)]
        public int Ipref6 { get; set; }

        /// <summary>
        /// Gets or sets the ipref7.
        /// </summary>
        /// <value>The ipref7.</value>
        [DataMember(Order = 12)]
        public int Ipref7 { get; set; }

        /// <summary>
        /// Gets or sets the ipref8.
        /// </summary>
        /// <value>The ipref8.</value>
        [DataMember(Order = 13)]
        public int Ipref8 { get; set; }

        /// <summary>
        /// Gets or sets the ipref9.
        /// </summary>
        /// <value>The ipref9.</value>
        [DataMember(Order = 14)]
        public int Ipref9 { get; set; } // bits extend bool properties


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref0.
        /// </summary>
        /// <value><c>true</c> if pref0; otherwise, <c>false</c>.</value>
        [DataMember(Order = 15)]
        public bool Pref0 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref1.
        /// </summary>
        /// <value><c>true</c> if pref1; otherwise, <c>false</c>.</value>
        [DataMember(Order = 16)]
        public bool Pref1 { get; set; } // false = use paged note index, true= scrolled

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref2.
        /// </summary>
        /// <value><c>true</c> if pref2; otherwise, <c>false</c>.</value>
        [DataMember(Order = 17)]
        public bool Pref2 { get; set; } // use alternate editor - obsolete

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref3.
        /// </summary>
        /// <value><c>true</c> if pref3; otherwise, <c>false</c>.</value>
        [DataMember(Order = 18)]
        public bool Pref3 { get; set; } // show responses by default

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref4.
        /// </summary>
        /// <value><c>true</c> if pref4; otherwise, <c>false</c>.</value>
        [DataMember(Order = 19)]
        public bool Pref4 { get; set; } // multiple expanded responses

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref5.
        /// </summary>
        /// <value><c>true</c> if pref5; otherwise, <c>false</c>.</value>
        [DataMember(Order = 20)]
        public bool Pref5 { get; set; } // expanded responses

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref6.
        /// </summary>
        /// <value><c>true</c> if pref6; otherwise, <c>false</c>.</value>
        [DataMember(Order = 21)]
        public bool Pref6 { get; set; } // alternate text editor

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref7.
        /// </summary>
        /// <value><c>true</c> if pref7; otherwise, <c>false</c>.</value>
        [DataMember(Order = 22)]
        public bool Pref7 { get; set; } // show content when expanded

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref8.
        /// </summary>
        /// <value><c>true</c> if pref8; otherwise, <c>false</c>.</value>
        [DataMember(Order = 23)]
        public bool Pref8 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserData"/> is pref9.
        /// </summary>
        /// <value><c>true</c> if pref9; otherwise, <c>false</c>.</value>
        [DataMember(Order = 24)]
        public bool Pref9 { get; set; }

        /// <summary>
        /// Gets or sets my unique identifier.
        /// </summary>
        /// <value>My unique identifier.</value>
        [StringLength(100)]
        [DataMember(Order = 25)]
        public string? MyGuid { get; set; }

    }
}
