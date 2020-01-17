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

  public render(): void {
    this.domElement.innerHTML = `
      <div class="${styles.inputfields}">
        <div class="${styles.container}">
          <div class="${styles.row}">
            <div class="${styles.column}">
              <span class="${styles.title}">Welcome to Sample Demo SharePoint APP!</span> <br><br>
            <form action="#">
                <label for="input1">Team Name :</label>
                <input class="${styles.input1}" type="text" placeholder="Team Name">
                <label for="input2">Channel Name :</label>
                <div class="${styles.tip}" data-tip="Add multiple channels">
                  <input class="${styles.input2}" type="text" placeholder="Channel Name"/>
                </div>
                <br><br><br>
                <input class="${styles.submit}" type="submit" value="Submit">
            </form>
          </div>
        </div>
      </div>`;
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
