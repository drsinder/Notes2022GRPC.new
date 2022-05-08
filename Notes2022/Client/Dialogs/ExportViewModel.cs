using Notes2022.Client.Menus;
using Notes2022.Proto;

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

        public ListMenu myMenu { get; set; }
    }

    public class Mark
    {
        public string? UserId { get; set; }

        public int NoteFileId { get; set; }

        public int ArchiveId { get; set; }

        public int MarkOrdinal { get; set; }

        public int NoteOrdinal { get; set; }

        public long NoteHeaderId { get; set; }

        public int ResponseOrdinal { get; set; }  // -1 == whole string, 0 base note only, > 0 Response
    }

}
