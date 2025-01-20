using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HFA.Models
{
    public class ElectedMember
    {
        public int? MemberId { get; set; }

        [Required(ErrorMessage = "The first name of all members is required.")]
        public string? MemberFirstName { get; set; }

        [Required(ErrorMessage = "The last name of all members is required.")]
        public string? MemberLastName { get; set; }

        [ForeignKey(nameof(ElectedGroup))]
        public int? GroupId { get; set; }
        public virtual ElectedGroup? Group { get; set; } = null!;

        public ElectedMember(string memberfirstname, string? memberlastname, int groupId)
        {
            MemberFirstName = memberfirstname;
            MemberLastName = memberlastname;
            MemberId = 0;
            GroupId = groupId;
        }

        public ElectedMember(int memberid,  string? memberfirstname, string? memberlastname, int groupId)
        {
            MemberId = memberid;
            MemberFirstName = memberfirstname;
            MemberLastName = memberlastname;
            GroupId = groupId;
        }

        public ElectedMember()
        {
            MemberId = 0;
            MemberFirstName = string.Empty;
            MemberLastName = string.Empty;
            GroupId = 0;
        }
    }
}
