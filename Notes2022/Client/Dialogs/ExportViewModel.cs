

using Notes2022.Proto;
using System.ComponentModel.DataAnnotations;

namespace Notes2022.Client.Dialogs
{
    public class ExportViewModel
    {
        /// <summary>
        /// Notefile we are exporting from
        /// </summary>
        public GNotefile NoteFile { get; set; }

        /// <summary>
        /// Possible non 0 archive
        /// </summary>
        public int ArchiveNumber { get; set; }

        /// <summary>
        /// Is it to be in html format?
        /// </summary>
        public bool isHtml { get; set; }

        /// <summary>
        /// Collapsible or "flat"
        /// </summary>
        public bool isCollapsible { get; set; }

        /// <summary>
        /// Direct output or destination collected via a dailog?
        /// </summary>
        public bool isDirectOutput { get; set; }

        //public bool isOnPage { get; set; }

        /// <summary>
        /// NoteOrdinal to export - 0 for all notes
        /// </summary>
        public int NoteOrdinal { get; set; }

        /// <summary>
        /// "Marks" to limit scope of notes exportes the a specific set
        /// selected by user "Marked"
        /// </summary>
        public List<Mark> Marks { get; set; }

        /// <summary>
        /// Email address if being mailed to someone
        /// </summary>
        public string Email { get; set; }

    }


    //public class ForwardViewModel
    //{
    //    /// <summary>
    //    /// File involved
    //    /// </summary>
    //    public GNotefile NoteFile { get; set; }

    //    /// <summary>
    //    /// Id of note user wants
    //    /// </summary>
    //    public long NoteID { get; set; }

    //    /// <summary>
    //    /// File Id of file
    //    /// </summary>
    //    public int FileID { get; set; }

    //    /// <summary>
    //    /// Archive I of file
    //    /// </summary>
    //    public int ArcID { get; set; }

    //    /// <summary>
    //    /// Ordianal / note # involved
    //    /// </summary>
    //    public int NoteOrdinal { get; set; }

    //    /// <summary>
    //    /// Subject of note
    //    /// </summary>
    //    [Display(Name = "Subject")]
    //    public string NoteSubject { get; set; }

    //    /// <summary>
    //    /// FLag to send the whole string or just one note?
    //    /// </summary>
    //    [Display(Name = "Forward whole note string")]
    //    public bool wholestring { get; set; }

    //    /// <summary>
    //    /// Does this note have a string?
    //    /// </summary>
    //    public bool hasstring { get; set; }

    //    /// <summary>
    //    /// Is this user an Admin
    //    /// </summary>
    //    public bool IsAdmin { get; set; }

    //    /// <summary>
    //    /// Is the Admin sending the Everyone?
    //    /// </summary>
    //    public bool toAllUsers { get; set; }

    //    /// <summary>
    //    /// Target email address
    //    /// </summary>
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Forward to Email Address")]
    //    public string? ToEmail { get; set; }
    //}

    public class Mark
    {
        public string? UserId { get; set; }

        public int NoteFileId { get; set; }

        public int ArchiveId { get; set; }

        public int MarkOrdinal { get; set; }

        public int NoteOrdinal { get; set; }

        public long NoteHeaderId { get; set; }

        public int ResponseOrdinal { get; set; }  // -1 == whole string, 0 base note only, > 0 Response

        //[ForeignKey("NoteFileId")]
        //public NoteFile? NoteFile { get; set; }
    }

}
