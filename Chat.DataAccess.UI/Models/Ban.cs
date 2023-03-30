using Chat.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Chat.DataAccess.UI.Models
{
    public class Ban
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Description { get; set; } = null!;
        public int? Value { get; set; }
        [MaxLength(10)]
        public string? Units { get; set; }

        public Ban()
        {
        }

        public Ban(BanModel model)
        {
            Id = model.Id;
            Description = model.Description;
            Value = model.Value;
            Units = model.Units;
        }

        public static explicit operator BanModel(Ban entity)
        {
            var model = new BanModel()
            {
                Id = entity.Id,
                Description = entity.Description,
                Value = entity.Value,
                Units = entity.Units
            };


            return model;
        }
    }
}
