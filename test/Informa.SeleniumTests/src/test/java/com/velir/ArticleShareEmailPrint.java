package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by ishan.kumar on 8/23/2016.
 */
public class ArticleShareEmailPrint extends SetupClass {

    @Test
    public void bookmarkArticle() {



        By removeLocator =By.cssSelector(".js-lightbox-modal-trigger");

        HomePage homePage = new HomePage(driver);


        helper.getURL(ENV + "SC071032/GlycoVaxyn-AG");

        //bookmarking
        helper.click(By.cssSelector(".action-flags.article-prologue__share-bar>ul>li:nth-child(4)"));

        Assert.assertTrue(helper.isElementPresent(By.name("username")));
        helper.log("username textbox is present");

        Assert.assertTrue(helper.isElementPresent(By.name("password")));
        helper.log("password textbox is present");

        helper.getURL(ENV);

        homePage.loginProcess(configuration.getString("emailID3"), configuration.getString("password"));


        helper.getURL(ENV + "SC071032/GlycoVaxyn-AG");
        helper.waitForSeconds(2);
        //bookmarking
        helper.click(By.cssSelector(".action-flags.article-prologue__share-bar>ul>li:nth-child(4)"));


        helper.getURL(ENV + "my-account/saved-articles");

        helper.waitForSeconds(2);

        Assert.assertTrue(helper.isElementPresent(By.partialLinkText("GlycoVaxyn")));


        for (WebElement element:driver.findElements(removeLocator)){

            helper.waitForSeconds(1);
            element.click();

            helper.waitForSeconds(2);
            helper.click(By.cssSelector(".lightbox-modal__submit"));

        }


    }



}
