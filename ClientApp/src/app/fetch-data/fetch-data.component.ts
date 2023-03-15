import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {

  public forecasts: WeatherForecast[] = [];
  public baseUrl: string = "";
  public url: string = "";
  public page = 0;
  public pageSize = 10;
  public number: number = 0;
  public clickedPage = 1;

  public forecastslength = 0;

  public http: HttpClient;


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.http = http;
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.url = this.baseUrl + 'api/weatherforecast';
    
    //alert(this.url);
    //'https://localhost:7137/api/weatherforecast'

    var params = new HttpParams()
      .set("pageIndex", '0')
      .set("pageSize", this.pageSize.toString());

    //http.get<WeatherForecast[]>(this.url, { params }).subscribe(result => {
    //  this.forecasts = result;
    //}, error => {
    //  console.error(error)
    //});

    this.http.get<Result>(this.url, { params }).subscribe(result => {
      this.forecasts = result.data;
      this.forecastslength = result.count;
    }, error => {
      console.error(error)
    });

  }

  changePageSize(newSize: number) {
    this.page = 0;
    this.pageSize = Number(newSize);
    this.loadData();
  }

  onPaginationClick(clickedPage: number) {

    console.log(clickedPage);
    this.clickedPage = clickedPage;

    var params = new HttpParams()
      .set("pageIndex", (this.clickedPage - 1).toString())
      .set("pageSize", this.pageSize.toString());

    //this.http?.get<WeatherForecast[]>(this.url, { params }).subscribe(result => {
    //  this.forecasts = result;
    //}, error => {
    //  console.error(error)
    //});

    //alert(this.clickedPage.toString());

    this.http?.get<Result>(this.url, { params }).subscribe(result => {
      this.forecasts = result.data;
      this.forecastslength = result.count;
    }, error => {
      console.error(error)
    });


  }
}

interface Result {
  data: WeatherForecast[];
  count: number;
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
