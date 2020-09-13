import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  public forecasts: MarvelCharacter[];
  private http: HttpClient;
  private baseUrl: string;
  public currPage: number;
  private currOffset: number;
  private totalOffset: number;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.currPage = 1;
    this.currOffset = 1;
    this.totalOffset = 1450;
    http.get<MarvelCharacter[]>(baseUrl + 'home?offset=1&limit=12').subscribe(result => {
      this.forecasts = result;
      this.baseUrl = baseUrl;
      this.http = http;
      this.totalOffset = result.length > 0 ? result[0].total : 0; 
      //console.log(result);
    }, error => console.error(error));
  }

  previous(): void {
    if (this.currOffset > 12) {
      this.currPage = this.currPage - 1;
      this.currOffset = this.currOffset - 12;
      this.http.get<MarvelCharacter[]>(this.baseUrl + 'home?offset=' + this.currOffset + '&limit=12').subscribe(result => {
        this.forecasts = result;
        //console.log(this.forecasts);
      }, error => console.error(error));
    }
  }

  next(): void {
    if ((this.currOffset+12) < this.totalOffset) {
      this.currPage = this.currPage + 1;
      this.currOffset = this.currOffset + 12;
      this.http.get<MarvelCharacter[]>(this.baseUrl + 'home?offset=' + this.currOffset + '&limit=12').subscribe(result => {
        this.forecasts = result;
        //console.log(this.forecasts);
      }, error => console.error(error));
    }
  }

}

export class FetchDataComponent {
  
}

interface Tumbnail {
  path: string;
  extension: string;
}

interface MarvelCharacter {
  total: number;
  id: string;
  name: string;
  description: string;
  thumbnail: Tumbnail;
}

interface CharacterData {
  total: bigint;
  results: MarvelCharacter[];
}
