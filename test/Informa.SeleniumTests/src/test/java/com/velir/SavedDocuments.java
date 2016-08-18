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
 * Created by ishan.kumar on 8/18/2016.
 */
public class SavedDocuments extends SetupClass {

    @Test
    public void itemsOnPage() {

        By title = By.cssSelector(".sortable-table__col.js-sortable-table-sorter.l-no-wrap");
        By titleMobile=By.xpath("//thead[2]/tr/td");

        By article=By.xpath("//table/tbody/tr[1]/td");

        List<String> expectedArticle = Arrays.asList("Pink Sheet", "Another Test Story", "18 Aug 2016", "REMOVE");

        List<String> expected = Arrays.asList("Publication", "Title", "Date");
        List<String> expectedMobile = Arrays.asList("SORT BY:", "Title", "Date");

        navigateLoginBookmarkPage();

        if(BROWSER.equalsIgnoreCase("Mobile")){
            Assert.assertEquals(helper.getElementsText(titleMobile), expectedMobile);
        }
        else {
            Assert.assertEquals(helper.getElementsText(title), expected);
        }

        Assert.assertEquals(helper.getElementsText(article),expectedArticle);

    }

    //Don't run on Mobile
    @Test
    public void removeButton() {

        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        By cancelLocator = By.cssSelector(".dismiss-button.js-close-lightbox-modal>svg");


        By removeLocator =By.cssSelector(".js-lightbox-modal-trigger");

        homePage.loginProcess(configuration.getString("emailID2") + "@yopmail.com", configuration.getString("password"));


        helper.getURL(ENV + "SC071032/GlycoVaxyn-AG");

        //bookmarking
        helper.click(By.cssSelector(".action-flags.article-prologue__share-bar>ul>li:nth-child(4)"));

        //helper.click(By.cssSelector(".pop-out__sign-in-submit"));

        helper.getURL(ENV + "my-account/saved-articles");

        helper.click(removeLocator);

        helper.click(cancelLocator);

        Assert.assertFalse(driver.findElement(cancelLocator).isDisplayed());

        for (WebElement element:driver.findElements(removeLocator)){

            element.click();

            helper.waitForSeconds(2);
            helper.click(By.cssSelector(".lightbox-modal__submit"));

        }



        Assert.assertFalse(helper.isElementPresent(By.linkText("GlycoVaxyn AG")));

    }


    private void navigateLoginBookmarkPage() {
        helper.getURL(ENV);

        HomePage homePage = new HomePage(driver);

        homePage.loginProcess(configuration.getString("emailID") + "@yopmail.com", configuration.getString("password"));

        helper.getURL(ENV + "my-account/saved-articles");


    }
}
