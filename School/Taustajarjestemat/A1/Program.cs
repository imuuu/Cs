using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Net;




namespace vscodedemo
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            //string address="http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental";
            Console.WriteLine("hello");
            
            RealTimeCityBikeDataFetcher r=new RealTimeCityBikeDataFetcher();
            OfflineCityBikeDataFetcher offline=new OfflineCityBikeDataFetcher();
            var b=await offline.GetBikeCountInStation(args[0]);
            try
            {
                var a= await r.GetBikeCountInStation(args[0]);
               
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                
            }

            
               
        }
        
    }

    public class BikeRentalStationList
    {
        public List<Station> stations{get;set;}       
    }

    public class Station
    {        
        public int bikesAvailable{get;set;}
        public string name{get;set;}
    }
    public interface ICityBikeDataFetcher
    {
        Task<int> GetBikeCountInStation(string stationName);
    }
    
    
    public class notFoundException : System.Exception
    {
        public notFoundException() { }
        public notFoundException(string message) : base(message) { }
        public notFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected notFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class OfflineCityBikeDataFetcher : ICityBikeDataFetcher
    {
        string address="https://github.com/vsillan/game-server-programming-course/blob/master/assignments/bikedata.txt";
       
        
        public async Task<int> GetBikeCountInStation(string stationName)
        {
            //Console.WriteLine("lol");
            WebClient wc = new WebClient();
            string site =  wc.DownloadString(address);
            //var test = JsonConvert.DeserializeObject
            //Console.WriteLine(site);
            return 0;
        }

        
    }
    public class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {
        string address="http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental";

        public async Task<int> GetBikeCountInStation(string stationName)
        {
            if (stationName.Any(char.IsDigit))
            {
                throw new System.ArgumentException("No numbers thanks");
            }
            
            HttpClient httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync(address);
            var bikeRentalStationList = JsonConvert.DeserializeObject<BikeRentalStationList>(json);
            
            Console.WriteLine(bikeRentalStationList.stations[0].bikesAvailable);

            foreach(var s in bikeRentalStationList.stations)
            {
                if(s.name == stationName)
                {
                    return s.bikesAvailable;
                }
            }

            throw new notFoundException("Not found");
        }
    }
}
