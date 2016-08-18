package com.velir;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.testng.Assert;
import org.testng.annotations.Test;

/**
 * Created by ishan.kumar on 8/17/2016.
 */
public class CookiePolicy extends SetupClass {

    @Test
    public void cookieBanner() {

        //div[2]/div[3]/div/section[1]
        helper.getURL(ENV);

        By dismissButton = By.cssSelector(".dismiss-button.js-dismiss-banner");
        By cookieElement = By.cssSelector(".banner__wrapper");
        By cookieSpan =By.cssSelector(".banner__wrapper>span");
        String expectedText= "This website uses cookies. By continuing to use this site, you agree to our use of cookies. To find out more visit our Cookie Policy.";



        Assert.assertTrue(driver.findElement(cookieElement).isDisplayed());

        Assert.assertEquals(helper.getElementText(cookieSpan), expectedText);

        helper.click(dismissButton);

        Assert.assertFalse(driver.findElement(cookieElement).isDisplayed());


        }


    @Test
    public void cookiePolicyPage() {

        //div[2]/div[3]/div/section[1]
        helper.getURL(ENV);


        By policyPage = By.linkText("Cookie Policy");

        helper.click(policyPage);

        Assert.assertEquals(driver.getCurrentUrl(), ENV + "cookies/");

        Assert.assertTrue(driver.getTitle().toLowerCase().contains("cookie policy"));




    }
    }


