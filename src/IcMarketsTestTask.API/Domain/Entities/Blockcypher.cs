using IcMarketsTestTask.API.Domain.Enums;

namespace IcMarketsTestTask.API.Domain.Entities;

public class Blockcypher
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long Height { get; set; }
    public DateTime Time { get; set; }
    public DateTime CreatedAt { get; set; }
}
