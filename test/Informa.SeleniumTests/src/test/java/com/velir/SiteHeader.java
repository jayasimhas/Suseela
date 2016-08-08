package com.velir;

import com.velir.baseclass.SetupClass;
import org.openqa.selenium.By;
import org.testng.Assert;
import org.testng.annotations.Test;

/**
 * Created by ishan.kumar on 3/21/2016.
 */
public class SiteHeader extends SetupClass {



    @Test
    public void informaBar(){

        helper.getURL(ENV);

        helper.log("Description - Informa bar present, more content is shown when clicked, left content is present, right content is present");

        Assert.assertFalse(helper.isElementPresent(By.cssSelector(".informa-ribbon.show")));
        helper.click(By.cssSelector(".informa-ribbon__title"));

        helper.waitTillElementLocated(By.cssSelector(".informa-ribbon__left>p"));

        String actualLeft= driver.findElement(By.cssSelector(".informa-ribbon__left>p")).getText();

        String actualRight= driver.findElement(By.cssSelector(".informa-ribbon__right>p")).getText();

        Assert.assertEquals(actualLeft, expectedData.getString("informaBar.expectedLeft"));
        helper.log("Left content is present");

        Assert.assertTrue(actualRight.contains(expectedData.getString("informaBar.expectedRight")));
        helper.log("Right content is present");

        Assert.assertTrue(helper.isElementPresent(By.cssSelector(".informa-ribbon.show")));
        helper.log("Bar is present");


    }

    @Test
    public void advertisingComponent(){

        helper.getURL(ENV);
        By adLocator = By.cssSelector(".advertising--leaderboard>a>img");

        Assert.assertTrue(helper.isImagePresent(adLocator));
        helper.log("Ad image is present");
        Assert.assertTrue(driver.findElement(adLocator).getAttribute("src").contains(expectedData.getString("advertisingComponent.partialSource")));
    }

    @Test
    public void logo(){

        helper.getURL(ENV);

        helper.log("Description - Verify that Logo Image is present and sourced from"+ expectedData.getString("logo.partialURL"));
        By logoLocator= By.cssSelector(".header__logo>a>img");

        Assert.assertTrue(helper.isImagePresent(logoLocator));
        helper.log("Logo Image is present");
        Assert.assertEquals(driver.findElement(logoLocator).getAttribute("src"),ENV+expectedData.getString("logo.partialURL"));


    }

    @Test
    public void siteSearch(){

        helper.getURL(ENV);

        helper.log("Description - Verify search field is present and placeholder text is");
        By searchLocator= By.cssSelector(".inf-standard-form-field.header-search__field");
        //By searchIcon =By.xpath("/html/body/header/div[4]/div[3]/form/svg[1]");

        Assert.assertTrue(helper.isElementPresent(searchLocator));
        helper.log("Search field is present");

        Assert.assertEquals(driver.findElement(searchLocator).getAttribute("placeholder"), expectedData.getString("siteSearch.placeholder"));
        helper.log("placeholder text is - "+expectedData.getString("siteSearch.placeholder"));
    }

    @Test

    public void accountInformation(){

        helper.getURL(ENV);
        helper.log("Description - Register and Login options are present");

        By registerElement=By.cssSelector(".header-account-access>div:nth-child(1)");

        By signInElement=By.cssSelector(".header-account-access>div:nth-child(2)");


        Assert.assertTrue(helper.isElementPresent(registerElement));

        Assert.assertEquals(driver.findElement(registerElement).getText(), expectedData.getString("accountInformation.register"));

        helper.log("Register element and text is present");
        Assert.assertTrue(helper.isElementPresent(signInElement));
        Assert.assertEquals(driver.findElement(signInElement).getText(), expectedData.getString("accountInformation.signin"));
        helper.log("Sign In element and text is present");


    }
}
