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

        homePage.loginProcess(configuration.getString("emailID"), configuration.getString("password"));



            helper.click(By.linkText("My Account"));


            Assert.assertTrue(helper.getElementText(title).contains("Account Settings") || helper.getElementText(title).contains("Subscriptions"));


    }


    @Test
    public void subscriptionsPage(){

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);
        homePage.loginProcess(configuration.getString("emailID"),configuration.getString("password"));

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
        homePage.loginProcess(configuration.getString("emailID"), configuration.getString("password"));

        helper.getURL(ENV + "my-account/email-preferences");


        helper.waitForSeconds(2);

        Assert.assertEquals(helper.getElementText(title), "Email Preferences");
        Assert.assertEquals(helper.getElementText(subTitle), "Newsletter Emails");

        By emailPreferenceButton = By.cssSelector(".button--filled.button--fly-right");


        //helper.log(driver.findElement(emailPreferenceButton).getAttribute("disabled"));
        Assert.assertEquals(driver.findElement(emailPreferenceButton).getAttribute("disabled"),"true");


        Assert.assertFalse(driver.findElement(By.cssSelector(".alert-success.js-form-success>p")).isDisplayed());



    }

    @Test
    public void UpdateEmailPreferences(){

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);
        homePage.loginProcess(configuration.getString("emailID") , configuration.getString("password"));

        helper.getURL(ENV + "my-account/email-preferences");

        By emailPreferenceButton = By.cssSelector(".button--filled.button--fly-right");

        helper.click(By.cssSelector("#INV"));

        helper.click(emailPreferenceButton);

        helper.waitForSeconds(4);

        Assert.assertEquals(helper.getElementText(By.cssSelector(".alert-success.js-form-success>p")), "Your preferences have been updated.");

        helper.waitForSeconds(4);

        helper.click(By.id("INV"));

        helper.click(emailPreferenceButton);

        helper.waitForSeconds(4);

        Assert.assertEquals(helper.getElementText(By.cssSelector(".alert-success.js-form-success>p")), "Your preferences have been updated.");



    }


    //only on scrip and not mobile
    @Test
    public void actionColumn(){

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);
        homePage.loginProcess("exipresoon@example.com", "password");

        By renew= By.xpath("//div/span/span/a");
        Assert.assertEquals(helper.getElementText(renew),"Click here to renew");

        helper.click(renew);

        helper.waitForSeconds(4);
       ArrayList<String> tabs2 = new ArrayList<String> (driver.getWindowHandles());
        if(tabs2.size()==2){
        driver.switchTo().window(tabs2.get(1));
        Assert.assertEquals(driver.getCurrentUrl(), "https://scrip.pharmamedtechbi.com/subscribe");
            driver.close();
            driver.switchTo().window(tabs2.get(0)); }
        else {
            Assert.assertEquals(driver.getCurrentUrl(), ENV + "subscribe");
        }

    }



}
