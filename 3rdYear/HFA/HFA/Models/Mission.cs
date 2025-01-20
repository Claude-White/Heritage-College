using System.ComponentModel.DataAnnotations;

namespace HFA.Models
{
    public class Mission
    {
        [Key]
        public int? MissionId { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        public string? Description { get; set; }

        public Mission() {
            MissionId = 0;
            Description = null;
        }

        public Mission(string description)
        {
            MissionId = 0;
            Description = description;
        }

        public Mission(int missionId, string description)
        {
            MissionId = missionId;
            Description = description;
        }
    }
}
