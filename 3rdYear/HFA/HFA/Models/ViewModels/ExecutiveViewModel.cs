using System.ComponentModel.DataAnnotations;

namespace HFA.Models.ViewModels
{
    public class ExecutiveViewModel
    {
        [Key]
        public int ExecutiveId { get; set; }

        public string? Name { get; set; }

        public string? Position { get; set; }

        public ExecutiveViewModel() { }

        public ExecutiveViewModel(Executive executive)
        {
            ExecutiveId = executive.ExecutiveId;
            Name = executive.FirstName + " " + executive.LastName;
            Position = executive.Position?.Trim().Replace("\r|\n", "");
        }
    }
}
