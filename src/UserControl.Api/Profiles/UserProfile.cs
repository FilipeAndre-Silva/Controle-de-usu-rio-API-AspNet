using AutoMapper;
using UserControl.Domain.Models;

namespace UserControl.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ViewModels.User.CreateUserViewModel, User>().ReverseMap();
            CreateMap<ViewModels.User.UpdateUserViewModel, User>().ReverseMap();
            CreateMap<ViewModels.User.UserViewModel, User>().ReverseMap();
        }
    }
}