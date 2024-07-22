using ReverseGeocoding.Interface;

namespace ReverseGeocoding.Tests;

/// <summary>
/// Tests mainly focusing on setting up the ReverseGeocoder class.
/// </summary>
[TestFixture]
public class ReverseGeocoderTests
{
    private const string citiesDb = "cities5000.txt";
    private const string countresDb = "countryInfo.txt";

    private const string testCity1 = "Grajaú";
    private const string testCountry1 = "Brazil";
    private const double testPlaceLatitude1 = -6.48449;
    private const double testPlaceLongitude1 = -46.07212;

    private const string testCity2 = "Chumikan";
    private const string testCountry2 = "Russia";
    private const double testPlaceLatitude2 = 54.30208;
    private const double testPlaceLongitude2 = 136.45948;

    private const string testCity3 = "Nayun";
    private const string testCountry3 = "China";
    private const double testPlaceLatitude3 = 21.92051;
    private const double testPlaceLongitude3 = 99.15254;

    private static string CitiesDbPath => Path.Combine(TestContext.CurrentContext.TestDirectory, citiesDb);
    private static string CountresDbPath => Path.Combine(TestContext.CurrentContext.TestDirectory, countresDb);

    private IReverseGeocoder preSetupReverseGeocoder;

    [SetUp]
    public void SetUp()
    {
        preSetupReverseGeocoder = new ReverseGeocoder(CitiesDbPath, CountresDbPath);
    }

    [TestCase(testPlaceLatitude1, testPlaceLongitude1, testCity1)]
    [TestCase(testPlaceLatitude2, testPlaceLongitude2, testCity2)]
    [TestCase(testPlaceLatitude3, testPlaceLongitude3, testCity3)]
    public void PlacePathTest(double latitude, double longitude, string placeName)
    {
        IReverseGeocoder sut = new ReverseGeocoder(CitiesDbPath);
        var place = sut.GetNearestPlace(latitude, longitude);

        Assert.NotNull(place);
        Assert.AreEqual(placeName, place.Name);
    }

    [TestCase(testPlaceLatitude1, testPlaceLongitude1, testCity1, testCountry1)]
    [TestCase(testPlaceLatitude2, testPlaceLongitude2, testCity2, testCountry2)]
    [TestCase(testPlaceLatitude3, testPlaceLongitude3, testCity3, testCountry3)]
    public void CountryPathTest(double latitude, double longitude, string placeName, string countryName)
    {
        IReverseGeocoder sut = new ReverseGeocoder(CitiesDbPath, CountresDbPath);
        var place = sut.GetNearestPlace(latitude, longitude);
            
        Assert.NotNull(place);
        Assert.AreEqual(placeName, place.Name);
        Assert.NotNull(place.CountryInfo);
        Assert.AreEqual(countryName, place.CountryInfo.Country);
    }

    [TestCase(testPlaceLatitude1, testPlaceLongitude1, testCity1)]
    [TestCase(testPlaceLatitude2, testPlaceLongitude2, testCity2)]
    [TestCase(testPlaceLatitude3, testPlaceLongitude3, testCity3)]
    public void PlaceStreamTest(double latitude, double longitude, string placeName)
    {
        using var fileStream = File.OpenRead(CitiesDbPath);
            
        IReverseGeocoder sut
            = new ReverseGeocoder(fileStream);
        var place = sut.GetNearestPlace(latitude, longitude);

        Assert.NotNull(place);
        Assert.AreEqual(placeName, place.Name);
    }

    [TestCase(testPlaceLatitude1, testPlaceLongitude1, testCity1, testCountry1)]
    [TestCase(testPlaceLatitude2, testPlaceLongitude2, testCity2, testCountry2)]
    [TestCase(testPlaceLatitude3, testPlaceLongitude3, testCity3, testCountry3)]
    public void CountryStreamTest(double latitude, double longitude, string placeName, string countryName)
    {
        using var fileStreamCities = File.OpenRead(CitiesDbPath);
        using var fileStreamCountries = File.OpenRead(CountresDbPath);
            
        IReverseGeocoder sut = new ReverseGeocoder(fileStreamCities, fileStreamCountries);
        var place = sut.GetNearestPlace(latitude, longitude);

        Assert.NotNull(place);
        Assert.AreEqual(placeName, place.Name);
        Assert.NotNull(place.CountryInfo);
        Assert.AreEqual(countryName, place.CountryInfo.Country);
    }

    [TestCase(testPlaceLatitude1, testPlaceLongitude1, testCity1)]
    [TestCase(testPlaceLatitude2, testPlaceLongitude2, testCity2)]
    [TestCase(testPlaceLatitude3, testPlaceLongitude3, testCity3)]
    public void PlaceDbTest(double latitude, double longitude, string placeName)
    {
        var sut = new GeoDb(CitiesDbPath);

        IReverseGeocoder geocoder = new ReverseGeocoder(sut);
        var place = geocoder.GetNearestPlace(latitude, longitude);

        Assert.NotNull(place);
        Assert.AreEqual(placeName, place.Name);
    }

    [TestCase(testPlaceLatitude1, testPlaceLongitude1, testCity1, testCountry1)]
    [TestCase(testPlaceLatitude2, testPlaceLongitude2, testCity2, testCountry2)]
    [TestCase(testPlaceLatitude3, testPlaceLongitude3, testCity3, testCountry3)]
    public void CountryDbTest(double latitude, double longitude, string placeName, string countryName)
    {
        var sut = new GeoDb(CitiesDbPath, CountresDbPath);

        IReverseGeocoder geocoder = new ReverseGeocoder(sut);
        var place = geocoder.GetNearestPlace(latitude, longitude);

        Assert.NotNull(place);
        Assert.AreEqual(placeName, place.Name);
        Assert.NotNull(place.CountryInfo);
        Assert.AreEqual(countryName, place.CountryInfo.Country);
    }

    [TestCase(testPlaceLatitude1, testPlaceLongitude1, testCity1)]
    [TestCase(testPlaceLatitude2, testPlaceLongitude2, testCity2)]
    [TestCase(testPlaceLatitude3, testPlaceLongitude3, testCity3)]
    public void PlaceNamePathTest(double latitude, double longitude, string placeNameReference)
    {
        IReverseGeocoder sut
            = new ReverseGeocoder(CitiesDbPath);
        var placeName = sut.GetNearestPlaceName(latitude, longitude);

        Assert.AreEqual(placeNameReference, placeName);
    }

    /// <summary>
    /// Latitudes range from -90 to 90.
    /// Longitudes range from -180 to 180.
    /// </summary>
    [TestCase(-90, -180)]
    [TestCase(-90, 180)]
    [TestCase(90, -180)]
    [TestCase(90, 180)]
    [TestCase(0, 0)]
    [TestCase(-91, -181)]
    [TestCase(-91, 181)]
    [TestCase(91, -181)]
    [TestCase(91, 181)]
    [TestCase(200, 200)]
    [TestCase(double.MaxValue, double.MaxValue)]
    [TestCase(double.MinValue, double.MaxValue)]
    [TestCase(double.MaxValue, -double.MinValue)]
    [TestCase(double.MinValue, -double.MinValue)]
    [TestCase(double.NaN, double.NegativeInfinity)]
    [TestCase(double.PositiveInfinity, double.PositiveInfinity)]
    [TestCase(double.PositiveInfinity, double.NaN)]
    public void OddCoordinatesTest(double latitude, double longitude)
    {
        Assert.DoesNotThrow(() => preSetupReverseGeocoder.GetNearestPlace(latitude, longitude));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" \t    \t")]
    public void CityPathNullTest(string? parameter)
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new ReverseGeocoder(parameter));
    }

    [Test]
    public void CityStreamNullTest()
    {
        Stream parameter = null;

        // ReSharper disable once ExpressionIsAlwaysNull
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new ReverseGeocoder(parameter));
    }
}