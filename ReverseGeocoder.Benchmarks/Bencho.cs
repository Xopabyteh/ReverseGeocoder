using BenchmarkDotNet.Attributes;

namespace ReverseGeocoding.Benchmarks;

[MemoryDiagnoser]
public class Bencho
{
    private ReverseGeocoder _reverseGeocoder = null!;
    private const string k_PlacesDb = "cities1000.txt";
    private const string k_CountryInfoDb = "countryInfo.txt";
    private static string PlacesDbPath => Path.Combine(Environment.CurrentDirectory, k_PlacesDb);
    private static string CountryInfoDbPath => Path.Combine(Environment.CurrentDirectory, k_CountryInfoDb);

    private const string k_TestCity1 = "Grajaú";
    private const string k_TestCountry1 = "Brazil";
    private const double k_TestPlaceLatitude1 = -6.48449;
    private const double k_TestPlaceLongitude1 = -46.07212;

    [GlobalSetup]
    public void Setup()
    {
        _reverseGeocoder = new(PlacesDbPath, CountryInfoDbPath);
    }

    [Benchmark]
    public void SinglePlace()
    {
        var placeName = _reverseGeocoder.GetNearestPlaceName(k_TestPlaceLatitude1, k_TestPlaceLongitude1);
    }

    [Benchmark]
    public void DoIt100Times()
    {
        for (int i = 0; i < 100; i++)
        {
            var placeName = _reverseGeocoder.GetNearestPlaceName(k_TestPlaceLatitude1, k_TestPlaceLongitude1);
        }
    }
}