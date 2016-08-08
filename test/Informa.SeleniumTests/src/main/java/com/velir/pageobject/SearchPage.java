package com.velir.pageobject;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

import java.util.List;

/**
 * Created by ishan.kumar on 3/29/2016.
 */
public class SearchPage extends GeneralPage {


    public SearchPage(WebDriver driver) {
        super(driver);
    }

    public void performSearch(String searchTerm){

        driver.findElement(By.id("js-search-field")).clear();
        driver.findElement(By.id("js-search-field")).sendKeys(searchTerm);

        helper.click(By.cssSelector("svg.search-bar__icon.search-bar__icon--search"));
        helper.waitForSeconds(2);
    }

    public int getCount(String searchString1) {
        int count =0;
        By titleLocator = By.xpath("//section[3]/div/div/a");
        List<String> actualList = helper.getElementsText(titleLocator);
        for (String actualString: actualList){
            if (actualString.toLowerCase().contains(searchString1)){
                count++;
            }
        }

        helper.log("Total titles with text " + searchString1 + " are " + count);
        return count;
    }
}
