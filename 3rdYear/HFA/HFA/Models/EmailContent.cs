using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HFA.Models
{
    public class EmailContent
    {

        [Required]
        [DisplayName("Message")]
        [StringLength(255)]
        public string Message { get; set; }
        [Required]
        [DisplayName("Subject")]

        public string Subject { get; set; }
       
        public EmailContent() { }
        public EmailContent(string subject, string message)
        {
            Message = message;
            Subject = subject;

        }
    }

}
