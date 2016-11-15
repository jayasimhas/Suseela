package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.testng.Assert;
import org.testng.annotations.Test;

/**
 * Created by ishan.kumar on 8/24/2016.
 */
public class Entitlement extends SetupClass {

    @Test
      public void freeArticle(){

        helper.getURL(ENV+"MT066441/Aus-Clinical-Evidence-Guidance-Tackles-Leading-Cause-Of-Medtech-Rejections");



        helper.log(expectedData.getString("freeArticle.text"));
        Assert.assertTrue(helper.getElementText(By.cssSelector(".article-body-content")).contains(expectedData.getString("freeArticle.text")));

        Assert.assertFalse(helper.isElementPresent(By.cssSelector(".article-call-to-action--sign-in-up")));

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID"), configuration.getString("password"));

        Assert.assertTrue(helper.getElementText(By.cssSelector(".article-body-content")).contains(expectedData.getString("freeArticle.text")));

        Assert.assertFalse(helper.isElementPresent(By.cssSelector(".article-call-to-action--sign-in-up")));

    }

    @Test
    public void unentitledUser(){

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID") , configuration.getString("password"));


        //navigate to article
        helper.getURL(ENV + "MT066291/Heads-roll-as-JNJ-seeks-to-strengthen-ailing-medtech-biz");

        //title
        Assert.assertEquals(helper.getElementText(By.cssSelector(".article-body-content>h1")),"Heads roll as JNJ seeks to strengthen ailing medtech biz");


        //author
        Assert.assertEquals(helper.getElementText(By.cssSelector(".article-author__name")),"Tina Tan");

        //summary
        Assert.assertEquals(helper.getElementText(By.cssSelector(".article-executive-summary>h4")), "Executive Summary");
        Assert.assertTrue(helper.getElementText(By.cssSelector(".article-executive-summary>p")).contains(expectedData.getString("unentitledUser.summary")));
        helper.log("executive summary is present");

        //body content not present
        Assert.assertFalse(helper.getElementText(By.cssSelector(".article-body-content")).contains("J&J saw a downturn in medtech revenue in 2014"));


        //subscribe
        Assert.assertEquals(helper.getElementText(By.cssSelector(".article-call-to-action__title")), "Please refresh if content doesn’t display. Not a subscriber?");


        //related content
        // Assert.assertEquals(helper.getElementText(By.cssSelector(".article-related-content>h2")), "Related Content");


        //related topics
        Assert.assertEquals(helper.getElementText(By.cssSelector(".article-topics>h2")), "Topics");

    }
}
