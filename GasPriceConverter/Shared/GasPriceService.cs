using GasPriceConverter.Shared.Models;

namespace GasPriceConverter.Shared;

public class GasPriceService
{
    private readonly VolumeService _volumeService;
    private readonly CurrencyService _currencyService;

    public GasPriceService(VolumeService volumeService, CurrencyService currencyService)
    {
        _volumeService = volumeService;
        _currencyService = currencyService;
    }

    public GasPrice Convert(GasPrice old, CurrencyType currencyType, VolumeType volumeType)
    {
        return ConvertToVolumeType(ConvertToCurrency(old, currencyType), volumeType);
    }

    private GasPrice ConvertToVolumeType(GasPrice gasPrice, VolumeType volumeType)
    {
        return gasPrice with
        {
            Currency = gasPrice.Currency with
            {
                Value = gasPrice.Currency.Value / _volumeService.Convert(gasPrice.Volume, volumeType).Value,
            },
            VolumeType = volumeType,
        };
    }

    private GasPrice ConvertToCurrency(GasPrice gasPrice, CurrencyType currencyType)
    {
        return gasPrice with
        {
            Currency = _currencyService.Convert(gasPrice.Currency, currencyType),
        };
    }
}