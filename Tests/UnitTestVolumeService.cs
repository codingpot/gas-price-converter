using GasPriceConverter.Shared;
using Xunit;

namespace Tests;

public class UnitTestVolumeService
{
  [Fact]
  public void TestVolumeServiceGallonToLiter()
  {
    VolumeService volumeService = new VolumeService();
    Volume expected = new Volume(VolumeType.Liter, 3.785);
    Volume actual = volumeService.Convert(new Volume(VolumeType.Gallon, 1.0), VolumeType.Liter);
    Assert.Equal(expected, actual);
  }
}
