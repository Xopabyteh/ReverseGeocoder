using BenchmarkDotNet.Running;
using ReverseGeocoding;
using ReverseGeocoding.Benchmarks;
using ReverseGeocoding.Interface;
using ReverseGeocoding.Model;

//BenchmarkRunner.Run<Bencho>();

ReverseGeocoder reverseGeocoder = null!;

const string placesDb = "cities500.txt";

const string countryInfoDb = "countryInfo.txt";

var placesDbPath = Path.Combine(Environment.CurrentDirectory, placesDb);

var countryInfoDbPath = Path.Combine(Environment.CurrentDirectory, countryInfoDb);

reverseGeocoder = new(placesDbPath, countryInfoDbPath);


//IPlaceInfo place = new PlaceInfo(0, 0);
//for (int i = 0; i < 10000; i++)
//{
//    place = reverseGeocoder.GetNearestPlace(50, 14);
//}

//Console.WriteLine((PlaceInfo)place);
////while (true)
////{
////    // 49.8758946,17.4671687
////    double lat = 0;
////    double lon = 0;
////    try
////    {
////        var inp = Console.ReadLine();
////        var se = inp.Split(", ");
////        lat = double.Parse(se[0]);
////        lon = double.Parse(se[1]);
////    }
////    catch (Exception e)
////    {
////        Console.WriteLine("wtf");
////        continue;
////    }
////    var place = reverseGeocoder.GetNearestPlace(lat, lon);
////    Console.WriteLine((PlaceInfo)place);
////}