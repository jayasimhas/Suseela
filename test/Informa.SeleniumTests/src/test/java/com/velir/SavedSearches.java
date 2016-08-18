package com.velir;

import com.velir.baseclass.SetupClass;
import com.velir.pageobject.HomePage;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;
import org.testng.Assert;
import org.testng.annotations.Test;

import java.util.Arrays;
import java.util.List;

/**
 * Created by ishan.kumar on 8/17/2016.
 */
public class SavedSearches extends SetupClass {



    @Test
    public void itemsOnPage() {

        By title = By.cssSelector(".sortable-table__col--title");

        List<String> expected = Arrays.asList("cancer","informa","informa qa");

        navigateLoginSearchPage();

        Assert.assertEquals(helper.getElementsText(title),expected);

    }


    @Test
    public void searchRedirect() {


        navigateLoginSearchPage();

        helper.click(By.linkText("cancer"));

        Assert.assertEquals(driver.getCurrentUrl(), ENV + "search#?q=cancer");


    }

    @Test
    public void removeButton() {


        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        By cancelLocator = By.cssSelector(".lightbox-modal.js-remove-search-modal>div:nth-child(1)");


        By removeLocator =By.cssSelector(".js-lightbox-modal-trigger");

        homePage.loginProcess(configuration.getString("emailID2") + "@yopmail.com", configuration.getString("password"));


        String newSearch = "drug"+helper.randomString();
        helper.getURL(ENV + "search#?q=" + newSearch);

        helper.click(By.cssSelector(".js-save-search"));

        helper.click(By.cssSelector(".pop-out__sign-in-submit"));

        helper.getURL(ENV + "my-account/saved-searches");

        helper.click(removeLocator);

        helper.click(cancelLocator);

        Assert.assertFalse(driver.findElement(cancelLocator).isDisplayed());

        for (WebElement element:driver.findElements(removeLocator)){

            element.click();

            helper.waitForSeconds(2);
            helper.click(By.cssSelector(".lightbox-modal__submit"));

        }



        Assert.assertFalse(helper.isElementPresent(By.linkText(newSearch)));

    }




    //Common method
    private void navigateLoginSearchPage() {
        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID") + "@yopmail.com", configuration.getString("password"));

        helper.getURL(ENV + "my-account/saved-searches");


    }

}
