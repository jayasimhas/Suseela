package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebElement;
import org.testng.Assert;
import org.testng.annotations.Test;
import java.util.UUID;

/**
 * Created by ishan.kumar on 8/23/2016.
 */
public class NewsletterSignup extends SetupClass {


    @Test
    public void componentDisplay() {


        helper.getURL(ENV);

        if(!PUBLICATION_NAME.equalsIgnoreCase("Stage.Scrip")){

            Assert.assertTrue(helper.getElementText(By.cssSelector(".newsletter-signup-before-submit>h3")).contains("Newsletter"));

            Assert.assertTrue(helper.isElementPresent(By.cssSelector(".newsletter-signup-before-submit")));


            Assert.assertEquals(helper.getElementText(By.cssSelector(".js-newsletter-signup-submit")), "SIGN UP"); }
        else {

            Assert.assertTrue(helper.getElementText(By.cssSelector(".newsletter-signup-before-submit>h3")).contains("Final regression"));

            Assert.assertTrue(helper.isElementPresent(By.cssSelector(".newsletter-signup-before-submit")));


            Assert.assertEquals(helper.getElementText(By.cssSelector(".js-newsletter-signup-submit")), "SIGN UP NOW");

        }


    }

    @Test
    public void signup() {

//        String uuid = UUID.randomUUID().toString();
//        System.out.println("uuid = " + uuid);


        helper.getURL(ENV);


        helper.sendKeys(By.name("userName"), "fullallpubs@example.com");

        helper.click(By.cssSelector(".js-newsletter-signup-submit"));

        //Assert.assertEquals(helper.getElementText(By.xpath("Manage Preferences")), "MANAGE PREFERENCES");

        helper.waitForSeconds(2);

        By chromeManageLocator = By.partialLinkText("MANAGE PREFERENCES");
        By ffManageLocator = By.partialLinkText("Manage Preferences");

        if(BROWSER.equalsIgnoreCase("FF"))
            Assert.assertEquals(helper.getElementText(ffManageLocator), "MANAGE PREFERENCES");
        else
            Assert.assertEquals(helper.getElementText(chromeManageLocator), "MANAGE PREFERENCES");

        Assert.assertTrue(helper.isElementPresent(By.cssSelector(".newsletter-signup-after-submit")));

    }

    @Test
    public void subscribedUser() {

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);


        homePage.loginProcess(configuration.getString("emailID4") , configuration.getString("password"));

        Assert.assertFalse(helper.isElementPresent(By.cssSelector(".newsletter-signup-before-submit>h3")));

        Assert.assertFalse(helper.isElementPresent(By.cssSelector(".newsletter-signup-before-submit")));


        Assert.assertFalse(helper.isElementPresent(By.cssSelector(".js-newsletter-signup-submit")));


    }


}