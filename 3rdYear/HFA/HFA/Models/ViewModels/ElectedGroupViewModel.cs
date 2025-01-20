using System.ComponentModel.DataAnnotations;

namespace HFA.Models.ViewModels
{
    public class ElectedGroupViewModel
    {
        [Key]
        public int ElectedGroupId { get; set; }

        public string? ElectedGroupName { get; set; }

        public string? ElectedMembers { get; set; }

        public ElectedGroupViewModel() { }

        public ElectedGroupViewModel(ElectedGroup electedGroup)
        {
            ElectedGroupId = electedGroup.GroupId;
            ElectedGroupName = electedGroup.GroupName;
            ElectedMembers = (electedGroup.Members != null && electedGroup.Members.Any()) ? string.Join("\r", electedGroup.Members.Select(x => x.MemberFirstName + " " + x.MemberLastName)).Trim() : string.Empty;
        }
    }
}
