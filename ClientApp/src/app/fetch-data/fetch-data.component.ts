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
  public page = 1;
  public pageSize = 10;
  public number: number = 0;
  public clickedPage = 1;
  
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;

    this.url = this.baseUrl + 'api/weatherforecast';

    //alert(this.url);
    //'https://localhost:7137/api/weatherforecast'
    http.get<WeatherForecast[]>(this.url).subscribe(result => {
      this.forecasts = result;
    }, error => {
      console.error(error)
    }
    );
  }

  onPaginationClick(clickedPage: number) {
    console.log(clickedPage);
    this.clickedPage = clickedPage;
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
