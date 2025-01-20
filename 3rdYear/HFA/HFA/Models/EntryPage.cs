using System.Collections;

namespace HFA.Models
{
    public class EntryPage(IEnumerable _Entries, string _PageTitle, object _TemplateModel, RESTResources _RESTResources, bool _AreRowsSummary = false, string _RawDisplay = "")
    {
        public IEnumerable Entries { get; set; } = _Entries;
        public string PageTitle { get; set; } = _PageTitle;
        public object TemplateModel { get; set; } = _TemplateModel;
        public RESTResources RESTResources { get; set; } = _RESTResources;
        public bool AreRowsSummary { get; set; } = _AreRowsSummary;
        public string RawDisplay { get; set; } = _RawDisplay;
    }
}