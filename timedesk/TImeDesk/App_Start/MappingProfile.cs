using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TImeDesk.Models.Database;
using TImeDesk.ViewModel;

namespace TImeDesk.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<TimeEntryViewModel, TimeEntry>();
            Mapper.CreateMap<TimeEntry, TimeEntryViewModel>();

            Mapper.CreateMap<TimeEntry, TimeEntry>()
                .ForMember(x => x.StartedDate, opt => opt.Ignore())
                .ForMember(x => x.ApplicationUserId, opt => opt.Ignore())
                .ForMember(x => x.WorkspaceId, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());

            Mapper.CreateMap<UserWorkspaceRoles, UserWorkspaceRoles>()
                  .ForMember(c => c.Id, opt => opt.Ignore());

            Mapper.CreateMap<Project, Project>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Created, opt => opt.Ignore())
                .ForMember(c => c.WorkSpaceId, opt => opt.Ignore())
                .ForMember(c => c.ApplicationUserId, opt => opt.Ignore())
                .ForMember(c => c.StatusId, opt => opt.Ignore())
                .ForMember(c => c.CurrencyId, opt => opt.Ignore());
        }
    }
}