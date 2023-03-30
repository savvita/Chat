using Chat.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Chat.DataAccess.UI.Models
{
    public class Ability
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public Ability()
        {
        }

        public Ability(AbilityModel model)
        {
            Id = model.Id;
            Name = model.Name;
        }

        public static explicit operator AbilityModel(Ability entity)
        {
            var model = new AbilityModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };


            return model;
        }
    }
}
