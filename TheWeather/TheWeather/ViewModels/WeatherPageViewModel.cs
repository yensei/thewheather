using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheWeather.Models;
using Xamarin.Forms;

namespace TheWeather.ViewModels
{
    public class WeatherPageViewModel : INotifyPropertyChanged
    {
        private WeatherData data;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public WeatherData Data
        {
            get => data; 
            set
            {
                data = value;
                OnPropertyChanged();
            }
        }
        public ICommand SearchCommand { get; set; }
        public WeatherPageViewModel()
        {
            SearchCommand = new Command(async (searchTerm) =>
            {
                var lat = (searchTerm as string).Split(',')[0];
                var lon = (searchTerm as string).Split(',')[1];
                await GetData($"https://api.weatherbit.io/v2.0/current?lat={lat}&lon={lon}&key=d56f797b1a3444f69c68e7f884a4776c&lang=es");
            });
        }


        private async Task GetData(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonResult = await response.Content.ReadAsStringAsync();
            Data = JsonConvert.DeserializeObject<WeatherData>(jsonResult);
        }
    }
}
