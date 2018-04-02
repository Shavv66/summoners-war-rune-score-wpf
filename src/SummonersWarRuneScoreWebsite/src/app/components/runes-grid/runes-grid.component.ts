import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'runes-grid',
  templateUrl: './runes-grid.component.html',
  styleUrls: ['./runes-grid.component.css']
})
export class RunesGridComponent implements OnInit {
  columns = [
    { prop: 'name' },
    { name: 'Gender' },
    { name: 'Company', sortable: false }
  ];

  rows = [
    {
        "name": "Ethel Price",
        "gender": "female",
        "company": "Johnson, Johnson and Partners, LLC CMP DDC",
        "age": 22
    },
    {
        "name": "Claudine Neal",
        "gender": "female",
        "company": "Sealoud",
        "age": 55
    },
    {
        "name": "Beryl Rice",
        "gender": "female",
        "company": "Velity",
        "age": 67
    }]

  constructor() {
  }

  ngOnInit() {
    
  }

  reloadData() {
  }

}
