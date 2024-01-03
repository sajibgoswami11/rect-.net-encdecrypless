using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
        public class TaskImageMapper : Profile
        {
            public TaskImageMapper()
            {
                CreateMap<TaskImageDto, TaskImage>()
                    .ForMember(dest => dest.IMAGE_ID, act => act.MapFrom(src => src.ImageId))
                    .ForMember(dest => dest.IMAGE_PATH, act => act.MapFrom(src => src.ImagePath))
                    .ForMember(dest => dest.REFERENCE_ID, act => act.MapFrom(src => src.ReferenceId))
                    .ForMember(dest => dest.ISDELETE, act => act.MapFrom(src => src.IsDelete))
                    ;

                CreateMap<TaskImage, TaskImageDto>()
                    .ForMember(dest => dest.ImageId, act => act.MapFrom(src => src.IMAGE_ID))
                    .ForMember(dest => dest.ImagePath, act => act.MapFrom(src => src.IMAGE_PATH))
                    .ForMember(dest => dest.ReferenceId, act => act.MapFrom(src => src.REFERENCE_ID))
                    .ForMember(dest => dest.IsDelete, act => act.MapFrom(src => src.ISDELETE))
                    ;
            }
        }
    
}
