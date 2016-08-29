package com.velir;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.testng.Assert;
import org.testng.annotations.Test;

/**
 * Created by ishan.kumar on 7/27/2016.
 */
public class Homepage extends SetupClass {

    @Test
    public void components(){

        //div[2]/div[3]/div/section[1]
        helper.getURL(ENV);

        By homepageArticles = By.cssSelector(".article-preview__inner-wrapper");
        By homepageImage = By.cssSelector(".article-preview__image");


        Assert.assertTrue(driver.findElements(homepageArticles).get(0).isDisplayed());

        Assert.assertTrue(driver.findElements(homepageArticles).size()>=3);

        if (ENV.contains("scrip") || ENV.contains("pink"))
        {
          Assert.assertTrue(helper.isImagePresent(homepageImage));
        }


    }
}
