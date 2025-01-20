using System.ComponentModel.DataAnnotations;

namespace HFA.Models
{
    public class FrequentlyAskedQuestion
    {
        [Key]
        public int FaqId { get; set; }

        [Required(ErrorMessage = "The Question is required.")]
        public string? Question { get; set; }

        [Required(ErrorMessage = "The Answer is required.")]
        public string? Answer { get; set; }

        public FrequentlyAskedQuestion()
        {
            FaqId = 0;
            Question = string.Empty;
            Answer = string.Empty;
        }

        public FrequentlyAskedQuestion(string question, string answer)
        {
            FaqId = 0;
            Question = question;
            Answer = answer;
        }

        public FrequentlyAskedQuestion(int faqId, string question, string answer)
        {
            FaqId = faqId;
            Question = question;
            Answer = answer;
        }
    }
}
