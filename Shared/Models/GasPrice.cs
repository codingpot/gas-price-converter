using gas_price_converter.Shared.Models;

namespace gas_price_converter.Shared;

public record GasPrice(Currency Currency, VolumeType VolumeType)
{
    public Volume Volume => new(VolumeType, 1.0);
}