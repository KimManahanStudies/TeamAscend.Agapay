namespace TeamAscend.Agapay.Web.Models.OpenWeatherMap;

public class WeatherResponse
{
    public OWM_Coord Coord { get; set; }
    public List<OWM_Weather> Weather { get; set; }
    public string Base { get; set; }
    public OWM_Main Main { get; set; }
    public int Visibility { get; set; }
    public OWM_Wind Wind { get; set; }
    public OWM_Rain Rain { get; set; }
    public OWM_Clouds Clouds { get; set; }
    public long Dt { get; set; }
    public OWM_Sys Sys { get; set; }
    public int Timezone { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Cod { get; set; }
}