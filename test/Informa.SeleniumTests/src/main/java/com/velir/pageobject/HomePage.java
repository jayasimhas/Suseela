package com.velir.pageobject;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

/**
 * Created by ishan.kumar on 7/24/2015.
 */
public class HomePage extends GeneralPage  {

     // private WebDriver driver;
    public HomePage(WebDriver driver) {
        super(driver);
    }


    By headerLogo=By.cssSelector(".logo");


    public String getHeaderText(){

        helper.waitForSeconds(20);
        //WebElement headerElement = driver.findElement(headerLogo);
        //Reporter.log(headerElement.getText());
        return driver.getTitle();

    }

    public void loginProcess(String username, String password) {

        helper.click(By.xpath("//div[2]/span"));



        helper.waitTillElementLocated(By.name("username"));

       // helper.sendKeys(By.name("username"),username);

       // helper.sendKeys(By.name("password"), password);

        helper.sendKeys(By.xpath("//div[2]/div[4]/div[2]/div/div[2]/form/input[1]"),username);

        helper.sendKeys(By.xpath("//div[2]/div[4]/div[2]/div/div[2]/form/input[2]"), password);

        helper.waitForSeconds(2);

        helper.click(By.xpath("//div[4]/div[2]/div/div[2]/form/div[2]/div[2]/button"));


        helper.waitForSeconds(2);


    }






}
