using System;

namespace MarvelAngularApp
{
    public class MarvelCharacter
    {
        public int total { get; set; }
        public string id { get; set; } // (int, optional) : The unique ID of the character resource.,
        public string name { get; set; } // (string, optional) : The name of the character.,
        public string description { get; set; } // (string, optional) : A short bio or description of the character.,
        public string modified { get; set; } // (Date, optional) : The date the resource was most recently modified.,
        public string resourceURI { get; set; } // (string, optional): The canonical URL identifier for this resource.,
        //public string urls { get; set; } // (Array[Url], optional): A set of public web site URLs for the resource.,
        public Thumbnail thumbnail { get; set; } // (Image, optional): The representative image for this character.,
        //public string comics(ComicList, optional): A resource list containing comics which feature this character.,
        //public string stories(StoryList, optional): A resource list of stories in which this character appears.,
        //public string events(EventList, optional): A resource list of events in which this character appears.,
        //public string series(SeriesList, optional): A resource list of series in which this character appears.
  }

    public class Thumbnail
    {
        public string path { get; set; }
        public string extension { get; set; }
    }

    public class CharacterData 
    {
      public int total { get; set; }
      public MarvelCharacter[] results { get; set; }
    }

 /*   public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }*/
}
