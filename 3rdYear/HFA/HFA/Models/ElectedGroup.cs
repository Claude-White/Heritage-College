using System.ComponentModel.DataAnnotations;

namespace HFA.Models
{
    public class ElectedGroup
    {
        public int GroupId { get; set; }

        [Required(ErrorMessage = "The group's name is required.")]
        public string? GroupName { get; set; }

        public virtual List<ElectedMember>? Members { get; set; }

        public ElectedGroup() { }

        public ElectedGroup(int groupId, string groupName)
        {
            GroupId = groupId;
            GroupName = groupName;
        }
    }
}
