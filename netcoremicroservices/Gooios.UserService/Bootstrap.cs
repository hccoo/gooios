using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gooios.Infrastructure.Events;
using Gooios.UserService.Domain.Events;
using Gooios.UserService.Events;
using Gooios.UserService.Domain.Aggregates;
using AutoMapper;
using Gooios.UserService.Applications.Dtos;

namespace Gooios.UserService
{
    public static class Bootstrap
    {
        public static void SubscribeEvents()
        {
            //IocProvider.GetService<IEventBus>().Subscribe<VerificationCreatedEvent>(new SendSmsHandler());
        }

        public static void InitMapper()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.CreateMap<CookAppUser, CookAppUserDto>();
                config.CreateMap<CookAppPartnerLoginUser, CookAppPartnerLoginUserDto>();
            });
            // only during development, validate your mappings; remove it before release
            configuration.AssertConfigurationIsValid();
            MapperProvider.Mapper = configuration.CreateMapper();
            
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<User, UserDTO>().ForMember(dest => dest.RoleNames, opt => opt.Ignore()).ForMember(dest => dest.OrganizationName, opt => opt.Ignore());
            //    //cfg.CreateMap<UserDTO, User>();
            //    //cfg.CreateMap<InvoiceForm, InvoiceFormDTO>().ForMember(dest => dest.FromOrganizationName, opt => opt.Ignore())
            //    //                                            .ForMember(dest => dest.ToOrganizationName, opt => opt.Ignore())
            //    //                                            .ForMember(dest => dest.StatusDesc, opt => opt.Ignore());
            //});
        }
    }
}
