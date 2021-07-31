namespace CarAdvertisementSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using static CarAdvertisementSystem.Data.Constants.User;
    public class User:IdentityUser
    {
        [MaxLength(UserNameMaxLength)]
        public string Name { get; set; }
    }
}
