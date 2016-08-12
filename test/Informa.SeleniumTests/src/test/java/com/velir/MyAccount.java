package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebElement;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * Created by ishan.kumar on 7/18/2016.
 */
public class MyAccount extends SetupClass {


    By title =By.cssSelector(".page-account__header");
    By subTitle = By.cssSelector(".page-account__table-headline");


    @Test
    public void myAccount() {


        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID") + "@yopmail.com", configuration.getString("password"));


        if(BROWSER.equalsIgnoreCase("Mobile")){

            helper.getURL(ENV+"/my-account/contact-information");
            helper.waitForSeconds(4);
        }
        else {
            helper.click(By.linkText("My Account"));

            helper.click(By.linkText("ACCOUNT SETTINGS"));
        }
        Assert.assertEquals(helper.getElementText(title),"Account Settings");

    }


    @Test
    public void subscriptionsPage(){

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);
        homePage.loginProcess(configuration.getString("emailID")+"@yopmail.com",configuration.getString("password"));

        helper.getURL(ENV + "my-account/subscriptions");

        By header = By.xpath("//div[2]/div/table/thead[1]/tr/td");

        List<String> expected = Arrays.asList("Publication","Subscription Type","Expiration Date","Action");

        if (BROWSER.equalsIgnoreCase("Mobile")){
            header =By.xpath("//div[2]/div/table/thead[2]/tr/td");
            expected = Arrays.asList("SORT BY:","Publication","Subscription Type","Expiration Date");
        }

        helper.waitForSeconds(4);
        Assert.assertEquals(helper.getElementsText(header),expected);



    }

    @Test
    public void emailPreferences(){

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);
        homePage.loginProcess(configuration.getString("emailID")+"@yopmail.com",configuration.getString("password"));

        helper.getURL(ENV + "my-account/email-preferences");

//        By header = By.xpath("//div[2]/div/table/thead[1]/tr/td");
//
//        //div[2]/form/div[1]/label
//
//        List<String> expected = Arrays.asList("Publication","Subscription Type","Expiration Date","Action");
//
//        if (BROWSER.equalsIgnoreCase("Mobile")){
//            header =By.xpath("//div[2]/div/table/thead[2]/tr/td");
//            expected = Arrays.asList("SORT BY:","Publication","Subscription Type","Expiration Date");
//        }

        Assert.assertEquals(helper.getElementText(title),"Email Preferences");
        Assert.assertEquals(helper.getElementText(subTitle), "Newsletter Emails");

        By emailPreferenceButton = By.cssSelector(".button--filled.button--fly-right");


        helper.click(emailPreferenceButton);

        Assert.assertFalse(driver.findElement(By.cssSelector(".alert-success.js-form-success>p")).isDisplayed());



    }

    @Test
    public void UpdateEmailPreferences(){

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);
        homePage.loginProcess(configuration.getString("emailID")+"@yopmail.com",configuration.getString("password"));

        helper.getURL(ENV + "my-account/email-preferences");

        By emailPreferenceButton = By.cssSelector(".button--filled.button--fly-right");

        helper.click(By.id("INV"));

        helper.click(emailPreferenceButton);

        helper.waitForSeconds(4);

        Assert.assertEquals(helper.getElementText(By.cssSelector(".alert-success.js-form-success>p")),"Your preferences have been updated.");



    }



}
