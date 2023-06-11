using AutoMapper;
using dbthirstthing.DTO;
using dbthirstthing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbthirstthing
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserModel, UserDTO>()

                    .ForMember(dest => dest.displayname, opt => opt.MapFrom(src => src.displayname))
                    .ForMember(dest => dest.login, opt => opt.MapFrom(src => src.login))
                    .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email));
            });
        }
    }
}