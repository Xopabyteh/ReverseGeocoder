namespace ReverseGeocoding.Tests;

public class ReverseGeocoderPrecisionTests
{
    private const string k_TestData = """
                       -Czechia
                       49.343377, 13.617472
                       50.468343, 13.584513
                       50.663738, 15.529093
                       49.933973, 16.814891
                       49.406291, 17.742547
                       50.823615, 14.561304
                       49.569951, 18.367510
                       50.265902, 12.464196
                       49.215187, 14.080909
                       49.455985, 16.575268
                       48.888239, 16.963279
                       -Slovakia
                       49.070216, 18.729270
                       48.151542, 17.943252
                       48.849374, 20.483803
                       49.061695, 21.684790
                       -Poland
                       51.441691, 15.516970
                       49.855784, 20.752944
                       51.875411, 22.834904
                       53.356272, 20.939388
                       54.173608, 21.933757
                       -Belarus
                       52.540360, 24.672605
                       53.695089, 24.848465
                       55.123606, 27.427609
                       52.121102, 29.820309
                       -Ukraine
                       48.601563, 23.693884
                       50.594983, 24.748572
                       51.492904, 32.746619
                       48.717672, 32.900427
                       47.692639, 37.075232
                       47.855075, 35.932654
                       47.031311, 30.422153
                       -Moldova
                       48.123860, 27.345154
                       47.270931, 29.161191
                       45.940354, 28.406686
                       46.815405, 28.388626
                       -Germany
                       48.644869, 12.433544
                       50.195180, 11.694475
                       50.876301, 13.043276
                       51.902409, 14.151880
                       53.327002, 13.800822
                       52.016266, 7.407874
                       -France
                       47.782333, 6.367620
                       49.178458, 5.887225
                       50.123464, 2.857042
                       43.079584, 2.801611
                       43.897277, -0.708967
                       -Spain
                       42.573568, -1.685197
                       38.101425, -4.058895
                       38.959923, -0.583600
                       42.963484, -7.168370
                       42.906092, -1.472021
                       -Portugal
                       38.593250, -8.030661
                       41.493099, -7.168370
                       38.994535, -7.238424
                       37.858468, -8.453471
                       """;

    private ReverseGeocoder _sut = null!;
    private IList<TestData> _testData = null!;

    private const string k_PlacesDb = "cities1000.txt";
    private const string k_CountryInfoDb = "countryInfo.txt";
    private static string PlacesDbPath => Path.Combine(Environment.CurrentDirectory, k_PlacesDb);
    private static string CountryInfoDbPath => Path.Combine(Environment.CurrentDirectory, k_CountryInfoDb);

    [SetUp]
    public void Setup()
    {
        _sut = new(PlacesDbPath, CountryInfoDbPath);
        _testData = LoadTestData();
    }

    [Test]
    public void TestPrecision()
    {
        foreach (var testData in _testData)
        {
            var place = _sut.GetNearestPlace(testData.Latitude, testData.Longitude);
            Assert.AreEqual(testData.CountryName, place.CountryInfo.Country);
        }
    }

    private IList<TestData> LoadTestData()
    {
        var lines = k_TestData.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var result = new List<TestData>();
        string? country = null;
        foreach (var line in lines)
        {
            if (line.StartsWith("-"))
            {
                country = line[1..].Trim();
            }
            else
            {
                var parts = line.Split(", ");
                var lat = double.Parse(parts[0]);
                var lon = double.Parse(parts[1]);
                result.Add(new TestData(lat, lon, country!));
            }
        }

        return result;
    }

    private readonly record struct TestData(double Latitude, double Longitude, string CountryName);
}