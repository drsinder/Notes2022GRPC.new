namespace Notes2022.Proto
{
    public sealed partial class WeatherForecast
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    }
}
