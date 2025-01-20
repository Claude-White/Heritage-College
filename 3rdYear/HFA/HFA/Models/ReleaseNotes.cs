using HFA.Resources;
using System.ComponentModel.DataAnnotations;

namespace HFA.Models
{
    public class ReleaseNotes
    {
        public int ReleaseNoteId { get; set; }

        public DateTime PublishedDate { get; set; }

        [Required(ErrorMessage = "The title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The version number is required")]
        [RegularExpression(pattern: "^[0-9]{1,2}\\.[0-9]{1,2}\\.[0-9]{1,2}$", ErrorMessage = "The version number should be #.#.#")]
        public string? ReleaseVersion { get; set; }

        [Required(ErrorMessage = "A short summary is required")]
        public string? ShortSummary { get; set; }

        public string? Content { get; set; } = "<h5 class='font-bold text-secondary'>" + @HFA_Resources.Updates + "</h5><ul class='list-disc'><li>Please enter the updates</li></ul><h5 class='font-bold text-secondary'>" + @HFA_Resources.BugFixes + "</h5>"
    + "<ul class='list-disc'><li>Please enter the bug fixes</li></ul><h5 class='font-bold text-secondary'>" + @HFA_Resources.OtherNotes + "</h5><p>Please enter the other notes that are relevant to these release notes</p>";

        public enum Type { Updates = 1, BugFixes = 2, KnownIssues = 3 };

        public ReleaseNotes()
        {
        }

        public ReleaseNotes(DateTime publishedDate, string? title, string? releaseVersion, string? shortSummary, string content)
        {
            PublishedDate = publishedDate;
            Title = title;
            ReleaseVersion = releaseVersion;
            ShortSummary = shortSummary;
            Content = content;
        }

        public ReleaseNotes(int id, DateTime publishedDate, string? title, string? releaseVersion, string? shortSummary, string? content)
        {
            ReleaseNoteId = id;
            PublishedDate = publishedDate;
            Title = title;
            ReleaseVersion = releaseVersion;
            ShortSummary = shortSummary;
            Content = content;
        }
    }
}
