using EF.Entities.Models;

namespace EF.Entities.ViewModels
{

    public class UserTokenViewModel
    {
        public Player User { get; set; }
        public string AccessToken { get; set; }
    }
}
