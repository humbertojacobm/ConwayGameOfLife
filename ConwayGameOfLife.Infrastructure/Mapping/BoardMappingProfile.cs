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
            CreateMap<BoardDTO, Board>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Cells, opt => opt.MapFrom(src => To2D(src.Cells)))
                .ForMember(dest => dest.Step, opt => opt.MapFrom(src => src.Step));

            CreateMap<Board, BoardDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.Cells, opt => opt.MapFrom(src => ToJagged(src.Cells)))
                .ForMember(dest => dest.Step, opt => opt.MapFrom(src => src.Step));
        }

        private bool[,] To2D(bool[][] jagged)
        {
            if (jagged is null || jagged.Length == 0)
                return new bool[0, 0];

            int height = jagged.Length;
            int width = jagged[0].Length;
            var result = new bool[height, width];
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    result[r, c] = jagged[r][c];
                }
            }
            return result;
        }

        private bool[][] ToJagged(bool[,] twoD)
        {
            int height = twoD.GetLength(0);
            int width = twoD.GetLength(1);
            var jagged = new bool[height][];
            for (int r = 0; r < height; r++)
            {
                jagged[r] = new bool[width];
                for (int c = 0; c < width; c++)
                {
                    jagged[r][c] = twoD[r, c];
                }
            }
            return jagged;
        }
    }
}
