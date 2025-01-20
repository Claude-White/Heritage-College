using System.Collections;

namespace HFA.Models
{
    public class RESTResources(string _POST, string _PUT, string _DELETE, string? _GET = null)
    {
        public string POST { get; set; } = _POST;
        public string PUT { get; set; } = _PUT;
        public string DELETE { get; set; } = _DELETE;
        public string? GET { get; set; } = _GET;
    }
}