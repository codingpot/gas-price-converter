namespace GasPriceConverter.Shared;

public class VolumeService
{
    public Volume Convert(Volume old, VolumeType to)
    {
        switch (old.Type, to)
        {
            case (VolumeType.Gallon, VolumeType.Liter):
                return new Volume(to, old.Value * 3.785);
            case (VolumeType.Liter, VolumeType.Gallon):
                return new Volume(to, old.Value / 3.785);
            default:
                return old;
        } 
    }
}