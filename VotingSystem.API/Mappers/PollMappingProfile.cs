using AutoMapper;
using VotingSystem.API.Features.Polls.DTOs;
using VotingSystem.Domain.Entities;

namespace VotingSystem.API.Mappers
{
    public class PollMappingProfile : Profile
    {
        public PollMappingProfile()
        {
            // CreatePollDTO -> Poll Entity
            CreateMap<CreatePollDTO, Poll>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src =>
                    src.Options.Select(option => new PollOption { OptionText = option }).ToList()
                ));

            // Poll Entity -> PollResponseDTO
            CreateMap<Poll, PollResponseDTO>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            // PollOption Entity -> PollOptionResponseDTO
            CreateMap<PollOption, PollOptionResponseDTO>();

            // UpdatePollDTO -> Poll Entity
            CreateMap<UpdatePollDTO, Poll>()
                .ForMember(dest => dest.Options, opt => opt.Ignore()); // options handled separately
        }
    }
}
