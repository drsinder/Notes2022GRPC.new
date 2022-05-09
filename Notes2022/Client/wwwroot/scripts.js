// ***********************************************************************
// Assembly         : Notes2022.Client
// Author           : sinde
// Created          : 04-20-2022
//
// Last Modified By : sinde
// Last Modified On : 10-31-2021
// ***********************************************************************
// <copyright file="scripts.js" company="Notes2022.Client">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

export function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}

function getWidth(x) {
    return window.innerWidth;
}