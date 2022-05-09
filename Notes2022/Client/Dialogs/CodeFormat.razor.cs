using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.RichTextEditor;
using System.Text;
using System.Web;

namespace Notes2022.Client.Dialogs
{
    /// <summary>
    /// Prepare a block of code for insertion
    /// </summary>
    public partial class CodeFormat
    {
        [CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }
#pragma warning disable IDE1006 // Naming Styles
        [Parameter] public string stuff { get; set; }
        [Parameter] public SfRichTextEditor EditObj { get; set; }

        protected SfTextBox TextObj { get; set; }
        protected string message { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        protected bool IsEditing { get; set; } = false;
        public string DropVal;

        public class CFormat
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public List<CFormat> CFormats = new()
        {
            new CFormat() { Name = "None",  Code = "none" },
            new CFormat() { Name = "C#",    Code = "csharp" },
            new CFormat() { Name = "C++",   Code = "cpp" },
            new CFormat() { Name = "C",     Code = "c" },
            new CFormat() { Name = "Razor", Code = "razor" },
            new CFormat() { Name = "Css",   Code = "css" },
            new CFormat() { Name = "Java",  Code = "java" },
            new CFormat() { Name = "JavaScript", Code = "js" },
            new CFormat() { Name = "Json",  Code = "json" },
            new CFormat() { Name = "Html",  Code = "html" },
            new CFormat() { Name = "Perl",  Code = "perl" },
            new CFormat() { Name = "Php",   Code = "php" },
            new CFormat() { Name = "SQL",   Code = "sql" },
            new CFormat() { Name = "PowerShell", Code = "powershell" }
        };

        protected override void OnParametersSet()
        {
            message = stuff;
            IsEditing = !string.IsNullOrEmpty(stuff);   // not yet permitted at higher levels
        }

        private async Task Ok()
        {
            string code;

            if (DropVal is not null && !string.IsNullOrEmpty(DropVal))
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                code = CFormats.Find(p => p.Name == DropVal).Code;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            else
                code = "none";

            switch (code)
            {
                case "none":
                    message = HttpUtility.HtmlEncode(TextObj.Value);
                    break;

                default:
                    message = HttpUtility.HtmlEncode(TextObj.Value);
                    message = MakeCode(message, code);
                    break;
            }
            //await EditObj.ExecuteCommandAsync(CommandName.InsertHTML, message);
            await ModalInstance.CloseAsync(ModalResult.Ok(message));
        }

        /// <summary>
        /// Surround user provided code with some html <pre> and <code> for prism
        /// highlighting
        /// </summary>
        /// <param name="stuff2"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        private static string MakeCode(string stuff2, string codeType)
        {
            StringBuilder sb = new();

            sb.Append("<pre><code class=\"language-");
            sb.Append(codeType);
            sb.Append("\">");
            sb.Append(stuff2);
            sb.Append("</code></pre>");

            return sb.ToString();
        }

        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }
    }
}
