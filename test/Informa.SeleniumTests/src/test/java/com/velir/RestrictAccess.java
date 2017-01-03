package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.Arrays;
import java.util.List;

/**
 * Created by Ishan on 10/17/2016.
 */
public class RestrictAccess extends SetupClass {

    @Test
    public void displayOfpage() {


        helper.getURL(ENV + "IKQA1");

        Assert.assertEquals(helper.getElementText(By.cssSelector(".general-header__section-title")), "Section Title: - QA Testing");

       // Assert.assertEquals(helper.getElementText(By.cssSelector(".article-executive-summary>h4")), "Executive Summary");


        Assert.assertTrue(helper.getElementText(By.cssSelector(".article-executive-summary")).contains("Summary - QA Testing - Summary Summary"));

        helper.log(helper.getElementsText(By.cssSelector(".article-call-to-action__subtitle")));

        List<String> actualValues = helper.getElementsText(By.cssSelector(".article-call-to-action__subtitle"));

       Assert.assertEquals(actualValues.get(0),"Sign in to continue reading.");

     //   Assert.assertTrue(actualValues.get(1).contains("Request a free trial today!\n" +
     //           "FREE TRIAL"));
        //Click here! and get is subscribed for yourself. MEDTECH
        //Register for our free email digests ROSE
    }

    @Test
    public void freeWithRegistration() {

;
        helper.getURL(ENV + "IKQA1");

        Assert.assertEquals(helper.getElementText(By.cssSelector(".general-header__section-title")), "Section Title: - QA Testing");

        //Assert.assertEquals(helper.getElementText(By.cssSelector(".article-executive-summary>h4")), "Executive Summary");

        Assert.assertTrue(helper.getElementText(By.cssSelector(".article-executive-summary")).contains("Summary - QA Testing - Summary Summary"));

        Assert.assertFalse(helper.isElementPresent(By.cssSelector(".GeneralRTEComponent")));



        helper.getURL(ENV);
        HomePage homePage = new HomePage(driver);
        homePage.loginProcess(configuration.getString("emailID") , configuration.getString("password"));
        helper.getURL(ENV + "IKQA1");
        helper.waitForSeconds(2);


        Assert.assertEquals(helper.getElementText(By.cssSelector(".general-header__section-title")), "Section Title: - QA Testing");

        Assert.assertFalse(helper.isElementPresent(By.cssSelector(".article-executive-summary")));

        Assert.assertTrue(helper.getElementText(By.cssSelector(".GeneralRTEComponent")).contains("Here is the body text for this restricted general content page"));


    }

    @Test
    public void freeWithEntitle() {



        helper.getURL(ENV + "IKQA2");

        Assert.assertEquals(helper.getElementText(By.cssSelector(".general-header__section-title")), "Section Title: - QA Testing");

       // Assert.assertEquals(helper.getElementText(By.cssSelector(".article-executive-summary>h4")), "Executive Summary");

        Assert.assertTrue(helper.getElementText(By.cssSelector(".article-executive-summary")).contains("Summary - QA Testing - Summary Summary"));

        Assert.assertFalse(helper.isElementPresent(By.cssSelector(".GeneralRTEComponent")));

        helper.getURL(ENV);
        HomePage homePage = new HomePage(driver);
        homePage.loginProcess("fullallpubs@example.com", "password");
        helper.getURL(ENV + "IKQA2");
        helper.waitForSeconds(2);

        Assert.assertEquals(helper.getElementText(By.cssSelector(".general-header__section-title")), "Section Title: - QA Testing");

        Assert.assertTrue(helper.isElementPresent(By.cssSelector(".article-executive-summary")));

        Assert.assertTrue(helper.getElementText(By.cssSelector(".GeneralRTEComponent")).contains("Here is the body text for this restricted general content page"));

    }


}
