using AutoMapper;
using VotingSystem.API.Features.Voting.DTOs;
using VotingSystem.Domain.Entities;

namespace VotingSystem.API.Mappers
{
    public class VoteMappingProfile : Profile
    {
        public VoteMappingProfile()
        {
            // VoteRequestDTO -> Vote 
            CreateMap<VoteRequestDTO, Vote>()
                .ForMember(dest => dest.VotedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Vote -> VoteResponseDTO 
            CreateMap<Vote, VoteResponseDTO>();
        }
    }
}
