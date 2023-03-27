using Chat.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Chat.DataAccess.UI.Models
{
    public class Request
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserId { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string RequestMessage { get; set; } = null!;

        public string? ResponseMessage { get; set; } = null;

        public Request()
        {

        }

        public Request(RequestModel model)
        {
            Id = model.Id;
            Date = model.Date;
            UserId = model.UserId;
            RequestMessage = model.RequestMessage;
            ResponseMessage = model.ResponseMessage;
        }

        public static explicit operator RequestModel(Request entity)
        {
            return new RequestModel()
            {
                Id = entity.Id,
                Date = entity.Date,
                UserId = entity.UserId,
                RequestMessage = entity.RequestMessage,
                ResponseMessage = entity.RequestMessage
            };
        }
    }
}
