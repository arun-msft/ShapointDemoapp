import { Version } from '@microsoft/sp-core-library';
import {
  BaseClientSideWebPart,
  IPropertyPaneConfiguration,
  PropertyPaneTextField
} from '@microsoft/sp-webpart-base';
import { escape } from '@microsoft/sp-lodash-subset';

import styles from './InputfieldsWebPart.module.scss';
import * as strings from 'InputfieldsWebPartStrings';

export interface IInputfieldsWebPartProps {
  description: string;
}

export default class InputfieldsWebPart extends BaseClientSideWebPart<IInputfieldsWebPartProps> {
  public createdgroup: any;

  public render(): void {
    this.domElement.innerHTML = `
      <div class="${styles.inputfields}">
        <div class="${styles.container}">
          <div class="${styles.row}">
            <div class="${styles.column}">
              <span class="${styles.title}">Welcome to Sample Demo SharePoint APP!</span> <br><br>
                <label for="input1">Team Name</label>
                <input type="text" id="teamname" placeholder="Team Name"> <br />
                <label for="input2">Channel Name</label>
                <div class="${styles.tip}" data-tip="Add multiple channels">
                  <input type="text" id="channelname" placeholder="Channel Name"/>
                </div>
                <br><br>
                <input class="${styles.submit}" id="submitbtn" type="submit" value="Submit">
          </div>
        </div>
      </div>`;
      document.getElementById('submitbtn').addEventListener('click', () => this.handleSubmit());
    
      
  }
  
  public handleSubmit(){
   var team:any=document.getElementById('teamname');
   var channels:any=document.getElementById('channelname');
    fetch('https://teamscreatedemo.azurewebsites.net/api/Team/Create?TeamName='+team.value+'&Channels='+channels.value).then(r => r.json()).then(res=>
    {
      this.createdgroup=res;
      this.domElement.innerHTML=`
      <div class="${styles.inputfields}">
        <div class="${styles.container}">
          <div class="${styles.row}">
            <div class="${styles.column}">
            <label>Team Name :</label>
              <span class="${styles.title}">${this.createdgroup.displayName}</span> <br><br>
              <div>
              <input  id="members" type="text" name="members" placeholder="Group Members"/>
             </div>
            <br>
            <input class="${styles.submit}" id="addmembers"  type="submit" value="Add">
          </div>
        </div>
      </div>`
      document.getElementById('addmembers').addEventListener('click', () => this.addMember());
    });

  }
 
  public addMember(){
    var members:any=document.getElementById('members');
    fetch('https://teamscreatedemo.azurewebsites.net/api/Team/AddMember?EmailId='+members.value+'&groupid='+this.createdgroup.id).then(r => r.json()).then(res=>
    {
    
    alert("user added");
     this.domElement.innerHTML = 
    `<div class="${styles.inputfields}">
        <div class="${styles.container}">
          <div class="${styles.row}">
            <div class="${styles.column}">
              <span class="${styles.title}">Welcome to Sample Demo SharePoint APP!</span> <br><br>
                <label for="input1">Team Name :</label>
                <input  type="text" name="teamname" id="teamname"  placeholder="Team Name"><br>
                <label for="input2">Channel Name :</label>
                <div class="${styles.tip}"  data-tip="Add multiple channels comma saperatedly">
                  <input id="channelname" type="text" name="channelname" placeholder="Channel Name"/>
                </div>
                <br><br>
                <input class="${styles.submit}" id="submitbtn"  type="submit" value="Submit">
          </div>
        </div>
      </div>`;
      document.getElementById('submitbtn').addEventListener('click', () => this.handleSubmit());
    })
  }
  protected get dataVersion(): Version {
    return Version.parse('1.0');
  }

  protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
    return {
      pages: [
        {
          header: {
            description: strings.PropertyPaneDescription
          },
          groups: [
            {
              groupName: strings.BasicGroupName,
              groupFields: [
                PropertyPaneTextField('description', {
                  label: strings.DescriptionFieldLabel
                })
              ]
            }
          ]
        }
      ]
    };
  }
}
