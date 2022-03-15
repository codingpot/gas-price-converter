using GasPriceConverter.Shared.Models;

namespace GasPriceConverter.Shared;

public record GasPrice(Currency Currency, VolumeType VolumeType)
{
    public Volume Volume => new(VolumeType, 1.0);
}