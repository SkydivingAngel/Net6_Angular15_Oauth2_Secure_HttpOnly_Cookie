import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {

  public forecasts: WeatherForecast[] = [];
  public baseUrl: string = "";
  public url: string = "";

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;

    alert(this.baseUrl + 'api/weatherforecast');
    //'https://localhost:7137/api/weatherforecast'

    this.url = this.baseUrl + 'api/weatherforecast';

    http.get<WeatherForecast[]>(this.url).subscribe(result => {
      this.forecasts = result;
    }, error => {
      console.error(error)
    }
    );
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
