using AutoMapper;
using ConwayGameOfLife.DatabaseModels;
using ConwayGameOfLife.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Infrastructure.Mapping
{
    public class BoardMappingProfile : Profile
    {
        public BoardMappingProfile()
        {
            CreateMap<BoardDTO, Board>().ReverseMap();
        }
    }
}
