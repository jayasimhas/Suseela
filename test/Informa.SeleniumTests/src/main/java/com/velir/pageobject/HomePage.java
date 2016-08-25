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


        helper.waitForSeconds(2);

        //helper.waitTillElementLocated(By.name("username"));

        helper.sendKeys(By.name("username"),username);

        helper.sendKeys(By.name("password"), password);


        helper.waitForSeconds(2);

        helper.click(By.xpath("//form/div[2]/div[2]/button"));


       // helper.click(By.cssSelector(".button--filled.sign-in__submit"));

       // helper.click(By.cssSelector("div.js - login - container > form > div.pop-out__form-actions-wrapper > div.sign-in__submit-wrapper"));
       // helper.click(By.cssSelector("button.button--filled.sign-in__submit"));

       // helper.click(By.linkText("SIGN IN"));
        helper.waitForSeconds(2);


        // driver.findElement(By.cssSelector("div.header-account-access__friendly-greeting.firepath-matching-node")).click();
    }






}
