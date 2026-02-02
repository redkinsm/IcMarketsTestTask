using AutoMapper;
using IcMarketsTestTask.API.Application.DTO;
using IcMarketsTestTask.API.Domain.Entities;

namespace IcMarketsTestTask.API.Infrastructure.Mappings;

public class BlockcypherProfile : Profile
{
    public BlockcypherProfile()
    {
        CreateMap<BlockchainSnapshotDto, Blockcypher>()
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(_ => DateTime.UtcNow)
            )
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore()
            );
    }
}